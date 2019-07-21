using System.Windows.Forms;
using PoolTouhou.UI.Buttons;
using PoolTouhou.Utils;
using Button = PoolTouhou.UI.Buttons.Button;

namespace PoolTouhou.UI {
    public class TitleMenuUi : IUi {
        private readonly Button[] buttons = {new GameStartButton(), new ExitButton()};
        private int curSelect;

        public void Draw(double deltaTime) {
            foreach (var button in buttons) {
                button.Draw(deltaTime);
            }
        }

        public int Update(ref InputData input) {
            if (input.empty) return UiEvents.FINE;
            int result = UiEvents.FINE;

            if (input.shoot == 1 || input.enter == 1) {
                result = buttons[curSelect].Click();
            } else if (input.spell == 1) {
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
                if (input.down > firstCd && input.down % cd == 1 || input.down == 1) {
                    buttons[curSelect++].Unselect();
                    if (curSelect >= buttons.Length) { curSelect = 0; }

                    buttons[curSelect].Select();
                } else if (input.up > firstCd && input.up % cd == 1 || input.up == 1) {
                    buttons[curSelect--].Unselect();
                    if (curSelect < 0) { curSelect = buttons.Length - 1; }

                    buttons[curSelect].Select();
                }
            }
            return result;
        }
    }
}