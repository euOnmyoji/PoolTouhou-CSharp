using System;
using PoolTouhou.Games;
using PoolTouhou.UI;
using PoolTouhou.Utils;

namespace PoolTouhou.GameStates {
    public abstract class GameState : IDrawable, IUpdateable, IDisposable {
        public readonly IGame game;

        protected GameState(IGame game) {
            this.game = game;
        }

        public abstract string GetStateName();
        public abstract void Draw(double deltaTime);
        public abstract void Update(ref InputData input);
        public abstract void Dispose();
    }
}