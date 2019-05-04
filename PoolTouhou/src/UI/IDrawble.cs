using SharpDX.Direct2D1;

namespace PoolTouhou.UI {
    public interface IDrawble {
        void draw(RenderTarget renderTarget);
    }
}