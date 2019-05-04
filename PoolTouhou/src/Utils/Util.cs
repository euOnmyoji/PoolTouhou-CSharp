using System;
using SharpDX.WIC;

namespace PoolTouhou.Utils {
    public static class Util {
        public static SharpDX.Direct2D1.Bitmap loadBitMapFromFile(string path, Guid guid) {
            var imageFactory = new ImagingFactory2();
            if(!System.IO.File.Exists(path)) {
                Console.WriteLine(@"error about path:" + path);
            } else {
                Console.WriteLine(@"loading bitmap:" + path);
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
