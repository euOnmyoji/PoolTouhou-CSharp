using SharpDX.Direct2D1;

namespace PoolTouhou.UI {
    public interface IDrawable {
        void Draw(DeviceContext d2d1Context);
    }
}