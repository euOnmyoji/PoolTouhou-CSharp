using System;
using System.Drawing;
using System.Windows.Forms;
using OpenGL;
using PoolTouhou.Utils;
using ErrorCode = OpenGL.ErrorCode;
using Exception = System.Exception;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using Keys = System.Windows.Forms.Keys;
using KhronosApi = OpenGL.KhronosApi;

namespace PoolTouhou.Window.Windows {
    public sealed class WindowsWindow : Form, IWindow {
        private readonly IntPtr handle;

        public WindowsWindow() {
            AutoScaleMode = AutoScaleMode.None;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = @"MainForm";
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1600, 900);
            Size = new Size(1600, 900);
            BackColor = Color.FromArgb(255, 0, 0, 0);
            Text = @"PoolTouhou";
            Visible = true;
            handle = Handle;
        }

        public void Draw() {
        }

        public bool Init() {
            Environment.SetEnvironmentVariable("OPENGL_NET_GL_STATIC_INIT", "NO");
            Environment.SetEnvironmentVariable("OPENGL_NET_EGL_STATIC_INIT", "NO");
            PoolTouhou.Logger.Info("Bind Wgl API Now");
            Wgl.BindAPI();
            PoolTouhou.Logger.Info("GetDevice");
            var dc = Wgl.GetDC(handle);
            if (dc == IntPtr.Zero || dc == Handle) {
                throw new Exception("Can not get device!");
            }
            KhronosApi.EglInitializing += (sender, args) => {
                args.Display = handle;
            };
            PoolTouhou.Logger.Info("Init Egl");
            Egl.Initialize();
            PoolTouhou.Logger.Info("Init Gl");
            Gl.Initialize();
            var pixelFormatDescriptor = new Wgl.PIXELFORMATDESCRIPTOR(32);
            Wgl.SetPixelFormat(dc, Wgl.ChoosePixelFormat(dc, ref pixelFormatDescriptor), ref pixelFormatDescriptor);
            GlContext = Wgl.CreateContext(dc);
            if (GlContext == IntPtr.Zero || GlContext == null) {
                throw new Exception("No Gl Context!");
            }
            if (!Wgl.MakeCurrent(dc, GlContext)) {
                var errorCode = Gl.GetError();
                if (errorCode != ErrorCode.NoError) {
                    PoolTouhou.Logger.Info($"Wgl error {errorCode}");
                    throw new Exception(errorCode.ToString());
                }
            }
            Gl.BindAPI(Gl.CurrentVersion, Gl.CurrentExtensions);

            return true;
        }

        public IntPtr GlContext { get; private set; }

        public void RunMessageLoop() {
            Application.Run(this);
        }

        public new void Dispose() {
            PoolTouhou.Logger.Info("开始释放原生窗口资源");
            base.Dispose();
            Wgl.MakeCurrent(IntPtr.Zero, IntPtr.Zero);
            Wgl.DeleteContext(GlContext);
            Application.Exit();
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
            if (e.Shift) {
                InputData.KEY_PRESSED.Add((int) Keys.Shift);
            }
            InputData.KEY_PRESSED.Add(e.KeyValue);
        }

        protected override void OnKeyUp(KeyEventArgs e) {
            base.OnKeyUp(e);
            if (!e.Shift) {
                InputData.KEY_PRESSED.Remove((int) Keys.Shift);
            }
            InputData.KEY_PRESSED.Remove(e.KeyValue);
        }
    }
}