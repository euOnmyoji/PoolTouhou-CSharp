using PoolTouhou.UI;

namespace PoolTouhou.GameState {
    public interface IGameState : IDrawable, IUpdateable {
        string GetStateName();
    }
}