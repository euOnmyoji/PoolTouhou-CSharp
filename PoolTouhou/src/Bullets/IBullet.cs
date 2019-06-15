using PoolTouhou.GameObject;
using PoolTouhou.UI;

namespace PoolTouhou.Bullets {
    public interface IBullet : ICollidable, IDrawable, IUpdateable, ILocateable {
        bool IsDead { get; }
    }
}