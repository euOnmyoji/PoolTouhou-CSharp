using PoolTouhou.Games;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;

namespace PoolTouhou.GameStates {
    public class GamingState : GameState {
        public GamingState(IGame game) : base(game) {
            game.Load();
        }

        public override void Draw(RenderTarget renderTarget) {
            game.Draw(renderTarget);
        }

        public override void Update(ref InputData input) {
            game.Update(ref input);
            if (game.IsExit()) {
                PoolTouhou.GameState = new MenuState();
            }
        }

        public override string GetStateName() => "Gaming";

        public override void Dispose() {
            game.Dispose();
        }
    }
}