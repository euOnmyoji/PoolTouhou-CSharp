using System;
using PoolTouhou.Utils;

namespace PoolTouhou.UI {
    public interface IUi : IDrawable {
        int Update(ref InputData input);
    }

    public static class UiEvents {
        public const int EXIT = int.MinValue;
        public const int FINE = 0;
        public const int CHOOSE_GAME = 1;
        public const int REPLY = 2;
    }
}