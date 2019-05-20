using System;
using PoolTouhou.Manager;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;

namespace PoolTouhou.Games.PoolRush {
    public class PoolRush : IGame {
        public string Name => @"PoolRush";
        public Random Random { get; } = new Random();

        public void Draw(RenderTarget renderTarget) {
        }

        public void Update(ref InputData input) {

        }


        public void Load() {

        }

        public bool IsExit() {
            return false;
        }
    }

    public static class MainClass {
        public static void OnLoad() {
            GameManager.Register(new PoolRush());
        }
    }
}