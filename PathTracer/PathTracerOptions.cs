using System;
using System.Collections.Generic;

namespace PathTracer
{
    public class PathTracerOptions
    {
        #region Constructors

        public PathTracerOptions()
        {
            this.DepthRenderingFalloff = 1000;
            this.PixelSampleRate = 1;
            this.Shaders = new List<Func<PathTracerScene, int, int, float, PathTracerColor, PathTracerColor>>();
        }

        #endregion

        #region Properties

        public int BounceCount { get; set; }

        public float DepthRenderingFalloff { get; set; }

        public int FrameCount { get; set; }

        public int Height { get; set; }

        public int MaxDegreeOfParallelism { get; set; }

        public Action<int, float, TimeSpan> PercentageDisplay { get; set; }

        public float PixelSampleRate { get; set; }

        public PathTracerRenderMode RenderMode { get; set; }

        public int SamplesPerPixel { get; set; }

        public List<Func<PathTracerScene, int, int, float, PathTracerColor, PathTracerColor>> Shaders { get; private set; }

        public int Width { get; set; }

        #endregion
    }
}