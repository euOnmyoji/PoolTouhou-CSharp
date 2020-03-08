using System;
using System.Diagnostics;
using Veldrid;
using Veldrid.OpenGL;
using Veldrid.OpenGLBinding;
using Veldrid.Sdl2;

namespace PoolTouhouFramework.Utils {
    public static class GlUtil {
        public static GraphicsDevice CreateDefaultOpenGlGraphicsDevice(Sdl2Window window) {
            SDL_SysWMinfo info;

            var sdlHandle = window.SdlWindowHandle;
            var contextHandle = Sdl2Native.SDL_GL_CreateContext(sdlHandle);
            var platformInfo = new OpenGLPlatformInfo(
                contextHandle,
                Sdl2Native.SDL_GL_GetProcAddress,
                context => Sdl2Native.SDL_GL_MakeCurrent(sdlHandle, context),
                Sdl2Native.SDL_GL_GetCurrentContext,
                () => { },
                Sdl2Native.SDL_GL_DeleteContext,
                () => Sdl2Native.SDL_GL_SwapWindow(sdlHandle),
                sync => Sdl2Native.SDL_GL_SetSwapInterval(sync ? 1 : 0)
            );
            return GraphicsDevice.CreateOpenGL(
                new GraphicsDeviceOptions(true),
                platformInfo,
                (uint) window.Width,
                (uint) window.Height
            );
        }

        public static void CheckGlError() {
            var code = (ErrorCode) OpenGLNative.glGetError();
            if (code != ErrorCode.NoError) {
                PoolTouhou.Logger.Log(
                    $"GL HAS ERROR:{code}{Environment.NewLine}{new StackTrace(1, true)}",
                    LogLevel.ERROR
                );
            }
        }
    }
}