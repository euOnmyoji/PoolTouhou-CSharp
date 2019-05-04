using System;
using System.Threading;
using System.Windows.Forms;
using PoolTouhou.UI.Button;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using static PoolTouhou.Utils.Util;

namespace PoolTouhou.GameState {
    internal class MenuState : IGameState {
        private readonly IButton[] buttons = {new GameStartButton(), new TitleExitButton()};
        private int curSelect;

        public MenuState() {
            buttons[curSelect].select();
        }

        public void draw(RenderTarget target) {
            target.Clear(new RawColor4(0, 0, 0, 0));
            foreach (var button in buttons) {
                button.draw(target);
            }
        }

        public void update(ref InputData input) {
            if (input.shoot == 1) {
                buttons[curSelect].click();
            } else if (input.spell == 1) {
                if (curSelect != 1) {
                    buttons[curSelect].unselect();
                    curSelect = 1;
                    buttons[curSelect].select();
                }
            } else if (input.down < 1 || input.up < 1) {
                //切换选中按钮
                const int cd = 10;
                const int firstCd = 30;
                if (input.down > firstCd && input.down % cd == 1 || input.down == 1) {
                    buttons[curSelect++].unselect();
                    if (curSelect >= buttons.Length) { curSelect = 0; }

                    buttons[curSelect].select();
                } else if (input.up > firstCd && input.up % cd == 1 || input.up == 1) {
                    buttons[curSelect--].unselect();
                    if (curSelect < 0) { curSelect = buttons.Length - 1; }

                    buttons[curSelect].select();
                }
            }
        }

        public string getStateName() {
            return @"Menu";
        }
    }

    internal class LoadMenuState : IGameState {
        private readonly Bitmap loadingMap = loadBitMapFromFile(
            "res/ascii/loading.png",
            SharpDX.WIC.PixelFormat.Format32bppPRGBA
        );

        private readonly Bitmap background = loadBitMapFromFile(
            "res/background/loading.jpg",
            SharpDX.WIC.PixelFormat.Format32bppPRGBA
        );

        private bool startLoading;
        private short tick { get; set; }
        private short add { get; set; } = 1;

        ~LoadMenuState() {
            loadingMap.Dispose();
            background.Dispose();
        }

        public void draw(RenderTarget target) {
            tick += add;
            if (tick == 60) {
                add = -1;
            } else if (tick == 0) {
                if (!startLoading) {
                    startLoading = true;
                    new Thread(() => MainForm.gameState = new MenuState()).Start();
                }

                add = 1;
            }

            float alpha = tick / 120.0f + 0.5f;

            var size = target.Size;
            float scale = size.Width / 640.0f;
            float startX = size.Width * 0.7f;
            float startY = size.Height * 0.8f;
            var girlR = new RawRectangleF(startX, startY, startX + scale * 128, startY + scale * 30);
            target.DrawBitmap(
                background,
                new RawRectangleF(0f, 0f, size.Width, size.Height),
                1.0f,
                BitmapInterpolationMode.Linear
            );
            target.DrawBitmap(
                loadingMap,
                girlR,
                alpha,
                BitmapInterpolationMode.Linear,
                new RawRectangleF(0, 0, 128, 30)
            );

            float offsetX = 0.25f * scale * 128;
            float offsetY = scale * 25;
            target.DrawBitmap(
                loadingMap,
                new RawRectangleF(
                    girlR.Left + offsetX,
                    girlR.Top + offsetY,
                    girlR.Right + offsetX,
                    girlR.Bottom + offsetY
                ),
                alpha,
                BitmapInterpolationMode.Linear,
                new RawRectangleF(0, 30, 128, 60)
            );
        }

        public void update(ref InputData input) {
        }

        public string getStateName() {
            return @"LoadingMenu";
        }
    }
}