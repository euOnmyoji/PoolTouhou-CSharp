using SharpDX.Direct2D1;

namespace PoolTouhou.UI {
    public interface IDrawable {
        void draw(RenderTarget renderTarget);
    }
}