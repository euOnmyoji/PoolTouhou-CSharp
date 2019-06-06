using System;
using PoolTouhou.Manager;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;

namespace PoolTouhou.Games.PoolRush {
    public class PoolRush : IGame {
        private bool exit;
        public string Name => @"PoolRush";
        public Random Random { get; } = new Random();

        public void Draw(RenderTarget renderTarget) {
            renderTarget.Clear(null);
        }

        public void Update(ref InputData input) {
            if (input.Spell == 1) {
                exit = true;
            }
        }


        public void Load() {
            PoolTouhou.SoundManager.Load("bgmtest", @"res/bgm/th06_05.wav");
            PoolTouhou.SoundManager.Loop("bgmtest");
        }

        public bool IsExit() => exit;

        public void Dispose() {
            exit = false;
            PoolTouhou.SoundManager.Unload("bgmtest");
        }
    }

    public static class MainClass {
        public static void OnLoad() {
            GameManager.Register(new PoolRush());
        }
    }
}