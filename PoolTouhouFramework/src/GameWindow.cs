using System;
using System.Diagnostics;
using System.Threading;
using PoolTouhouFramework.GameStates;
using PoolTouhouFramework.Utils;
using Veldrid;
using Veldrid.OpenGLBinding;
using Veldrid.Sdl2;
using static PoolTouhouFramework.PoolTouhou;

namespace PoolTouhouFramework {
    public sealed class GameWindow : IDisposable {
        private readonly Sdl2Window window;
        private readonly GraphicsDevice device;

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
                while (window.Exists && running) {
                    OpenGLNative.glClear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                }
            } catch (Exception e) {
                running = false;
                PoolTouhou.Logger.Info(e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        public void Init() {
            new Thread(
                () => {
                    PoolTouhou.GameState = new LoadingMenuState();
                    new Thread(DrawLoop).Start();
                    UpdateLoop();
                }
            ).Start();
        }

        public GameWindow() {
            window = new Sdl2Window(
                "PoolTouhou",
                20,
                20,
                1600,
                900,
                SDL_WindowFlags.OpenGL | SDL_WindowFlags.AllowHighDpi,
                false
            );
            device = OpenGlDeviceUtil.CreateDefaultOpenGlGraphicsDevice(window);
        }


        public void Dispose() {
            PoolTouhou.Logger.Info("开始释放窗口的托管资源");
            running = false;
        }

        public void RunMessageLoop() {
            while (window.Exists && running) {
                var input = window.PumpEvents();

            }
        }
    }
}