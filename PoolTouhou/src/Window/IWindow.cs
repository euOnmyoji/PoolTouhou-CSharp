using System;

namespace PoolTouhou.Window {
    public interface IWindow : IDisposable {
        void Draw();

        bool Init();

        IntPtr GlContext { get; }

        void RunMessageLoop();
    }
}