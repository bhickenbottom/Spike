namespace PathTracer
{
    public interface IPathTracerFrameRecorder
    {
        #region Methods

        void AddFrame(int width, int height);

        void Complete();

        void SetPixel(int x, int y, PathTracerColor color);

        #endregion
    }
}