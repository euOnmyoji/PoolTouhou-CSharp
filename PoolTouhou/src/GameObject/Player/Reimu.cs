using System.Collections.Generic;
using System.Drawing;
using PoolTouhou.Bullets;
using PoolTouhou.Games;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using Brush = SharpDX.Direct2D1.Brush;

namespace PoolTouhou.GameObject.Player {
    public class Reimu : Player {
        private float x = 800;
        private float y = 900;
        private const byte shootInterval = 7;
        private byte shootCd;
        private readonly Brush brush;
        private readonly ICollection<IBullet> bullets;
        private readonly GameRegion region;

        public Reimu(IGame g) {
            bullets = g.Bullets;
            region = g.GameRegion;
            brush = new SolidColorBrush(PoolTouhou.DxResource.RenderTarget, new RawColor4(1, 0, 0, 1));
        }

        public float X {
            get => x;
            set {
                if (value > region.maxX) {
                    x = region.maxX;
                } else if (value < region.minX) {
                    x = region.minX;
                } else {
                    x = value;
                }
            }
        }

        public float Y {
            get => y;
            set {
                if (value > region.maxY) {
                    y = region.maxY;
                } else if (value < region.minY) {
                    y = region.minY;
                } else {
                    y = value;
                }
            }
        }

        public override void Update(ref InputData input) {
            var location = input.MoveLocation;
            float delta = MoveUtil.GetCoordinateDelta(ref location);
            if ((location & MoveLocation.UP) != 0) {
                Y -= delta;
            }
            if ((location & MoveLocation.DOWN) != 0) {
                Y += delta;
            }
            if ((location & MoveLocation.LEFT) != 0) {
                X -= delta;
            }
            if ((location & MoveLocation.RIGHT) != 0) {
                X += delta;
            }
            if (shootCd > 0) {
                --shootCd;
            }
            if (input.shoot > 0) {
                if (shootCd == 0) {
                    shootCd = shootInterval;
                    bullets.Add(new ReimuMagicPin(x - 5, y));
                    bullets.Add(new ReimuMagicPin(x + 5, y));
                }
            }
        }

        public override void Draw(RenderTarget renderTarget) {
            renderTarget.FillEllipse(new Ellipse(new RawVector2(x, y), 10, 10), brush);
        }

        public override bool IsCollided(ICollidable that) {
            return false;
        }

        public override ICollection<IBoundingBox> BoundingBoxes { get; }

        public override PointF Point {
            get => new PointF(X, Y);
            set {
                X = value.X;
                Y = value.Y;
            }
        }

        public override void Dispose() {
            brush?.Dispose();
        }
    }


    public class ReimuMagicPin : IBullet {
        private float x;
        private float y;

        public ReimuMagicPin(float x, float y) {
            this.x = x;
            this.y = y;
        }

        public bool IsCollided(ICollidable that) {
            return false;
        }

        public ICollection<IBoundingBox> BoundingBoxes => null;

        public void Draw(RenderTarget renderTarget) {
            using var brush = new SolidColorBrush(
                PoolTouhou.DxResource.RenderTarget,
                new RawColor4(0.5f, 0.125f, 0.125f, 1)
            );
            renderTarget.FillRectangle(new RawRectangleF(x - 3, y - 5, x + 3, y + 5), brush);
        }

        public void Update(ref InputData input) {
            y -= 10;
        }

        public PointF Point {
            get => new PointF(x, y);
            set {
                x = value.X;
                y = value.Y;
            }
        }

        public bool IsDead => y < PoolTouhou.GameState.game.GameRegion.minY;
    }
}