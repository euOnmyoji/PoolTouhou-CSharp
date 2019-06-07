using System;
using PoolTouhou.Manager;
using PoolTouhou.Sound;
using PoolTouhou.Utils;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using static PoolTouhou.PoolTouhou;
using Resource = SharpDX.Direct3D11.Resource;

namespace PoolTouhou.Games.PoolRush {
    public class PoolRush : IGame {
        private bool exit;
        private int a;
        public string Name => @"PoolRush";
        public Random Random { get; } = new Random();

        public void Draw(RenderTarget renderTarget) {
            using var buffer = DxResource.swapChain.GetBackBuffer<Texture2D>(0);
            using var res = CppObject.FromPointer<Resource>(buffer.NativePointer);
            using var renderTargetView = new RenderTargetView(DxResource.d3d11Device, res);
            var context = DxResource.d3d11Device.ImmediateContext;
            context.ClearRenderTargetView(renderTargetView, new RawColor4(0.5f, 0.5f, 0.5f, 1));
        }

        public void Update(ref InputData input) {
            if (input.Spell == 1) {
                exit = true;
            }
        }


        public void Load() {
            PoolTouhou.SoundManager.Load("bgmtest", @"res/bgm/mdl.mp3",GetSoundStreamMethods.GetMp3SoundStream);
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