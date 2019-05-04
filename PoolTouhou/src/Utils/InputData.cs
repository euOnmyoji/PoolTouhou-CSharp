using System.Collections.Generic;
using System.Windows.Forms;

namespace PoolTouhou.Utils {
    public class InputData {
        public static readonly HashSet<int> KEY_PRESSED = new HashSet<int>();
        private static InputData last { get; set; } = empty();
        public int shoot { get; private set; }
        public int spell { get; private set; }
        public int up { get; private set; }
        public int down { get; private set; }
        public int left { get; private set; }
        public int right { get; private set; }

        public InputData() {

            shoot = KEY_PRESSED.Contains('Z') ? last.shoot + 1 : 0;
            spell = KEY_PRESSED.Contains('X') ? last.spell + 1 : 0;
            up = KEY_PRESSED.Contains((int) Keys.Up) ? last.up + 1 : 0;
            down = KEY_PRESSED.Contains((int) Keys.Down) ? last.down + 1 : 0;
            left = KEY_PRESSED.Contains((int) Keys.Left) ? last.left + 1 : 0;
            right = KEY_PRESSED.Contains((int) Keys.Right) ? last.right + 1 : 0;
        }

        public void step() {
            last = this;
        }

        private static InputData empty() {
            return new InputData {
                shoot = 0,
                spell = 0,
                up = 0,
                down = 0,
                left = 0,
                right = 0
            };
        }
    }
}