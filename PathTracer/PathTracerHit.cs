using System.Numerics;

namespace PathTracer
{
    public struct PathTracerHit
    {
        #region Static Constructors

        static PathTracerHit()
        {
            PathTracerHit.Miss = new PathTracerHit();
        }

        #endregion

        #region Static Fields

        public static PathTracerHit Miss;

        #endregion

        #region Fields

        public float Distance;

        public bool IsHit;

        public PathTracerMaterial Material;

        public Vector3 Normal;

        public Vector3 Position;

        #endregion
    }
}