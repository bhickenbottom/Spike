namespace PathTracer
{
    public static class ConsoleHelper
    {
        #region Static Constructors

        static ConsoleHelper()
        {
            ConsoleHelper.consoleLock = new object();
        }

        #endregion

        #region Static Fields

        private static object consoleLock;

        #endregion
    }
}