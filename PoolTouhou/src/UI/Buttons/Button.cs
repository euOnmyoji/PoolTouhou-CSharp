using System;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using PixelFormat = SharpDX.WIC.PixelFormat;

namespace PoolTouhou.UI.Buttons {
    public static class ButtonsResources {
        public static readonly Bitmap TITLE01 = Util.LoadBitMapFromFile(
            @"res/title/title01.png",
            PixelFormat.Format32bppPRGBA
        );
    }

    public abstract class Button : IDrawable {
        protected DateTime? selectTime;

        public void Select() {
            selectTime = DateTime.Now;
        }
        public abstract int Click();
        public abstract string GetName();

        public void Unselect() {
            selectTime = null;
        }

        public abstract void Draw(RenderTarget renderTarget);

        protected static void GetOffset(double ms, out int x, out int y) {
            //16.666666666 ms ≈ 1tick
            //so just using 17 ms
            int tick = (int)(ms / 34);
            switch (tick) {
                case 1:
                case 7: {
                    x = -1;
                    y = 0;
                    break;
                }
                case 2: {
                    x = -1;
                    y = -1;
                    break;
                }
                case 3: {
                    x = 1;
                    y = 0;
                    break;
                }
                case 5: {
                    x = 0;
                    y = -1;
                    break;
                }
                case 6: {
                    x = 1;
                    y = 0;
                    break;
                }
                default: {
                    x = 0;
                    y = 0;
                    break;
                }
            }
        }
    }
}