using System;
using System.Diagnostics;
using System.Windows.Forms;
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


        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        public static int Main(string[] args) {
            Watch.Start();
            try {
                Logger.Info("开始实例化游戏窗口类");
                MainForm = new MainForm();
                Logger.Info("初始化游戏");
                MainForm.Init();
            } catch (Exception e) {
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace, @"很抱歉出错了！");
                MainForm.Dispose();
                Application.Exit();
                return 1;
            }

            Application.Run(MainForm);
            MainForm.Dispose();
            Logger.Info("回收资源 关闭游戏完成");
            return 0;
        }

        private static ushort tps = 100;
        public static double OneTickCount { get; private set; } = (double) Stopwatch.Frequency / tps;
    }
}