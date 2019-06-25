using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using PoolTouhou.Bullets;
using PoolTouhou.GameObject.Player;
using PoolTouhou.Utils;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using SharpDX.WIC;
using static PoolTouhou.PoolTouhou;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using BitmapInterpolationMode = SharpDX.Direct2D1.BitmapInterpolationMode;
using BitmapRenderTarget = SharpDX.DirectWrite.BitmapRenderTarget;
using FillMode = SharpDX.Direct2D1.FillMode;
using Math = System.Math;
using PixelFormat = SharpDX.WIC.PixelFormat;

namespace PoolTouhou.Games.PoolRush {
    public class PoolRush : IGame {
        private Player player;
        private readonly Utils.LinkedList<IBullet> bullets = new Utils.LinkedList<IBullet>();

        private bool exit;
        private static readonly GameRegion gameRegion = new GameRegion(0, 1600, 0, 900);
        public string Name => @"PoolRush";
        public ICollection<IBullet> Bullets => bullets;
        public Random Random { get; } = new Random();

        private Bitmap zzzz;
        private long firstDrawTime;


        public void Draw(RenderTarget renderTarget) {
            renderTarget.Clear(null);
            player.Draw(renderTarget);
            var bulletNode = bullets.Header;
            double delta = 0;
            if (firstDrawTime == 0) {
                firstDrawTime = Watch.ElapsedTicks;
            } else {
                delta = Math.PI * ((Watch.ElapsedTicks - firstDrawTime) * 0.5 / Stopwatch.Frequency);
            }
            while (bulletNode != null) {
                var bullet = bulletNode.value;
                bullet.Draw(renderTarget);
                bulletNode = bulletNode.Next;
            }
            var playerPoint = player.Point;
            (double x, double y) = Utils.Math.RotatePoint(delta, 128, 128);
            renderTarget.Transform = Utils.Math.RotateMatrix(
                delta * 16,
                (float) x + playerPoint.X,
                (float) y + playerPoint.Y
            );
            renderTarget.DrawBitmap(zzzz, new RawRectangleF(-64, -64, 64, 64), 1, BitmapInterpolationMode.Linear);

            using var pathGeom = new PathGeometry(DxResource.d2dFactory);
            using var sink = pathGeom.Open();
            const double degree = Math.PI / 5.0;
            var points = new (double x, double y)[5];
            points[0] = (0, 1);
            for (int i = 1; i < 5; ++i) {
                points[i] = (Utils.Math.RotatePoint(degree * i, 0, 1));
            }
            sink.SetFillMode(FillMode.Winding);
            sink.BeginFigure(new RawVector2(0, 0), FigureBegin.Filled);
            sink.AddLine(new RawVector2((float) (points[1].x - points[3].x), (float) (points[1].y - points[3].y)));
            sink.EndFigure(FigureEnd.Closed);
            sink.Dispose();
            using var brush = new SolidColorBrush(renderTarget, new RawColor4(0.25f, 0.25f, 0.125f, 1));
            renderTarget.Transform = Utils.Math.RotateMatrix(delta, 700, 200, 100, 100);
            renderTarget.DrawGeometry(pathGeom, brush, 1);
            renderTarget.Flush();
            renderTarget.Transform = Utils.Math.RotateMatrix(0, 0, 0);
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
            zzzz = Util.LoadBitMapFromFile("res/zzzz/zzzz.png", PixelFormat.Format32bppPRGBA);
            //todo: h灰灰出来挨打
            SoundManager.Load("bgmtest", @"res/bgm/dff2.wav");
            SoundManager.Loop("bgmtest");
        }

        public bool IsExit() => exit;
        public GameRegion GameRegion => gameRegion;

        public void Dispose() {
            exit = false;
            SoundManager.Unload("bgmtest");
        }
    }

    public static class MainClass {
        public static void OnLoad() {
            GameManager.Register(new PoolRush());
        }
    }
}