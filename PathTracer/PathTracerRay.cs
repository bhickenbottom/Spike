using System.Numerics;

namespace PathTracer
{
    public struct PathTracerRay
    {
        #region Fields

        public Vector3 Direction;

        public Vector3 Origin;

        #endregion

        #region Method Overrides

        public override string ToString()
        {
            return $"(Origin: {this.Origin}, Direction: {this.Direction})";
        }

        #endregion
    }
}