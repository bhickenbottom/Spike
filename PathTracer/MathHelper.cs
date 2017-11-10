using System;
using System.Numerics;

namespace PathTracer
{
    public static class MathHelper
    {
        #region Static Constructors

        static MathHelper()
        {
            MathHelper.PI = (float) Math.PI;
        }

        #endregion

        #region Static Fields

        public static float PI;

        #endregion

        #region Static Methods

        public static float Lerp(float value1, float value2, float amount)
        {
            return value1 + (value2 - value1) * amount;
        }

        public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount)
        {
            Vector3 result = new Vector3(MathHelper.Lerp(value1.X, value2.X, amount), MathHelper.Lerp(value1.Y, value2.Y, amount), MathHelper.Lerp(value1.Z, value2.Z, amount));
            result = Vector3.Normalize(result);
            return result;
        }

        #endregion
    }
}