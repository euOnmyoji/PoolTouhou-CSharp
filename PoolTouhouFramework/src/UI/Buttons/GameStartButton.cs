using System;

namespace PoolTouhouFramework.UI.Buttons {
    public class GameStartButton : Button {
        private const float dx = 128;
        private const float dy = 15;

        public GameStartButton() {
            selectTime = DateTime.Now.AddSeconds(-10);
        }

        public override void Draw(double deltaTime) {
//            var size = renderTarget.Size;
//            float xScala = size.Width / 640;
//            float yScala = size.Height / 480;
//            float width = size.Width / 2;
//            float height = size.Height / 2;
//            ref var mapRf = ref unselectedRf;
//            int xOffset = 0;
//            int yOffset = 0;
//            if (selectTime != null) {
//                mapRf = ref selectedRf;
//                GetOffset(DateTime.Now.Subtract((DateTime) selectTime).TotalMilliseconds, out xOffset, out yOffset);
//            }
//
//            renderTarget.DrawBitmap(
//                ButtonsResources.INSTANCE.title01,
//                new RawRectangleF(
//                    width + 10 * xScala + xOffset,
//                    height + 63 * yScala + yOffset,
//                    width + (10 + dx) * xScala + xOffset,
//                    height + (63 + dy) * yScala + yOffset
//                ),
//                1,
//                BitmapInterpolationMode.Linear,
//                mapRf
//            );
        }

        public override int Click() {
            return UiEvents.CHOOSE_GAME;
        }

        public override string GetName() {
            return "GameStart";
        }
    }
}