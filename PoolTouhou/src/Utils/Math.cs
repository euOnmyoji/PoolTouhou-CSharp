using System.Diagnostics;

namespace PoolTouhou.Utils {
    public static class Math {
        /// <summary>
        /// √2 / 2 的值       为了防止不同机器的cpu精度不同 我选择用计算器算好后赋值
        /// </summary>
        public const double HALF_SQRT_TWO = 0.70710678118654752440084436210485;

        public const double ONE_DEGREE = System.Math.PI / 180.0;
        public static readonly double ONE_MS_COUNT = Stopwatch.Frequency / 1000.0;


//        public static RawMatrix3x2 RotateMatrix(double degree, float x = 0, float y = 0) {
//            float sin = (float) System.Math.Sin(degree);
//            float cos = (float) System.Math.Cos(degree);
//            return new RawMatrix3x2(cos, sin, -sin, cos, x, y);
//        }
//
//        public static RawMatrix3x2 RotateMatrix(double degree,
//            float x = 0,
//            float y = 0,
//            float xScala = 1,
//            float yScala = 1) {
//            float sin = (float) System.Math.Sin(degree);
//            float cos = (float) System.Math.Cos(degree);
//            return new RawMatrix3x2(cos * xScala, sin * yScala, -sin * xScala, cos * yScala, x, y);
//        }

        public static (double x, double y) RotatePoint(double degree, double x, double y) {
            double sin = System.Math.Sin(degree);
            double cos = System.Math.Cos(degree);
            return (x * cos - y * sin, x * sin + y * cos);
        }
    }
}