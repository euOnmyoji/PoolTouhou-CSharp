using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using PoolTouhou.GameState;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;

namespace PoolTouhou {
    public class MainForm : Form {
        public static IGameState gameState;
        private volatile bool running = true;
        private double fps { get; set; } = 60f;
        private int paintCount { get; set; } = 0;
        private DateTime last { get; set; } = DateTime.Now;

        private Factory d2dFactory { get; set; }
        public RenderTarget renderTarget { get; private set; }

        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private IContainer components;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                components?.Dispose();
                d2dFactory.Dispose();
                renderTarget.Dispose();
                running = false;
            }

            base.Dispose(disposing);
        }

        private const double constFps = 60.0;
        private const double timeInPerFrame = 1000.0 / constFps;

        private void loop() {
            try {
                while (running) {
                    var start = DateTime.Now;
                    draw();
                    var end = DateTime.Now;
                    double phase = end.Subtract(start).Duration().TotalMilliseconds;
                    if (phase < timeInPerFrame) {
                        Thread.Sleep(TimeSpan.FromMilliseconds(timeInPerFrame - phase));
                    }
                }
            }
            catch (Exception e) {
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace, @"很抱歉出错了！");
                Application.Exit();
            }
        }

        private void draw() {
            renderTarget.BeginDraw();
            var input = new InputData();
            gameState.update(ref input);
            gameState.draw(renderTarget);
            renderTarget.EndDraw();
            input.step();
        }


        public void init() {
            initializeComponent();
            components = new Container();
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 960);
            Size = new Size(1280, 960);
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            BackColor = Color.Black;
            Text = @"PoolTouhou";

            d2dFactory = new Factory();
            var p = new PixelFormat();
            var h = new HwndRenderTargetProperties {
                Hwnd = Handle, PixelSize = new SharpDX.Size2(Width, Height), PresentOptions = PresentOptions.None
            };

            var r = new RenderTargetProperties(RenderTargetType.Hardware, p, 0, 0, RenderTargetUsage.None,
                FeatureLevel.Level_DEFAULT);
            renderTarget = new WindowRenderTarget(d2dFactory, r, h);
            new Thread(() => {
                gameState = new LoadMenuState();
                loop();
            }).Start();
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
            InputData.KEY_PRESSED.Add(e.KeyValue);
        }

        protected override void OnKeyUp(KeyEventArgs e) {
            base.OnKeyUp(e);
            InputData.KEY_PRESSED.Remove(e.KeyValue);


        }

        private void initializeComponent() {
            SuspendLayout();
            // 
            // MainForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1280, 960);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = @"MainForm";
            ResumeLayout(false);
        }
    }
}