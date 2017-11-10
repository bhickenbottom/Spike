using System;

namespace PathTracer
{
    public static class AngleHelper
    {
        #region Static Methods

        public static double ToRadians(double value)
        {
            return Math.PI / 180 * value;
        }

        #endregion
    }
}