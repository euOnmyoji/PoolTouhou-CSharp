using System;
using System.Collections.Generic;
using PoolTouhou.Bullets;
using PoolTouhou.GameObject.Player;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;

namespace PoolTouhou.Games.PoolRush {
    public class PoolRush : IGame {
        private Player player;
        private Utils.LinkedList<IBullet> bullets = new Utils.LinkedList<IBullet>();

        private bool exit;
        private static GameRegion gameRegion = new GameRegion(0, 1600, 0, 900);
        public string Name => @"PoolRush";
        public ICollection<IBullet> Bullets => bullets;
        public Random Random { get; } = new Random();

        public void Draw(RenderTarget renderTarget) {
            renderTarget.Clear(null);
            player.Draw(renderTarget);
            var bulletNode = bullets.Header;

            while (bulletNode != null) {
                var bullet = bulletNode.value;
                bullet.Draw(renderTarget);
                bulletNode = bulletNode.Next;
            }
        }

        public void Update(ref InputData input) {
            if (input.spell == 1) {
                exit = true;
            }
            player.Update(ref input);
            var bulletNode = bullets.Header;
            while (bulletNode != null) {
                var bullet = bulletNode.value;
                bullet.Update(ref input);
                if (bullet.IsCollided(player)) {
                    player.dying = 30;
                }
                if (bullet.IsDead) {
                    bulletNode.Remove();
                }
                bulletNode = bulletNode.Next;
            }
        }


        public void Load() {
            player = new Reimu(this);
            //todo: h灰灰出来挨打
            PoolTouhou.SoundManager.Load("bgmtest", @"res/bgm/dff2.wav");
            PoolTouhou.SoundManager.Loop("bgmtest");
        }

        public bool IsExit() => exit;
        public GameRegion GameRegion => gameRegion;

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