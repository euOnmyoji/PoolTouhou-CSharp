using System;
using System.Diagnostics;
using PoolTouhouFramework.GameStates;
using PoolTouhouFramework.UI.Buttons;
using PoolTouhouFramework.Utils;

namespace PoolTouhouFramework {
    public static class PoolTouhou {
        public static GameWindow GameWindow { get; private set; }
        public static Logger Logger { get; private set; }

        public static GameState GameState {
            get => gameState;
            set {
                var old = gameState;
                gameState = value;
                old?.Dispose();
            }
        }


        public static ushort Tps {
            get => tps;
            set {
                tps = value;
                OneTickCount = (double) Stopwatch.Frequency / value;
            }
        }


        public static Stopwatch Watch { get; } = new Stopwatch();
        public static volatile bool running = true;

        public static int Main(string[] args) {
            Watch.Start();
            try {
                Logger = new Logger();
                Init();
            } catch (Exception e) {
                Logger.Log("初始化时发生异常");
                Logger.Log(e.Message + Environment.NewLine + e.StackTrace);
                Dispose();
                Logger.StopLog();
                return -1;
            }
            GC.Collect();
            try {
                Logger.Log("游戏初始化完毕");
                GameWindow.RunMessageLoop();
                Logger.Log("开始回收资源");
            } catch (Exception e) {
                Logger.Log(e.Message + Environment.NewLine + e.StackTrace);
            }
            Dispose();
            GC.Collect();
            Logger.StopLog();
            Logger.Log("退出主线程");
            return 0;
        }

        private static ushort tps = 100;
        public static double OneTickCount { get; private set; } = (double) Stopwatch.Frequency / tps;

        private static void Init() {
            Logger.Log("[TODO:something about steam...");
            //todo: link steam in the future.
            Logger.Log("开始实例化窗口类");
            GameWindow = new GameWindow();
            Logger.Log("初始化图形资源");
            Logger.Log("初始化音频资源");
            Logger.Log("初始化窗口类 & 游戏");
            GameWindow.Init();
        }

        private static void Dispose() {
            GameWindow?.Dispose();
            gameState?.Dispose();
            ButtonsResources.INSTANCE?.Dispose();
        }

        private static GameState gameState;
    }
}