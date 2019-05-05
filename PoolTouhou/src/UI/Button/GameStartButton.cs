using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace PoolTouhou.UI.Button {
    public class GameStartButton : Button {
        private const float dx = 128;
        private const float dy = 15;
        private static RawRectangleF selectedRf = new RawRectangleF(0, 0, dx, dy);
        private static RawRectangleF unselectedRf = new RawRectangleF(128, 0, 128 + dx, dy);
        private int selected;

        public GameStartButton(int selected = 9999) {
            this.selected = selected;
        }

        public override void draw(RenderTarget renderTarget) {
            var size = renderTarget.Size;
            float xScala = size.Width / 640;
            float yScala = size.Height / 480;
            float width = size.Width / 2;
            float height = size.Height / 2;
            ref var mapRf = ref unselectedRf;
            if (selected > 0) {
                if (++selected == 1) {
                    selected = 0;
                } else {
                    mapRf = ref selectedRf;
                }
            }

            getOffset(selected, out int xOffset, out int yOffset);

            renderTarget.DrawBitmap(
                ButtonsResources.TITLE01,
                new RawRectangleF(
                    width + 10 * xScala + xOffset,
                    height + 63 * yScala + yOffset,
                    width + (10 + dx) * xScala + xOffset,
                    height + (63 + dy) * yScala + yOffset
                ),
                1,
                BitmapInterpolationMode.Linear,
                mapRf
            );
        }

        public override void select() {
            selected = 1;
        }

        public override void click() {
        }

        public override string getName() {
            return "GameStart";
        }

        public override void unselect() {
            selected = 0;
        }
    }
}