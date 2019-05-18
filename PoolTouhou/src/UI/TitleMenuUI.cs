using System.Windows.Forms;
using PoolTouhou.UI.Buttons;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;
using Button = PoolTouhou.UI.Buttons.Button;

namespace PoolTouhou.UI {
    public class TitleMenuUi : IUi {
        private readonly Button[] buttons = {new GameStartButton(), new ExitButton()};
        private int curSelect;

        public void Draw(RenderTarget target) {
            target.Clear(null);
            foreach (var button in buttons) {
                button.Draw(target);
            }
        }

        public int Update(ref InputData input) {
            if (input.NoInput) return UiEvents.FINE;
            int result = UiEvents.FINE;

            if (input.Shoot == 1) {
                result = buttons[curSelect].Click();
            } else if (input.Spell == 1) {
                if (curSelect != 1) {
                    buttons[curSelect].Unselect();
                    curSelect = 1;
                    buttons[curSelect].Select();
                } else {
                    Application.Exit();
                    return UiEvents.EXIT;
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
            return result;
        }
    }
}