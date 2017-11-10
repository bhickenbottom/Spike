using System;
using System.Collections.Generic;

namespace PathTracer
{
    public class PathTracerScene
    {
        #region Constructors

        public PathTracerScene()
        {
            this.Triangles = new List<PathTracerTriangle>();
        }

        #endregion

        #region Properties

        public Action<int> Animator { get; set; }

        public PathTracerMaterial BackgroundMaterial { get; set; }

        public PathTracerCamera Camera { get; set; }

        public PathTracerColor FogColor { get; set; }

        public float FogDistance { get; set; }

        public List<PathTracerTriangle> Triangles { get; set; }

        #endregion
    }
}