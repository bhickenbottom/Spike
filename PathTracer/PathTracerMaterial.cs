namespace PathTracer
{
    public class PathTracerMaterial
    {
        #region Properties

        public PathTracerColor Color { get; set; }

        public float Gloss { get; set; }

        public bool IsLight { get; set; }

        public float Reflectivity { get; set; }

        #endregion
    }
}