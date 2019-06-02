using System;
using System.Diagnostics;
using System.Windows.Forms;
using PoolTouhou.Sound;
using PoolTouhou.Utils;

namespace PoolTouhou {
    public static class PoolTouhou {
        public static MainForm MainForm { get; private set; }

        public static ushort Tps {
            get => tps;
            set {
                tps = value;
                OneTickCount = (double) Stopwatch.Frequency / value;
            }
        }


        public static Stopwatch Watch { get; } = new Stopwatch();
        public static volatile bool running = true;
        public static SoundManager SoundManager { get; private set; }


        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        public static int Main(string[] args) {
            Watch.Start();
            try {
                Logger.Info("开始实例化游戏窗口类");
                MainForm = new MainForm();
                Logger.Info("开始初始化游戏");
                Init();
            } catch (Exception e) {
                Logger.Info(e.Message + Environment.NewLine + e.StackTrace);
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace, @"很抱歉出错了！");
                Dispose();
                Application.Exit();
                return 1;
            }
            GC.Collect();
            Application.Run(MainForm);
            Dispose();
            Logger.Info("回收资源 退出主线程");
            return 0;
        }

        private static ushort tps = 100;
        public static double OneTickCount { get; private set; } = (double) Stopwatch.Frequency / tps;

        private static void Init() {
            SoundManager = new SoundManager();
            MainForm.Init();
        }

        private static void Dispose() {
            MainForm?.Dispose();
            SoundManager?.Dispose();
        }
    }
}