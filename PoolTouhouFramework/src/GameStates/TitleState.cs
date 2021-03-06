﻿using System;
using System.IO;
using System.Reflection;
using System.Threading;
using PoolTouhouFramework.Games;
using PoolTouhouFramework.UI;
using PoolTouhouFramework.Utils;

namespace PoolTouhouFramework.GameStates {
    internal class MenuState : GameState {
        private readonly IUi[] uis = {new TitleMenuUi(), new GameChooseUi()};
        private sbyte cur;

        public MenuState() : base(null) {
            PoolTouhou.Logger.Log("实例化主菜单状态");
//            PoolTouhou.SoundManager.TryLoad(
//                "title",
//                @"res\bgm\上海アリス幻樂団 - 桜舞い散る天空.mp3",
//                GetSoundStreamMethods.GetMp3SoundStream
//            );
        }

        public override void Draw(double deltaTime) {
            uis[cur].Draw(deltaTime);
        }

        public override void Update(ref InputData input) {
//            PoolTouhou.SoundManager.Loop("title");
            int result = uis[cur].Update(ref input);
            switch (result) {
                case UiEvents.CHOOSE_GAME:
                    cur = 1;
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

        public override string GetStateName() => @"Menu";

        public IGame GamingGame() {
            return null;
        }

        public override void Dispose() {
        }
    }

    internal class LoadingMenuState : GameState {
        private MenuState menuState;
        private readonly DateTime start = DateTime.Now;

        public LoadingMenuState() : base(null) {
            new Thread(
                () => {
                    try {
                        var dirInfo = new DirectoryInfo("./games");

                        if (!dirInfo.Exists) {
                            dirInfo.Create();
                        }
//                        MainClass.OnLoad();
                        foreach (var fileInfo in dirInfo.GetFiles()) {
                            if (fileInfo.Name.EndsWith(".dll")) {
                                PoolTouhou.Logger.Log($"loading {fileInfo} ");
                                var asm = Assembly.LoadFile(fileInfo.FullName);
                                var type = asm.GetType(
                                    $"{fileInfo.Name.Substring(0, fileInfo.Name.Length - 4)}.MainClass"
                                );
                            }
                        }
                        menuState = new MenuState();
                    } catch (Exception e) {
                        PoolTouhou.Logger.Log(e.Message + Environment.NewLine + e.StackTrace);
                        throw;
                    }
                }
            ).Start();
        }

        public override void Draw(double deltaTime) {
//            //loading won't last long?
//            float ms = (float) DateTime.Now.Subtract(start).TotalMilliseconds;
//            if (ms > 2000) {
//                ms -= 2000;
//                startLoading = true;
//                while (ms > 2000) {
//                    ms -= 2000;
//                }
//            }
//
//            if (ms > 1000) {
//                ms = 2000 - ms;
//            }
//            var size = target.Size;
//
//            float alpha = ms / 2000 + 0.5f;
//
//            float scale = size.Width / 640.0f;
//            float startX = size.Width * 0.7f;
//            float startY = size.Height * 0.8f;
//            var girlR = new RawRectangleF(startX, startY, startX + scale * 128, startY + scale * 30);
//            target.DrawBitmap(
//                background,
//                new RawRectangleF(0f, 0f, size.Width, size.Height),
//                1.0f,
//                BitmapInterpolationMode.Linear
//            );
//            target.DrawBitmap(
//                loadingMap,
//                girlR,
//                alpha,
//                BitmapInterpolationMode.Linear,
//                new RawRectangleF(0, 0, 128, 30)
//            );
//            float offsetX = 0.25f * scale * 128;
//            float offsetY = scale * 25;
//            target.DrawBitmap(
//                loadingMap,
//                new RawRectangleF(
//                    girlR.Left + offsetX,
//                    girlR.Top + offsetY,
//                    girlR.Right + offsetX,
//                    girlR.Bottom + offsetY
//                ),
//                alpha,
//                BitmapInterpolationMode.Linear,
//                new RawRectangleF(0, 30, 128, 60)
//            );
        }

        public override void Update(ref InputData input) {
            if (menuState != null) {
                PoolTouhou.GameState = menuState;
                menuState = null;
            }
        }

        public override string GetStateName() {
            return @"LoadingMenu";
        }


        public override void Dispose() {
        }
    }
}