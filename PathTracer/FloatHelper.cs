namespace PathTracer
{
    public static class FloatHelper
    {
        #region Static Constructors

        static FloatHelper()
        {
            FloatHelper.Epsilon = 0.0001F;
        }

        #endregion

        #region Static Fields

        public static float Epsilon;

        #endregion
    }
}