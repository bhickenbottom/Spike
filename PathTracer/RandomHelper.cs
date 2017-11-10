using System;

namespace PathTracer
{
    public static class RandomHelper
    {
        #region Static Fields

        [ThreadStatic] private static Random random;

        #endregion

        #region Static Methods

        public static float RandomFloat()
        {
            if (RandomHelper.random == null)
            {
                RandomHelper.random = new Random();
            }
            return (float) RandomHelper.random.NextDouble();
        }

        public static float RandomFloat(float minimum, float maximum)
        {
            float randomFloat = RandomHelper.RandomFloat();
            float range = maximum - minimum;
            float scaledRandomFloat = range * randomFloat;
            return minimum + scaledRandomFloat;
        }

        #endregion
    }
}