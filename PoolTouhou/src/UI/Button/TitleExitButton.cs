using System.Windows.Forms;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace PoolTouhou.UI.Button {
    public class TitleExitButton : Button {
        private int selected = -1;
        private const float dx = 64;
        private const float dy = 16;
        private static RawRectangleF selectedRf = new RawRectangleF(0, 140, dx, 140 + dy);
        private static RawRectangleF unselectedRf = new RawRectangleF(128, 140, 128 + dx, 140 + dy);

        public override void draw(RenderTarget renderTarget) {
            var size = renderTarget.Size;
            float xScala = size.Width / 640;
            float yScala = size.Height / 480;
            float width = size.Width / 2;
            float height = size.Height / 2;
            ref var bitmapRf = ref unselectedRf;
            if (selected > 0) {
                ++selected;
                bitmapRf = ref selectedRf;
            }

            getOffset(selected, out int xOffset, out int yOffset);


            renderTarget.DrawBitmap(
                ButtonsResources.TITLE01,
                new RawRectangleF(
                    width + 10 * xScala + xOffset,
                    height + 80 * yScala + yOffset,
                    width + (10 + dx) * xScala + xOffset,
                    height + (80 + dy) * yScala + yOffset
                ),
                1,
                BitmapInterpolationMode.Linear,
                bitmapRf
            );
        }

        public override void select() {
            selected = 1;
        }

        public override void click() {
            Application.Exit();
        }

        public override string getName() {
            return "TitleExit";
        }

        public override void unselect() {
            selected = -1;
        }
    }
}