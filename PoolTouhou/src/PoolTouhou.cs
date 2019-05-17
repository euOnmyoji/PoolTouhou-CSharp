using System;
using System.Diagnostics;
using System.Windows.Forms;
using PoolTouhou.Manager;


namespace PoolTouhou {
    public static class PoolTouhou {
        public static MainForm MainForm { get; private set; }

        public static Stopwatch Watch { get; } = new Stopwatch();


        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        public static int Main(string[] args) {
            Watch.Start();
            try {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                MainForm = new MainForm();
                MainForm.Init();
            } catch (Exception e) {
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace, @"很抱歉出错了！");
                MainForm.Dispose();
                Application.Exit();
                return 1;
            }

            Application.Run(MainForm);
            MainForm.Dispose();
            return 0;
        }
    }
}