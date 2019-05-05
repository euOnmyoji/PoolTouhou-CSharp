using System;
using System.Reflection;
using System.Windows.Forms;

namespace PoolTouhou {
    public static class PoolTouhou {
        public static MainForm mainForm;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        // ReSharper disable all (It's Main!)
        public static int Main(string[] args) {
            try {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                mainForm = new MainForm();
                mainForm.init();
            } catch (Exception e) {
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace, @"很抱歉出错了！");
                Application.Exit();
                return 1;
            }

            Application.Run(mainForm);
            return 0;
        }
    }
}