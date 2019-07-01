using System;
using System.Windows.Forms;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace PoolTouhou.UI.Buttons {
    public class ExitButton : Button {
        private const float dx = 64;
        private const float dy = 16;
        private static RawRectangleF selectedRf = new RawRectangleF(0, 140, dx, 140 + dy);
        private static RawRectangleF unselectedRf = new RawRectangleF(128, 140, 128 + dx, 140 + dy);

        public override void Draw(DeviceContext renderTarget) {
            var size = renderTarget.Size;
            float xScala = size.Width / 640;
            float yScala = size.Height / 480;
            float width = size.Width / 2;
            float height = size.Height / 2;
            ref var mapRf = ref unselectedRf;
            int xOffset = 0;
            int yOffset = 0;
            if (selectTime != null) {
                mapRf = ref selectedRf;
                GetOffset(DateTime.Now.Subtract((DateTime) selectTime).TotalMilliseconds, out xOffset, out yOffset);
            }

            renderTarget.DrawBitmap(
                ButtonsResources.INSTANCE.title01,
                new RawRectangleF(
                    width + 10 * xScala + xOffset,
                    height + 80 * yScala + yOffset,
                    width + (10 + dx) * xScala + xOffset,
                    height + (80 + dy) * yScala + yOffset
                ),
                1,
                BitmapInterpolationMode.Linear,
                mapRf
            );
        }

        public override int Click() {
            Application.Exit();
            return UiEvents.EXIT;
        }

        public override string GetName() => "TitleExit";
    }
}