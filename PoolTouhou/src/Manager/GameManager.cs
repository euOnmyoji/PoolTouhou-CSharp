using System;
using System.Collections.Generic;
using System.IO;
using PoolTouhou.UI;

namespace PoolTouhou.Manager {
    public static class GameManager {
        private static Dictionary<string, IGame> dictionary = new Dictionary<string, IGame>();

        public static void Register(IGame game) {
            dictionary.Add(game.Name, game);
        }
    }

    public interface IGame : IDrawable, IUpdatble {
        string Name { get; }

        Random Random { get; }

        void Load();

        void Exit();
    }
}