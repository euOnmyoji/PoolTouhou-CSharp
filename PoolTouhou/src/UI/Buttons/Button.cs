using System;
using PoolTouhou.Sound;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using PixelFormat = SharpDX.WIC.PixelFormat;

namespace PoolTouhou.UI.Buttons {
    public class ButtonsResources : IDisposable {
        public static readonly ButtonsResources INSTANCE = new ButtonsResources();

        public readonly Bitmap title01;

        private ButtonsResources() {
            title01 = Util.LoadBitMapFromFile(
                @"res/title/title01.png",
                PixelFormat.Format32bppPRGBA
            );
            PoolTouhou.SoundManager.Load(
                @"selectButton",
                @"res/SE/se_select00.mp3",
                GetSoundStreamMethods.GetMp3SoundStream
            );
        }

        public void Dispose() {
            title01?.Dispose();
            PoolTouhou.SoundManager.Unload(@"selectButton");
            PoolTouhou.Logger.Info("释放按钮资源完成");
        }
    }

    public abstract class Button : IDrawable {
        protected DateTime? selectTime;

        public void Select() {
            selectTime = DateTime.Now;
            PoolTouhou.SoundManager.Overplay(@"selectButton");
        }

        public abstract int Click();
        public abstract string GetName();

        public void Unselect() {
            selectTime = null;
        }

        public abstract void Draw(RenderTarget renderTarget);

        protected static void GetOffset(double ms, out int x, out int y) {
            //16.666666666 ms ≈ 1tick
            //so just using 17 ms
            int tick = (int) (ms / 34);
            switch (tick) {
                case 1:
                case 7: {
                    x = -1;
                    y = 0;
                    break;
                }
                case 2: {
                    x = -1;
                    y = -1;
                    break;
                }
                case 3: {
                    x = 1;
                    y = 0;
                    break;
                }
                case 5: {
                    x = 0;
                    y = -1;
                    break;
                }
                case 6: {
                    x = 1;
                    y = 0;
                    break;
                }
                default: {
                    x = 0;
                    y = 0;
                    break;
                }
            }
        }
    }
}