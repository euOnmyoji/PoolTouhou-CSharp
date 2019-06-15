using System.Diagnostics;

namespace PoolTouhou.Utils {
    public static class Math {
        /// <summary>
        /// √2 / 2 的值       为了防止不同机器的cpu精度不同 我选择用计算器算好后赋值
        /// </summary>
        public const double HALF_SQRT_TWO = 0.70710678118654752440084436210485;

        public static readonly double ONE_MS_COUNT = Stopwatch.Frequency / 1000.0;
    }
}