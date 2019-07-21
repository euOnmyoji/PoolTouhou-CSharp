//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using PoolTouhou.Bullets;
//using PoolTouhou.GameObject.Player;
//using PoolTouhou.Utils;
//using SharpDX.Direct2D1;
//using SharpDX.Mathematics.Interop;
//using static System.Math;
//using static PoolTouhou.PoolTouhou;
//using Bitmap = SharpDX.Direct2D1.Bitmap;
//using BitmapInterpolationMode = SharpDX.Direct2D1.BitmapInterpolationMode;
//using PixelFormat = SharpDX.WIC.PixelFormat;
//
//namespace PoolTouhou.Games.PoolRush {
//    public class PoolRush : IGame {
//        private Player player;
//        private readonly Utils.LinkedList<BulletBase> bullets = new Utils.LinkedList<BulletBase>();
//
//        private bool exit;
//        private static readonly GameRegion gameRegion = new GameRegion(0, 1600, 0, 900);
//        public string Name => @"PoolRush";
//        public ICollection<BulletBase> Bullets => bullets;
//        public Random Random { get; } = new Random();
//
//        private Bitmap zzzz;
//        private long firstDrawTime;
//
//
//        public void Draw(DeviceContext renderTarget) {
//            using var factory = renderTarget.Factory;
//            renderTarget.Clear(new RawColor4(0.125f, 0.125f, 0.125f, 1));
//            player.Draw(renderTarget);
//            var bulletNode = bullets.Header;
//            double delta = 0;
//            if (firstDrawTime == 0) {
//                firstDrawTime = Watch.ElapsedTicks;
//            } else {
//                delta = PI * ((Watch.ElapsedTicks - firstDrawTime) * 0.5 / Stopwatch.Frequency);
//            }
//            while (bulletNode != null) {
//                var bullet = bulletNode.value;
//                bullet.Draw(renderTarget);
//                bulletNode = bulletNode.Next;
//            }
//            var playerPoint = player.Point;
//            (double x, double y) = Utils.Math.RotatePoint(delta, 128, 128);
//            renderTarget.Transform = Utils.Math.RotateMatrix(
//                delta * 16,
//                (float) x + playerPoint.X,
//                (float) y + playerPoint.Y
//            );
//            renderTarget.DrawBitmap(zzzz, new RawRectangleF(-64, -64, 64, 64), 1, BitmapInterpolationMode.Linear);
//
//            using var pathGeom = new PathGeometry(factory);
//            using var sink = pathGeom.Open();
//            const double degree = PI / 2.5;
//            var points = new (double x, double y)[5];
//            points[0] = (0, 50);
//            for (int i = 1; i < 5; ++i) {
//                points[i] = Utils.Math.RotatePoint(degree * i, 0, 50);
//            }
//            sink.BeginFigure(new RawVector2((float) points[3].x, (float) points[3].y), FigureBegin.Hollow);
//            sink.AddLine(new RawVector2((float) points[1].x, (float) points[1].y));
//            sink.AddLine(new RawVector2((float) points[4].x, (float) points[4].y));
//            sink.AddLine(new RawVector2((float) points[2].x, (float) points[2].y));
//            sink.AddLine(new RawVector2((float) points[0].x, (float) points[0].y));
//            sink.EndFigure(FigureEnd.Closed);
//            sink.BeginFigure(new RawVector2(0, 50), FigureBegin.Hollow);
//            sink.EndFigure(FigureEnd.Closed);
//            using var brush = new SolidColorBrush(renderTarget, new RawColor4(1, 0, 0, 1));
//            double magic = delta % (PI * 2);
//
//            if (magic > PI) {
//                magic = 2 * PI - magic;
//            } else {
//                magic = magic + PI * 0.5;
//            }
//            float xS = (float) (magic / PI);
//            float yS = 2 - xS;
//            renderTarget.Transform = Utils.Math.RotateMatrix(delta, 700, 200, xS * 2, yS * 2);
//            sink.Dispose();
//            using var geom = new TransformedGeometry(factory, pathGeom, Utils.Math.RotateMatrix(0, 0, 0));
//            renderTarget.DrawGeometry(geom, brush);
//            var ellipse = new Ellipse(new RawVector2(0, 0), 50, 50);
//            renderTarget.DrawEllipse(ellipse, brush);
////            renderTarget.Flush();
//            renderTarget.Transform = Utils.Math.RotateMatrix(0, 0, 0);
//            using var biu = new SolidColorBrush(renderTarget, new RawColor4(0.25f, 0.25f, 0, 0.5f));
//            var effect = new Effect(renderTarget, Effect.Blend);
//            renderTarget.FillEllipse(
//                new Ellipse(new RawVector2(600, 400), (float) delta * 100, (float) delta * 100),
//                biu
//            );
//            renderTarget.FillEllipse(
//                new Ellipse(new RawVector2(550, 400), (float) delta * 50, (float) delta * 50),
//                biu
//            );
//            renderTarget.FillEllipse(
//                new Ellipse(new RawVector2(650, 400), (float) delta * 50, (float) delta * 50),
//                biu
//            );
//            renderTarget.FillEllipse(
//                new Ellipse(new RawVector2(600, 350), (float) delta * 50, (float) delta * 50),
//                biu
//            );
//            renderTarget.FillEllipse(
//                new Ellipse(new RawVector2(600, 450), (float) delta * 50, (float) delta * 50),
//                biu
//            );
////            context.OutputMerger.SetBlendState(old, oldColor4, oldSampleMask);
//        }
//
//        public void Update(ref InputData input) {
//            if (input.spell == 1) {
//                exit = true;
//            }
//            player.Update(ref input);
//            var bulletNode = bullets.Header;
//            while (bulletNode != null) {
//                var bullet = bulletNode.value;
//                bullet.Update(ref input);
//                if (bullet.IsCollided(player)) {
//                    player.dying = 30;
//                }
//                if (bullet.dead) {
//                    bulletNode.Remove();
//                }
//                bulletNode = bulletNode.Next;
//            }
//        }
//
//
//        public void Load() {
//            player = new Reimu(this);
//            zzzz = Util.LoadBitMapFromFile("res/zzzz/zzzz.png", PixelFormat.Format32bppPRGBA);
//            //todo: h灰灰出来挨打
////            SoundManager.Load("bgmtest", @"res/bgm/dff2.wav");
////            SoundManager.Loop("bgmtest");
//        }
//
//        public bool IsExit() => exit;
//        public GameRegion GameRegion => gameRegion;
//
//        public void Dispose() {
//            exit = false;
//            firstDrawTime = 0;
////            SoundManager.Unload("bgmtest");
//        }
//    }
//
//    public static class MainClass {
//        public static void OnLoad() {
//            GameManager.Register(new PoolRush());
//        }
//    }
//}