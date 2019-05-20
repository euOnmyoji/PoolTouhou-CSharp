using System;
using System.Collections.Generic;
using PoolTouhou.UI;

namespace PoolTouhou.Manager {
    public static class GameManager {
        private static readonly Dictionary<string, IGame> dictionary = new Dictionary<string, IGame>();

        public static void Register(IGame game) {
            dictionary.Add(game.Name, game);
        }

        public static Dictionary<string, IGame> RegisteredGames => new Dictionary<string, IGame>(dictionary);
    }

    public interface IGame : IDrawable, IUpdateable {
        string Name { get; }

        Random Random { get; }

        void Load();

        bool IsExit();
    }
}