using System;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Factory = SharpDX.Direct2D1.Factory;
using FeatureLevel = SharpDX.Direct2D1.FeatureLevel;

namespace PoolTouhou.Device {
    public class DirectXResource : IDisposable {
        public readonly Factory d2dFactory;
        public readonly SharpDX.DXGI.Factory dxgiFactory;
        public readonly SharpDX.Direct3D11.Device d3d11Device;
        public readonly SharpDX.DXGI.Device dxgiDevice;
        public readonly SwapChain swapChain;

        public void Dispose() {
            d2dFactory?.Dispose();
            dxgiFactory?.Dispose();
            d3d11Device?.Dispose();
            dxgiDevice?.Dispose();
            swapChain?.Dispose();
            renderTarget?.Dispose();
        }

        public RenderTarget RenderTarget {
            get => renderTarget;
            private set {
                renderTarget?.Dispose();
                renderTarget = value;
            }
        }

        internal DirectXResource() {
            var form = PoolTouhou.MainForm;
            ComObject.LogMemoryLeakWarning = PoolTouhou.Logger.MemoryLack;
            d2dFactory = new Factory();
            d3d11Device = new SharpDX.Direct3D11.Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport);
            dxgiDevice = ComObject.As<SharpDX.DXGI.Device>(d3d11Device.NativePointer);
            using var adapter = dxgiDevice.Adapter;
            dxgiFactory = adapter.GetParent<SharpDX.DXGI.Factory>();
            var swapChainDescription = new SwapChainDescription {
                BufferCount = 1,
                OutputHandle = form.Handle,
                IsWindowed = true,
                SwapEffect = SwapEffect.Sequential,
                Usage = Usage.RenderTargetOutput,
                ModeDescription = {
                    Width = form.Width,
                    Height = form.Height,
                    Format = Format.B8G8R8A8_UNorm,
                    Scaling = DisplayModeScaling.Unspecified,
                    RefreshRate = new Rational(60, 1)
                },
                SampleDescription = {
                    Count = 1,
                    Quality = 0
                }
            };
            swapChain = new SwapChain(dxgiFactory, d3d11Device, swapChainDescription);

            var pixelFormat = new PixelFormat(Format.Unknown, AlphaMode.Premultiplied);
            var dpi = d2dFactory.DesktopDpi;
            var prop = new RenderTargetProperties {
                MinLevel = FeatureLevel.Level_DEFAULT,
                Usage = RenderTargetUsage.None,
                DpiX = dpi.Width,
                DpiY = dpi.Height,
                Type = RenderTargetType.Default,
                PixelFormat = pixelFormat
            };
            using var surface = swapChain.GetBackBuffer<Surface>(0);
            renderTarget = new RenderTarget(d2dFactory, surface, prop);
        }


        private RenderTarget renderTarget;
    }
}