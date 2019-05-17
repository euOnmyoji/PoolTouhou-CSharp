using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PoolTouhou.GameState;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;
using Timer = System.Threading.Timer;

namespace PoolTouhou {
    public class MainForm : Form {
        public static IGameState gameState;
        private volatile bool running = true;
        private double Fps { get; set; } = 60f;
        private int PaintCount { get; set; } = 0;
        private DateTime Last { get; set; } = DateTime.Now;

        private Factory D2dFactory { get; set; }
        public RenderTarget RenderTarget { get; private set; }

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
                running = false;
                components?.Dispose();
                D2dFactory.Dispose();
                RenderTarget.Dispose();
            }

            base.Dispose(disposing);
        }

        private const double constFps = 60.0;
        private const double timeInPerFrame = 1.0 / constFps;

        private void UpdateLoop() {
            try {
                long frequency = Stopwatch.Frequency;
                if (!Stopwatch.IsHighResolution) {
                    frequency = 1;
                }
                double oneFrameCount = timeInPerFrame * frequency;
                long lastCount = PoolTouhou.Watch.ElapsedTicks;
                double nextFrameCount = lastCount + oneFrameCount;
                while (running) {
                    var input = new InputData();
                    gameState.Update(ref input);
                    input.Step();
                    lastCount = PoolTouhou.Watch.ElapsedTicks;
                    if (lastCount < nextFrameCount) {
                        Thread.Sleep((int) ((nextFrameCount - lastCount) / frequency));
                    }
                    while (PoolTouhou.Watch.ElapsedTicks < nextFrameCount) {
                        //waiting for next tick
                    }
                    nextFrameCount = lastCount + oneFrameCount;
                }
            } catch (Exception e) {
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace, @"很抱歉出错了！");
                Application.Exit();
            }
        }

        private void DrawLoop() {
            try {
                while (running && !RenderTarget.IsDisposed) {
                    RenderTarget.BeginDraw();
                    gameState.Draw(RenderTarget);
                    RenderTarget.EndDraw();
                }
            } catch (Exception e) {
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace, @"很抱歉出错了！");
                Application.Exit();
            }
        }

        public void Init() {
            InitializeComponent();
            components = new Container();
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1600, 900);
            Size = new Size(1600, 900);
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            BackColor = Color.Black;
            Text = @"PoolTouhou";

            D2dFactory = new Factory();
            var p = new PixelFormat();
            var h = new HwndRenderTargetProperties {
                Hwnd = Handle, PixelSize = new SharpDX.Size2(Width, Height), PresentOptions = PresentOptions.None
            };

            var r = new RenderTargetProperties(
                RenderTargetType.Hardware,
                p,
                0,
                0,
                RenderTargetUsage.None,
                FeatureLevel.Level_DEFAULT
            );
            RenderTarget = new WindowRenderTarget(D2dFactory, r, h);
            new Thread(
                () => {
                    gameState = new LoadMenuState();
                    new Thread(DrawLoop).Start();
                    UpdateLoop();
                }
            ).Start();
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
            InputData.KEY_PRESSED.Add(e.KeyValue);
        }

        protected override void OnKeyUp(KeyEventArgs e) {
            base.OnKeyUp(e);
            InputData.KEY_PRESSED.Remove(e.KeyValue);
        }

        protected override void OnClosing(CancelEventArgs e) {
            running = false;
            base.OnClosing(e);
        }

        private void InitializeComponent() {
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