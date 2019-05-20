using PoolTouhou.Manager;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;

namespace PoolTouhou.GameState {
    public class GamingState : IGameState {
        private readonly IGame game;

        public GamingState(IGame game) {
            this.game = game;
        }
        public void Draw(RenderTarget renderTarget) {
            game.Draw(renderTarget);
        }

        public void Update(ref InputData input) {
            game.Update(ref input);
        }

        public string GetStateName() => "Gaming";
    }
}