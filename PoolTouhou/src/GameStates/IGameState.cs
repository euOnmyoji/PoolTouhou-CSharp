using System;
using PoolTouhou.UI;

namespace PoolTouhou.GameStates {
    public interface IGameState : IDrawable, IUpdateable, IDisposable {
        string GetStateName();
    }
}