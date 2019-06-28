using System;
using System.Collections.Generic;
using PoolTouhou.Bullets;
using PoolTouhou.UI;

namespace PoolTouhou.Games {
    public static class GameManager {
        private static readonly Dictionary<string, IGame> dictionary = new Dictionary<string, IGame>();

        public static void Register(IGame game) {
            dictionary.Add(game.Name, game);
        }

        public static Dictionary<string, IGame> RegisteredGames => new Dictionary<string, IGame>(dictionary);
    }

    public interface IGame : IDrawable, IUpdateable, IDisposable {
        string Name { get; }

        ICollection<BulletBase> Bullets { get; }

        Random Random { get; }

        void Load();

        bool IsExit();

        GameRegion GameRegion { get; }
    }

    public readonly struct GameRegion {
        public readonly int minX;
        public readonly int maxX;
        public readonly int minY;
        public readonly int maxY;

        public GameRegion(int minX = 0, int maxX = 640, int minY = 0, int maxY = 480) {
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
        }
    }
}