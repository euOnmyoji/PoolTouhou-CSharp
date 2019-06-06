using System;
using System.Windows.Forms;
using PoolTouhou.Utils;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.DXGI;
using Factory = SharpDX.Direct2D1.Factory;
using FeatureLevel = SharpDX.Direct2D1.FeatureLevel;

namespace PoolTouhou.Device {
    public class DirectXResource : IDisposable {
        public readonly Factory d2dFactory;
        public readonly SharpDX.DXGI.Factory dxgiFactory;

        public RenderTarget RenderTarget {
            get => renderTarget;
            private set => renderTarget = value;
        }

        internal DirectXResource() {
            d2dFactory = new Factory();
            ComObject.LogMemoryLeakWarning = Logger.MemoryLack;
            var p = new PixelFormat();
            var form = PoolTouhou.MainForm;
            var h = new HwndRenderTargetProperties {
                Hwnd = form.Handle, PixelSize = new Size2(form.Width, form.Height),
                PresentOptions = PresentOptions.None
            };
            var r = new RenderTargetProperties(
                RenderTargetType.Hardware,
                p,
                0,
                0,
                RenderTargetUsage.None,
                FeatureLevel.Level_DEFAULT
            );
            renderTarget = new WindowRenderTarget(d2dFactory, r, h);
            Create3DDevice(form);
        }

        public void Dispose() {
            renderTarget?.Dispose();
            d2dFactory?.Dispose();
            dxgiFactory?.Dispose();
        }

        private RenderTarget renderTarget;


        private void Create3DDevice(Control form) {
            var sd = new SwapChainDescription {
                BufferCount = 1,
                OutputHandle = form.Handle,
                IsWindowed = true,
                Usage = Usage.RenderTargetOutput,
                ModeDescription = {
                    Height = form.Height,
                    Width = form.Width,
                    Format = Format.R8G8B8A8_SNorm,
                    RefreshRate = {
                        Denominator = 1,
                        Numerator = 60
                    }
                },
                SampleDescription = {
                    Count = 1,
                    Quality = 0
                }
            };
            var d = new SharpDX.Direct3D11.Device(DriverType.Reference);
        }
    }
}