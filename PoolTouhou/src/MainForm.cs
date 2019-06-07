﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using PoolTouhou.GameStates;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using static PoolTouhou.PoolTouhou;
using Brush = SharpDX.Direct2D1.Brush;

namespace PoolTouhou {
    public sealed class MainForm : Form {
        public const float FONT_SIZE = 15;
        public Brush brush;
        private SharpDX.DirectWrite.Factory textFactory;
        public TextFormat textFormat;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                running = false;
                textFactory?.Dispose();
                textFormat?.Dispose();
                brush?.Dispose();
            }

            base.Dispose(disposing);
        }

        private static double tps;

        private static void UpdateLoop() {
            try {
                ushort tickCount = 0;
                long last = Watch.ElapsedTicks;
                long lastCount = Watch.ElapsedTicks;
                double nextFrameCount = lastCount + OneTickCount;
                while (running) {
                    ushort goalTps = Tps;
                    var input = new InputData();
                    GameState.Update(ref input);

                    if (++tickCount >= goalTps) {
                        tickCount = 0;
                        long now = Watch.ElapsedTicks;
                        tps = goalTps * Stopwatch.Frequency / (double) (now - last);
                        last = now;
                    }
                    input.Step();
                    lastCount = Watch.ElapsedTicks;
                    if (lastCount < nextFrameCount) {
                        Thread.Sleep((int) ((nextFrameCount - lastCount) * 1000 / Stopwatch.Frequency));
                    }
                    while (Watch.ElapsedTicks < nextFrameCount) {
                        //waiting for next tick
                    }

                    if ((nextFrameCount += OneTickCount) < lastCount) {
                        nextFrameCount = lastCount + OneTickCount;
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
                long lastCount = 0;
                long last = Watch.ElapsedTicks;
                var query = new Query(
                    DxResource.d3d11Device,
                    new QueryDescription {
                        Flags = QueryFlags.None, Type = QueryType.Event
                    }
                );
                while (running && !DxResource.RenderTarget.IsDisposed) {
                    var renderTarget = DxResource.RenderTarget;
                    renderTarget.BeginDraw();
                    GameState.Draw(renderTarget);
                    long now = Watch.ElapsedTicks;
                    long dur = now - last;
                    if (dur >= Stopwatch.Frequency) {
                        long paintCount = DxResource.swapChain.LastPresentCount;
                        fps = (paintCount - lastCount) * Stopwatch.Frequency / (double) dur;
                        last = now;
                        lastCount = paintCount;
                    }
                    if (tps > 0 && fps > 0) {
                        var size = renderTarget.Size;
                        string tpsStr = $"{tps:F1}tps";
                        string fpsStr = $"{fps:F1}fps";
                        const float textHeight = FONT_SIZE * 1.25f;
                        float lnLocationY = size.Height - textHeight;
                        renderTarget.DrawText(
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
                        renderTarget.DrawText(
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
                    renderTarget.EndDraw();
                    DxResource.swapChain.Present(0, PresentFlags.None);
                }
            } catch (Exception e) {
                Logger.Info(e.Message + Environment.NewLine + e.StackTrace);
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace, @"很抱歉出错了！");
                Application.Exit();
            }
        }

        public void Init() {
            brush = new SolidColorBrush(DxResource.RenderTarget, new RawColor4(100, 100, 100, 1));
            textFactory = new SharpDX.DirectWrite.Factory();
            textFormat = new TextFormat(textFactory, Font.FontFamily.Name, FONT_SIZE);
            new Thread(
                () => {
                    GameState = new LoadingMenuState();
                    new Thread(DrawLoop).Start();
                    UpdateLoop();
                }
            ).Start();
        }

        public MainForm() {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = @"MainForm";
            ResumeLayout(false);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1600, 900);
            Size = new Size(1600, 900);
            BackColor = Color.FromArgb(255, 0, 0, 0);
            Text = @"PoolTouhou";
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
    }
}