using System;
namespace Helpers.Double
{
    public static class DoubleHelper
    {
        public static double FixNan(this double input)
        {
            return double.IsNaN(input) || double.IsInfinity(input) ? 0 : Math.Round(input, 2);
        }
    }
}

