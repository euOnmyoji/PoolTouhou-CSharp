using PoolTouhou.Utils;
using SharpDX.Direct2D1;

namespace PoolTouhou.UI {
    public class GameChooseUi : IUi {
        public void Draw(RenderTarget renderTarget) {
            renderTarget.Clear(null);
        }

        public int Update(ref InputData input) {
            if (input.Spell > 0) {
                return UiEvents.EXIT;
            }

            return UiEvents.FINE;
        }
    }
}