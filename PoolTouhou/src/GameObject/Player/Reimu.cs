using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using PoolTouhou.Bullets;
using PoolTouhou.Games;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using static PoolTouhou.PoolTouhou;
using static PoolTouhou.Utils.Math;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using Math = System.Math;
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
        private short power = 100;
        private readonly Bitmap bitmap;
        private readonly Bitmap child;
        private readonly ICollection<BulletBase> bullets;
        private readonly GameRegion region;

        private Child[] children;

        private long lastDrawTime;

        public Reimu(IGame g) {
            bullets = g.Bullets;
            region = g.GameRegion;
            //image from J u e  Y i n g ()
            bitmap = Util.LoadBitMapFromFile("res/player/reimu/reimu1.png", PixelFormat.Format32bppPRGBA);
            child = Util.LoadBitMapFromFile("res/player/reimu/child.png", PixelFormat.Format32bppPRGBA);

            children = new[] {new Child(this), new Child(this), new Child(this), new Child(this)};
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
            slow = input.slow > 0;
            float delta = MoveUtil.GetCoordinateDelta(ref location, slow ? 2 : 4);
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
                    switch (power / 100) {
                        case 0: {
                            //no child
                            break;
                        }
                        case 1: {
                            break;
                        }
                        case 2: {
                            break;
                        }
                        case 3: {
                            break;
                        }
                        case 4: {
                            break;
                        }
                        default: {
                            PoolTouhou.Logger.Info($"wrong power:{power}");
                            power = power > 400 ? (short) 400 : (short) 0;
                            break;
                        }
                    }
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
            double delta = 0;
            if (lastDrawTime == 0) {
                lastDrawTime = Watch.ElapsedTicks;
            } else {
                delta = Math.PI * ((Watch.ElapsedTicks - lastDrawTime) / (double) Stopwatch.Frequency);
            }
            renderTarget.Transform = RotateMatrix(delta, x, y - 48);
            for (int i = 0; i < power / 100; ++i) {
                children[i].Draw(renderTarget);
            }
            renderTarget.Transform = RotateMatrix(0, 0, 0);
        }

        private struct Child {
            private float x;
            private float y;
            private float ax;
            private float ay;
            internal Reimu reimu;

            internal Child(Reimu reimu) {
                this.reimu = reimu;
                x = 0;
                y = 0;
                ax = 0;
                ay = 0;
            }

            public void Draw(RenderTarget renderTarget) {
                renderTarget.DrawBitmap(
                    reimu.child,
//                    new RawRectangleF(reimu.x - 8, reimu.y - 48, reimu.x + 8, reimu.y - 32),
                    new RawRectangleF(-8, -8, 8, 8),
                    1,
                    BitmapInterpolationMode.Linear,
                    new RawRectangleF(0, 0, 16, 16)
                );
            }
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


    public class ReimuMagicPin : BulletBase {
        private float x;
        private float y;

        public ReimuMagicPin(float x, float y) {
            this.x = x;
            this.y = y;
        }

        public override bool IsCollided(ICollidable that) {
            return false;
        }

        public override ICollection<IBoundingBox> BoundingBoxes => null;

        public override void Draw(RenderTarget renderTarget) {
            using var brush = new SolidColorBrush(DxResource.RenderTarget, new RawColor4(0.5f, 0.125f, 0.125f, 1));
            renderTarget.FillRectangle(new RawRectangleF(x - 3, y - 5, x + 3, y + 5), brush);
        }

        public override void Update(ref InputData input) {
            y -= 10;
            dead = y < GameState.game.GameRegion.minY;
        }

        public override float X {
            get => x;
            set => x = value;
        }

        public override float Y {
            get => x;
            set => x = value;
        }

        public override float Ax { get; set; }
        public override float Ay { get; set; }
        public override float Vx { get; set; }
        public override float Vy { get; set; }

        public PointF Point {
            get => new PointF(x, y);
            set {
                x = value.X;
                y = value.Y;
            }
        }
    }
}