using System;
using PoolTouhou.Manager;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;

namespace PoolTouhou.Games.PoolRush {
    public class PoolRush : IGame {
        public void Draw(RenderTarget renderTarget) {
        }

        public void Update(ref InputData input) {
        }

        public string Name => @"PoolRush";
        public Random Random { get; } = new Random();

        public void Load() {
        }

        public void Exit() {

        }
    }

    public static class MainClass {
        public static void OnStarted() {

        }
    }
}