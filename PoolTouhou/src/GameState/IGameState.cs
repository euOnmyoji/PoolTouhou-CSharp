using PoolTouhou.UI;

namespace PoolTouhou.GameState {
    public interface IGameState : IDrawble, IUpdatble {
        string getStateName();
    }
}