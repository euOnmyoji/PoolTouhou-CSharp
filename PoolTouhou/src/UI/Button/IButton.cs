using SharpDX.Direct2D1;
using static PoolTouhou.Utils.Util;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using PixelFormat = SharpDX.WIC.PixelFormat;

namespace PoolTouhou.UI.Button {
    public static class ButtonsResources {
        public static readonly Bitmap TITLE01 = loadBitMapFromFile(
            @"res/title/title01.png",
            PixelFormat.Format32bppPRGBA
        );
    }

    public abstract class Button : IDrawable {
        public abstract void select();
        public abstract void click();
        public abstract string getName();
        public abstract void unselect();

        public abstract void draw(RenderTarget renderTarget);

        protected static void getOffset(int selected, out int x, out int y) {
            if (selected == 0) {
                x = y = 0;
                return;
            }
            switch (selected / 2) {
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