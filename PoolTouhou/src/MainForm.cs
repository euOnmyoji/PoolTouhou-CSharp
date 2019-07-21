using System;
using System.Diagnostics;
using System.Threading;
using OpenGL;
using PoolTouhou.GameStates;
using PoolTouhou.Utils;
using PoolTouhou.Window;
using PoolTouhou.Window.Windows;
using static PoolTouhou.PoolTouhou;

namespace PoolTouhou {
    public sealed class MainForm : IDisposable {
        public uint Id { get; private set; }
        public const float FONT_SIZE = 15;
        public IWindow Impl { get; private set; }


        private static double tps;

        private static void UpdateLoop() {
            try {
                ushort tickCount = 0;
                long last = Watch.ElapsedTicks;
                long lastCount = Watch.ElapsedTicks;
                double nextFrameCount = lastCount + OneTickCount;
                while (running) {
                    ushort goalTps = Tps;
                    var input = InputData.GetData();
                    PoolTouhou.GameState.Update(ref input);

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
                running = false;
                PoolTouhou.Logger.Info(e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        private void DrawLoop() {
            try {
                double fps = 0;
                long lastCount = 0;
                long last = Watch.ElapsedTicks;

                while (running) {
                    Gl.ClearColor(0, 1, 1, 1);
//                    Gl.VertexAttrib2f(3, (-1920, -1080));
                    Gl.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
                    Gl.Flush();
                    Gl.CheckErrors();
                }


//                while (running && !DxResource.RenderTarget.IsDisposed) {
//                    var renderTarget = DxResource.RenderTarget;
//                    renderTarget.BeginDraw();
//                    PoolTouhou.GameState.Draw(renderTarget);
//                    renderTarget = DxResource.RenderTarget;
//                    long now = Watch.ElapsedTicks;
//                    long dur = now - last;
//                    if (dur >= Stopwatch.Frequency) {
//                        long paintCount = DxResource.swapChain.LastPresentCount;
//                        fps = (paintCount - lastCount) * Stopwatch.Frequency / (double) dur;
//                        last = now;
//                        lastCount = paintCount;
//                    }
//                    if (tps > 0 && fps > 0) {
//                        var size = renderTarget.Size;
//                        string tpsStr = $"{tps:F1}tps";
//                        string fpsStr = $"{fps:F1}fps";
//                        const float textHeight = FONT_SIZE * 1.25f;
//                        float lnLocationY = size.Height - textHeight;
//                        renderTarget.Transform = new RawMatrix3x2(1, 0, 0, 1, 0, 0);
//                        renderTarget.DrawText(
//                            fpsStr,
//                            textFormat,
//                            new RawRectangleF(
//                                size.Width - FONT_SIZE * fpsStr.Length / 2,
//                                lnLocationY,
//                                size.Width,
//                                size.Height
//                            ),
//                            brush,
//                            DrawTextOptions.None,
//                            MeasuringMode.Natural
//                        );
//                        renderTarget.DrawText(
//                            tpsStr,
//                            textFormat,
//                            new RawRectangleF(
//                                size.Width - FONT_SIZE * tpsStr.Length / 2,
//                                size.Height - textHeight * 1.75f,
//                                size.Width,
//                                lnLocationY
//                            ),
//                            brush,
//                            DrawTextOptions.None,
//                            MeasuringMode.Natural
//                        );
//                    }
//                    DxResource.RenderTarget.TryEndDraw(out long tag1, out long tag2);
//                    DxResource.swapChain.Present(1, PresentFlags.None);
//                }
            } catch (Exception e) {
                running = false;
                PoolTouhou.Logger.Info(e.Message + Environment.NewLine + e.StackTrace);
                Impl.Dispose();
            }
        }

        public void Init() {
            if (!Impl.Init()) {
                Gl.CheckErrors();
                throw new Exception("can't init window!");
            }
            Id = Gl.CreateProgram();
            Gl.CheckErrors();
            new Thread(
                () => {
                    PoolTouhou.GameState = new LoadingMenuState();
                    new Thread(DrawLoop).Start();
                    UpdateLoop();
                }
            ).Start();
        }

        public MainForm() {
            switch (Environment.OSVersion.Platform) {
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.Win32NT:
                case PlatformID.WinCE: {
                    Impl = new WindowsWindow();
                    break;
                }
                case PlatformID.Unix: {
                    throw new NotImplementedException(Environment.OSVersion.Platform + " is not supported!");
                }
                case PlatformID.Xbox: {
                    throw new NotImplementedException(Environment.OSVersion.Platform + " is not supported!");
                }
                case PlatformID.MacOSX: {
                    throw new NotImplementedException(Environment.OSVersion.Platform + " is not supported!");
                }
                default: {
                    throw new NotImplementedException(Environment.OSVersion.Platform + " is not supported!");
                }
            }
        }

        public void Dispose() {
            PoolTouhou.Logger.Info("开始释放窗口的托管资源");
            running = false;
            Impl?.Dispose();
        }
    }
}