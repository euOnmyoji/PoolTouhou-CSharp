using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace PoolTouhou.UI.Button {
    public class GameStartButton : IButton {
        private const float dx = 128;
        private const float dy = 15;
        private static RawRectangleF selectedRf = new RawRectangleF(0, 0, dx, dy);
        private static RawRectangleF unselectedRf = new RawRectangleF(128, 0, 128 + dx, dy);
        private int selected = -1;

        public void draw(RenderTarget renderTarget) {
            var size = renderTarget.Size;
            float xScala = size.Width / 640;
            float yScala = size.Height / 480;
            float width = size.Width / 2;
            float height = size.Height / 2;
            ref var mapRf = ref unselectedRf;
            if (selected > 0) {
                ++selected;
                mapRf = ref selectedRf;
            }

            renderTarget.DrawBitmap(
                ButtonsResources.TITLE01,
                new RawRectangleF(
                    width + 10 * xScala,
                    height + 63 * yScala,
                    width + (10 + dx) * xScala,
                    height + (63 + dy) * yScala
                ),
                1,
                BitmapInterpolationMode.Linear,
                mapRf
            );
        }

        public void select() {
            selected = 1;
        }

        public void click() {
        }

        public string getName() {
            return "GameStart";
        }

        public void unselect() {
            selected = -1;
        }
    }
}