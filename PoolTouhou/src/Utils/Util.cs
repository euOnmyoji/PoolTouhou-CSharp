using System;
using System.IO;
using SharpDX.WIC;

namespace PoolTouhou.Utils {
    public static class Util {
        public static SharpDX.Direct2D1.Bitmap loadBitMapFromFile(string path, Guid guid, bool useDefault = true) {
            var imageFactory = new ImagingFactory2();
            if (!File.Exists(path) && useDefault) {
                path = @"res/404notfound.png";
            }
            var decoder = new BitmapDecoder(imageFactory, path, DecodeOptions.CacheOnDemand);
            var firstFrame = decoder.GetFrame(0);
            var convert = new FormatConverter(imageFactory);
            convert.Initialize(firstFrame, guid, BitmapDitherType.None, null, 0.0, BitmapPaletteType.Custom);

            var map = SharpDX.Direct2D1.Bitmap.FromWicBitmap(PoolTouhou.mainForm.renderTarget, convert);

            decoder.Dispose();
            firstFrame.Dispose();
            imageFactory.Dispose();
            convert.Dispose();
            return map;
        }
    }
}
