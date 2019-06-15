using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PoolTouhou.Utils {
    [Flags] public enum MoveLocation : byte {
        NONE = 0,
        UP = 1,
        DOWN = 2,
        LEFT = 4,
        RIGHT = 8,
        LEFT_UP = LEFT | UP,
        LEFT_DOWN = LEFT | DOWN,
        RIGHT_UP = RIGHT | UP,
        RIGHT_DOWN = RIGHT | DOWN
    }

    public static class MoveUtil {
        public static float GetCoordinateDelta(ref MoveLocation move, float speed = 2) {
            if (move > MoveLocation.LEFT_UP) {
                return (float) (speed * Math.HALF_SQRT_TWO);
            }
            return speed;
        }
    }

    public readonly struct InputData {
        public static readonly HashSet<int> KEY_PRESSED = new HashSet<int>();
        private static InputData last = Empty();
        public readonly bool empty;
        public readonly int shoot;
        public readonly int spell;
        public readonly int up;
        public readonly int down;
        public readonly int left;
        public readonly int right;

        public InputData(bool empty = true,
            int shoot = 0,
            int spell = 0,
            int up = 0,
            int down = 0,
            int left = 0,
            int right = 0) {
            this.empty = empty;
            this.shoot = shoot;
            this.spell = spell;
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
        }

        public static InputData GetData() {
            if (KEY_PRESSED.Count > 0) {
                return new InputData(
                    false,
                    KEY_PRESSED.Contains('Z') ? last.shoot + 1 : 0,
                    KEY_PRESSED.Contains('X') ? last.spell + 1 : 0,
                    KEY_PRESSED.Contains((int) Keys.Up) ? last.up + 1 : 0,
                    KEY_PRESSED.Contains((int) Keys.Down) ? last.down + 1 : 0,
                    KEY_PRESSED.Contains((int) Keys.Left) ? last.left + 1 : 0,
                    KEY_PRESSED.Contains((int) Keys.Right) ? last.right + 1 : 0
                );
            }
            return new InputData();
        }

        public MoveLocation MoveLocation {
            get {
                if (IsNoMove()) {
                    return MoveLocation.NONE;
                }
                if (up > down) {
                    if (left > right) {
                        return MoveLocation.LEFT_UP;
                    }
                    return left == right ? MoveLocation.UP : MoveLocation.RIGHT_UP;
                }
                if (up == down) {
                    return left > right ? MoveLocation.LEFT : MoveLocation.RIGHT;
                }
                if (left > right) {
                    return MoveLocation.LEFT_DOWN;
                }
                return left == right ? MoveLocation.DOWN : MoveLocation.RIGHT_DOWN;
            }
        }

        public bool IsNoMove() {
            return up == down && left == right;
        }

        public void Step() {
            last = this;
        }

        private static InputData Empty() {
            return new InputData();
        }
    }
}