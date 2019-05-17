using PoolTouhou.UI;

namespace PoolTouhou.GameState {
    public interface IGameState : IDrawable, IUpdatble {
        string GetStateName();
    }
}