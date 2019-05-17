using PoolTouhou.UI.Buttons;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace PoolTouhou.UI {
    public class TitleMenuUi : IUi {

        private readonly Button[] buttons = {new GameStartButton(), new TitleExitButton()};
        private int curSelect;
        public void Draw(RenderTarget target) {
            target.Clear(new RawColor4(0, 0, 0, 0));
            foreach (var button in buttons) {
                button.Draw(target);
            }
        }
        public IUi Update(ref InputData input) {
            if (input.NoInput) return this;

            if (input.Shoot == 1) {
                buttons[curSelect].Click();
            } else if (input.Spell == 1) {
                if (curSelect != 1) {
                    buttons[curSelect].Unselect();
                    curSelect = 1;
                    buttons[curSelect].Select();
                }
            } else if (!input.IsNoMove()) {
                const int cd = 10;
                const int firstCd = 30;
                if (input.Down > firstCd && input.Down % cd == 1 || input.Down == 1) {
                    buttons[curSelect++].Unselect();
                    if (curSelect >= buttons.Length) { curSelect = 0; }

                    buttons[curSelect].Select();
                } else if (input.Up > firstCd && input.Up % cd == 1 || input.Up == 1) {
                    buttons[curSelect--].Unselect();
                    if (curSelect < 0) { curSelect = buttons.Length - 1; }

                    buttons[curSelect].Select();
                }
            }
            return this;
        }
    }
}