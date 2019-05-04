using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.Direct2D1;

namespace PoolTouhou {
    public static class PoolTouhou {
        public static MainForm mainForm;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        // ReSharper disable once InconsistentNaming
        private static void Main() {
            Console.WriteLine(@"start");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainForm = new MainForm();
            mainForm.init();
            Application.Run(mainForm);
        }
    }
}