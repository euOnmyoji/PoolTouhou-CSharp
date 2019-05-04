using SharpDX.WIC;
using static PoolTouhou.Utils.Util;
using Bitmap = SharpDX.Direct2D1.Bitmap;

namespace PoolTouhou.UI.Button {
    public static class ButtonsResources {
        public static readonly Bitmap TITLE01 = loadBitMapFromFile(@"res/title/title01.png", PixelFormat.Format32bppPRGBA);
    }
    public interface IButton : IDrawble {
        void select();
        void click();
        string getName();
        void unselect();
    }
}