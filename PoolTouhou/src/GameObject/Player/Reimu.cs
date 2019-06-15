using System.Collections.Generic;
using System.Drawing;
using PoolTouhou.Bullets;
using PoolTouhou.Games;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using PixelFormat = SharpDX.WIC.PixelFormat;

namespace PoolTouhou.GameObject.Player {
    /// <summary>
    ///灵梦(x)
    /// 红点(√)
    /// </summary>
    public class Reimu : Player {
        private float x = 800;
        private float y = 800;
        private const byte dx = 16;
        private const float dy = 47 / 2f;
        private const byte shootInterval = 7;
        private byte shootCd;
        private readonly SharpDX.Direct2D1.Bitmap bitmap;
        private readonly ICollection<IBullet> bullets;
        private readonly GameRegion region;

        public Reimu(IGame g) {
            bullets = g.Bullets;
            region = g.GameRegion;z
            //image from J u e  Y i n g ()
            bitmap = Util.LoadBitMapFromFile("res/player/reimu1.png", PixelFormat.Format32bppPRGBA);
        }

        public float X {
            get => x;
            set {
                if (value + dx > region.maxX) {
                    x = region.maxX - dx;
                } else if (value - dx < region.minX) {
                    x = region.minX + dx;
                } else {
                    x = value;
                }
            }
        }

        public float Y {
            get => y;
            set {
                if (value + dy > region.maxY) {
                    y = region.maxY - dy;
                } else if (value - dy < region.minY) {
                    y = region.minY + dy;
                } else {
                    y = value;
                }
            }
        }

        public override void Update(ref InputData input) {
            var location = input.MoveLocation;
            float delta = MoveUtil.GetCoordinateDelta(ref location, input.slow > 0 ? 2 : 4);
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
            renderTarget.DrawBitmap(
                bitmap,
                new RawRectangleF(x - dx, y - dy, x + dx, y + dy),
                1,
                BitmapInterpolationMode.Linear
            );
        }

        public override bool IsCollided(ICollidable that) {
            return false;
        }

        public override ICollection<IBoundingBox> BoundingBoxes => null;

        public override PointF Point {
            get => new PointF(X, Y);
            set {
                X = value.X;
                Y = value.Y;
            }
        }

        public override void Dispose() {
            bitmap?.Dispose();
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