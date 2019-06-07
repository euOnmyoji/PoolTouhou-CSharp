using PoolTouhou.Manager;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;

namespace PoolTouhou.GameStates {
    public class GamingState : IGameState {
        private readonly IGame game;

        public GamingState(IGame game) {
            this.game = game;
            game.Load();
        }

        public void Draw(RenderTarget renderTarget) {
            game.Draw(renderTarget);
        }

        public void Update(ref InputData input) {
            game.Update(ref input);
            if (game.IsExit()) {
                PoolTouhou.GameState = new MenuState();
            }
        }

        public string GetStateName() => "Gaming";

        public void Dispose() {
            game.Dispose();
        }
    }
}