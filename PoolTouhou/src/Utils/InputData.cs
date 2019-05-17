using System.Collections.Generic;
using System.Windows.Forms;

namespace PoolTouhou.Utils {
    public class InputData {
        public static readonly HashSet<int> KEY_PRESSED = new HashSet<int>();
        private static InputData Last { get; set; } = Empty();
        public bool NoInput { get; private set; }
        public int Shoot { get; private set; }
        public int Spell { get; private set; }
        public int Up { get; private set; }
        public int Down { get; private set; }
        public int Left { get; private set; }
        public int Right { get; private set; }

        public InputData() {
            if (KEY_PRESSED.Count > 0) {
                NoInput = false;
                Shoot = KEY_PRESSED.Contains('Z') ? Last.Shoot + 1 : 0;
                Spell = KEY_PRESSED.Contains('X') ? Last.Spell + 1 : 0;
                Up = KEY_PRESSED.Contains((int) Keys.Up) ? Last.Up + 1 : 0;
                Down = KEY_PRESSED.Contains((int) Keys.Down) ? Last.Down + 1 : 0;
                Left = KEY_PRESSED.Contains((int) Keys.Left) ? Last.Left + 1 : 0;
                Right = KEY_PRESSED.Contains((int) Keys.Right) ? Last.Right + 1 : 0;
            } else {
                NoInput = true;
            }
        }

        public bool IsNoMove() {
            return Up == Down && Left == Right;
        }

        public void Step() {
            Last = this;
        }

        private static InputData Empty() {
            return new InputData {
                Shoot = 0,
                Spell = 0,
                Up = 0,
                Down = 0,
                Left = 0,
                Right = 0,
                NoInput = true
            };
        }
    }
}