using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using PoolTouhou.GameState;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using Brush = SharpDX.Direct2D1.Brush;
using Factory = SharpDX.Direct2D1.Factory;

namespace PoolTouhou {
    public class MainForm : Form {
        public const float FONT_SIZE = 15;
        public Brush brush;
        private SharpDX.DirectWrite.Factory textFactory;
        public TextFormat textFormat;
        public static IGameState gameState;

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
                PoolTouhou.running = false;
                components?.Dispose();
                D2dFactory?.Dispose();
                RenderTarget?.Dispose();
                textFactory?.Dispose();
                textFormat?.Dispose();
            }

            base.Dispose(disposing);
        }

        private static double tps;

        private static void UpdateLoop() {
            try {
                ushort tickCount = 0;
                long last = PoolTouhou.Watch.ElapsedTicks;
                long lastCount = PoolTouhou.Watch.ElapsedTicks;
                double nextFrameCount = lastCount + PoolTouhou.OneTickCount;
                while (PoolTouhou.running) {
                    ushort goalTps = PoolTouhou.Tps;
                    var input = new InputData();
                    gameState.Update(ref input);

                    if (++tickCount >= goalTps) {
                        tickCount = 0;
                        long now = PoolTouhou.Watch.ElapsedTicks;
                        tps = goalTps * Stopwatch.Frequency / (double) (now - last);
                        last = now;
                    }
                    input.Step();
                    lastCount = PoolTouhou.Watch.ElapsedTicks;
                    if (lastCount < nextFrameCount) {
                        Thread.Sleep((int) ((nextFrameCount - lastCount) * 1000 / Stopwatch.Frequency));
                    }
                    while (PoolTouhou.Watch.ElapsedTicks < nextFrameCount) {
                        //waiting for next tick
                    }

                    if ((nextFrameCount += PoolTouhou.OneTickCount) < lastCount) {
                        nextFrameCount = lastCount + PoolTouhou.OneTickCount;
                    }
                }
            } catch (Exception e) {
                Logger.Info(e.Message + Environment.NewLine + e.StackTrace);
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace, @"很抱歉出错了！");
                Application.Exit();
            }
        }

        private void DrawLoop() {
            try {
                double fps = 0;
                const int updateFpsPaintCount = 60;
                double k = updateFpsPaintCount * Stopwatch.Frequency;
                int paintCount = 0;
                long last = PoolTouhou.Watch.ElapsedTicks;
                while (PoolTouhou.running && !RenderTarget.IsDisposed) {
                    RenderTarget.BeginDraw();
                    gameState.Draw(RenderTarget);
                    if (++paintCount == updateFpsPaintCount) {
                        paintCount = 0;
                        long now = PoolTouhou.Watch.ElapsedTicks;
                        fps = k / (now - last);
                        last = now;
                    }
                    if (tps > 0 && fps > 0) {
                        var size = RenderTarget.Size;
                        string tpsStr = $"{tps:F1}tps";
                        string fpsStr = $"{fps:F1}fps";
                        const float textHeight = FONT_SIZE * 1.25f;
                        float lnLocationY = size.Height - textHeight;
                        RenderTarget.DrawText(
                            fpsStr,
                            textFormat,
                            new RawRectangleF(
                                size.Width - FONT_SIZE * fpsStr.Length / 2,
                                lnLocationY,
                                size.Width,
                                size.Height
                            ),
                            brush,
                            DrawTextOptions.None,
                            MeasuringMode.Natural
                        );
                        RenderTarget.DrawText(
                            tpsStr,
                            textFormat,
                            new RawRectangleF(
                                size.Width - FONT_SIZE * tpsStr.Length / 2,
                                size.Height - textHeight * 1.75f,
                                size.Width,
                                lnLocationY
                            ),
                            brush,
                            DrawTextOptions.None,
                            MeasuringMode.Natural
                        );
                    }
                    RenderTarget.EndDraw();
                }
            } catch (Exception e) {
                Logger.Info(e.Message + Environment.NewLine + e.StackTrace);
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
            brush = new SolidColorBrush(RenderTarget, new RawColor4(100, 100, 100, 1));
            textFactory = new SharpDX.DirectWrite.Factory();
            textFormat = new TextFormat(textFactory, Font.FontFamily.Name, FONT_SIZE);
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
            PoolTouhou.running = false;
            base.OnClosing(e);
        }

        private void InitializeComponent() {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1280, 960);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = @"MainForm";
            ResumeLayout(false);
        }
    }
}