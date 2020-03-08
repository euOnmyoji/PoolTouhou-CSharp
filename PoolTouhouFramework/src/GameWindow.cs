using System;
using System.Diagnostics;
using System.Threading;
using ImGuiNET;
using PoolTouhouFramework.GameStates;
using PoolTouhouFramework.Utils;
using Veldrid;
using Veldrid.OpenGL;
using Veldrid.OpenGLBinding;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using static PoolTouhouFramework.PoolTouhou;

namespace PoolTouhouFramework {
    public sealed class GameWindow : IDisposable {
        private readonly Sdl2Window window;
        private GraphicsDevice device;

        private static double tps;

        private static void UpdateLoop() {
            PoolTouhou.Logger.Log("开始逻辑线程");
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
                PoolTouhou.Logger.Log(e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        private void DrawLoop() {
            PoolTouhou.Logger.Log("开始渲染线程循环");
            try {
                long last = 0;
                while (window.Exists && running) {
                    long now = Watch.ElapsedTicks;
                    double delta = Stopwatch.Frequency / (double) (now - last);
                    OpenGLNative.glClearColor(0, 1, 1, 0.5f);
                    OpenGLNative.glClear(ClearBufferMask.ColorBufferBit);
                    PoolTouhou.GameState.Draw(delta);
                    Sdl2Native.SDL_GL_SwapWindow(window.SdlWindowHandle);
                    GlUtil.CheckGlError();
                    last = now;
                }
            } catch (Exception e) {
                running = false;
                PoolTouhou.Logger.Log(e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        public void Init() {
            new Thread(
                () => {
                    device = VeldridStartup.CreateGraphicsDevice(
                        window,
                        new GraphicsDeviceOptions {
                            SyncToVerticalBlank = true
                        },
                        GraphicsBackend.OpenGL
                    );
                    Sdl2Native.SDL_GL_MakeCurrent(window.SdlWindowHandle, Sdl2Native.SDL_GL_CreateContext(window.SdlWindowHandle));
                    Sdl2Native.SDL_GL_SetSwapInterval(1);
                    PoolTouhou.Logger.Log($"Using : {device.BackendType}");
                    GlUtil.CheckGlError();
                    PoolTouhou.GameState = new LoadingMenuState();
                    new Thread(UpdateLoop).Start();
                    DrawLoop();
                }
            ).Start();
        }

        public GameWindow() {
            var windowInfo = new WindowCreateInfo(100, 100, 1600, 900, WindowState.Normal, "PoolTouhou");
            window = VeldridStartup.CreateWindow(windowInfo);
        }


        public void Dispose() {
            PoolTouhou.Logger.Log("开始释放窗口的托管资源");
            device?.Dispose();
            running = false;
        }

        public void RunMessageLoop() {
            while (window.Exists && running) {
                var input = window.PumpEvents();
            }
            if (window.Exists) {
                PoolTouhou.Logger.Log("窗口不存在 退出消息循环");
            }
            running = false;
        }
    }
}