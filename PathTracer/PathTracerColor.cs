using System;
using System.Numerics;

namespace PathTracer
{
    public struct PathTracerColor
    {
        #region Static Fields

        public static PathTracerColor Black = new PathTracerColor(1, 0, 0, 0);

        public static PathTracerColor Blue = new PathTracerColor(1, 0, 0, 1);

        public static PathTracerColor Cyan = new PathTracerColor(1, 0, 1, 1);

        public static PathTracerColor DarkGray = new PathTracerColor(1, 0.25F, 0.25F, 0.25F);

        public static PathTracerColor Gray = new PathTracerColor(1, 0.5F, 0.5F, 0.5F);

        public static PathTracerColor Green = new PathTracerColor(1, 0, 1, 0);

        public static PathTracerColor LightGray = new PathTracerColor(1, 0.75F, 0.75F, 0.75F);

        public static PathTracerColor Magenta = new PathTracerColor(1, 1, 0, 1);

        public static PathTracerColor Red = new PathTracerColor(1, 1, 0, 0);

        public static PathTracerColor White = new PathTracerColor(1, 1, 1, 1);

        public static PathTracerColor Yellow = new PathTracerColor(1, 1, 1, 0);

        #endregion

        #region Static Methods

        public static PathTracerColor Add(ref PathTracerColor color1, ref PathTracerColor color2)
        {
            PathTracerColor result = new PathTracerColor();
            result.A = color1.A + color2.A;
            result.R = color1.R + color2.R;
            result.G = color1.G + color2.G;
            result.B = color1.B + color2.B;
            return result;
        }

        public static PathTracerColor Divide(ref PathTracerColor color1, ref float value)
        {
            PathTracerColor result = new PathTracerColor();
            result.A = color1.A / 1;
            result.R = color1.R / value;
            result.G = color1.G / value;
            result.B = color1.B / value;
            return result;
        }

        public static PathTracerColor Divide(ref PathTracerColor color1, ref PathTracerColor color2)
        {
            PathTracerColor result = new PathTracerColor();
            result.A = color1.A / color2.A;
            result.R = color1.R / color2.R;
            result.G = color1.G / color2.G;
            result.B = color1.B / color2.B;
            return result;
        }

        public static PathTracerColor FromNormal(Vector3 normal)
        {
            return new PathTracerColor(1, (normal.X + 1) / 2, (normal.Y + 1) / 2, (normal.Z + 1) / 2);
        }

        public static PathTracerColor MakeMaterialSafe(ref PathTracerColor color1)
        {
            float max = 0.9F;
            float min = 0.1F;
            PathTracerColor result = new PathTracerColor();
            result.A = Math.Min(color1.A, 1);
            result.A = Math.Max(result.A, 0);
            result.R = Math.Min(color1.R, max);
            result.R = Math.Max(result.R, min);
            result.G = Math.Min(color1.G, max);
            result.G = Math.Max(result.G, min);
            result.B = Math.Min(color1.B, max);
            result.B = Math.Max(result.B, min);
            return result;
        }

        public static PathTracerColor Multiply(ref PathTracerColor color1, ref PathTracerColor color2)
        {
            PathTracerColor result = new PathTracerColor();
            result.A = color1.A * color2.A;
            result.R = color1.R * color2.R;
            result.G = color1.G * color2.G;
            result.B = color1.B * color2.B;
            return result;
        }

        public static PathTracerColor Multiply(ref PathTracerColor color1, ref float value)
        {
            PathTracerColor result = new PathTracerColor();
            result.A = color1.A * 1;
            result.R = color1.R * value;
            result.G = color1.G * value;
            result.B = color1.B * value;
            return result;
        }

        public static PathTracerColor Subtract(ref PathTracerColor color1, ref PathTracerColor color2)
        {
            PathTracerColor result = new PathTracerColor();
            result.A = color1.A - color2.A;
            result.R = color1.R - color2.R;
            result.G = color1.G - color2.G;
            result.B = color1.B - color2.B;
            return result;
        }

        #endregion

        #region Constructors

        public PathTracerColor(float a, float r, float g, float b)
        {
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;
        }

        #endregion

        #region Properties

        public float A { get; set; }

        public float B { get; set; }

        public float G { get; set; }

        public float R { get; set; }

        #endregion

        #region Methods

        public void Clamp()
        {
            this.A = Math.Min(this.A, 1);
            this.A = Math.Max(this.A, 0);
            this.R = Math.Min(this.R, 1);
            this.R = Math.Max(this.R, 0);
            this.G = Math.Min(this.G, 1);
            this.G = Math.Max(this.G, 0);
            this.B = Math.Min(this.B, 1);
            this.B = Math.Max(this.B, 0);
        }

        #endregion

        #region Method Overrides

        public override string ToString()
        {
            return $"(A: {this.A}, R: {this.R}, G: {this.G}, B: {this.B})";
        }

        #endregion
    }
}