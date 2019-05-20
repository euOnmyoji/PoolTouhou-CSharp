using System;
using System.IO;
using System.Reflection;
using System.Threading;
using PoolTouhou.Games.PoolRush;
using PoolTouhou.UI;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using static PoolTouhou.Utils.Util;

namespace PoolTouhou.GameState {
    internal class MenuState : IGameState {
        private readonly IUi[] uis = {new TitleMenuUi(), new GameChooseUi()};
        private int cur;

        public void Draw(RenderTarget target) {
            uis[cur].Draw(target);
        }

        public void Update(ref InputData input) {
            int result = uis[cur].Update(ref input);
            switch (result) {
                case UiEvents.CHOOSE_GAME: cur = 1;
                    break;
                case UiEvents.EXIT: {
                    if (cur == 1) {
                        cur = 0;
                    }
                    break;
                }
                default: {
                    return;
                }
            }
        }

        public string GetStateName() => @"Menu";
    }

    internal class LoadMenuState : IGameState {
        private readonly Bitmap loadingMap = LoadBitMapFromFile(
            "res/ascii/loading.png",
            SharpDX.WIC.PixelFormat.Format32bppPRGBA
        );

        private readonly Bitmap background = LoadBitMapFromFile(
            "res/background/loading.png",
            SharpDX.WIC.PixelFormat.Format32bppPRGBA
        );

        private bool startLoading;
        private readonly DateTime start = DateTime.Now;

        private MenuState menuState;

        public LoadMenuState() {
            new Thread(
                () => {
                    var dirInfo = new DirectoryInfo(".\\games");

                    if (!dirInfo.Exists) {
                        dirInfo.Create();
                    }
                    MainClass.OnLoad();
                    foreach (var fileInfo in dirInfo.GetFiles()) {
                        if (fileInfo.Name.EndsWith(".dll")) {
                            Logger.Info($"loading {fileInfo} ");
                            var asm = Assembly.LoadFile(fileInfo.FullName);
                            var type = asm.GetType($"{fileInfo.Name.Substring(0, fileInfo.Name.Length - 4)}.MainClass");
                        }
                    }
                    menuState = new MenuState();
                }
            ).Start();
        }

        ~LoadMenuState() {
            loadingMap.Dispose();
            background.Dispose();
        }

        public void Draw(RenderTarget target) {
            //loading won't last long?
            float ms = (float) DateTime.Now.Subtract(start).TotalMilliseconds;
            if (ms > 2000) {
                ms -= 2000;
                startLoading = true;
                while (ms > 2000) {
                    ms -= 2000;
                }
            }

            if (ms > 1000) {
                ms = 2000 - ms;
            }
            var size = target.Size;

            float alpha = ms / 2000 + 0.5f;

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

        public void Update(ref InputData input) {
            if (startLoading && menuState != null) {
                MainForm.gameState = menuState;
            }
        }

        public string GetStateName() {
            return @"LoadingMenu";
        }
    }
}