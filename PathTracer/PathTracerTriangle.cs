using System;
using System.Numerics;

namespace PathTracer
{
    public struct PathTracerTriangle
    {
        #region Constructors

        public PathTracerTriangle(Vector3 v0, Vector3 v1, Vector3 v2, PathTracerMaterial material, bool clockwise = true)
        {
            this.V0 = v0;
            this.V1 = v1;
            this.V2 = v2;
            this.Material = material;
            this.Clockwise = clockwise;
        }

        #endregion

        #region Fields

        public bool Clockwise;

        public PathTracerMaterial Material;

        public Vector3 V0;

        public Vector3 V1;

        public Vector3 V2;

        #endregion

        #region Methods

        public PathTracerHit GetHit(PathTracerRay ray)
        {
            // Direction
            Vector3 direction = ray.Direction;
            Vector3 normalizedRayDirection;
            normalizedRayDirection = Vector3.Normalize(ray.Direction);

            // U and V Vectors
            Vector3 u = Vector3.Subtract(this.V1, this.V0);
            Vector3 v = Vector3.Subtract(this.V2, this.V0);

            // Normal
            Vector3 normal = Vector3.Cross(u, v);
            normal = Vector3.Normalize(normal);
            if (this.Clockwise)
            {
                normal = Vector3.Negate(normal);
            }
            if (Vector3.Dot(normalizedRayDirection, normal) > 0)
            {
                normal = Vector3.Negate(normal);
            }

            // Perpendicular Vectors
            Vector3 perpendicularToU = Vector3.Cross(normal, u);
            Vector3 perpendicularToV = Vector3.Cross(normal, v);

            // Denominator ST
            float denominatorSt = Vector3.Dot(u, perpendicularToV);

            // Broken Triangle?
            if (Math.Abs(denominatorSt) < FloatHelper.Epsilon)
            {
                // Broken triangle
                return PathTracerHit.Miss;
            }

            // Denominator T
            float denominatorT = Vector3.Dot(direction, normal);

            // Parallel?
            if (Math.Abs(denominatorT) < FloatHelper.Epsilon)
            {
                return PathTracerHit.Miss;
            }

            // Ray Origin to V0
            Vector3 rayToV0 = Vector3.Subtract(this.V0, ray.Origin);

            // Hit Distance
            float hitDistance = Vector3.Dot(rayToV0, normal);
            hitDistance /= denominatorT;

            // Plane Behind Origin?
            if (hitDistance < 0)
            {
                return PathTracerHit.Miss;
            }

            // Hit Position
            Vector3 relativeHitPosition = Vector3.Multiply(normalizedRayDirection, hitDistance);
            Vector3 hitPosition = Vector3.Add(ray.Origin, relativeHitPosition);

            // W Vector
            Vector3 w = Vector3.Subtract(hitPosition, this.V0);

            // S
            float s = Vector3.Dot(w, perpendicularToV);
            s /= denominatorSt;

            // Is In Triangle?
            if (s < 0 || s > 1)
            {
                return PathTracerHit.Miss;
            }

            // T
            float t = Vector3.Dot(w, perpendicularToU);
            t /= -denominatorSt;

            // Is Hit?
            if (t >= 0 && s + t <= 1)
            {
                PathTracerHit hit = new PathTracerHit();
                hit.IsHit = true;
                hit.Distance = hitDistance;
                hit.Position = hitPosition;
                hit.Normal = normal;
                hit.Material = this.Material;
                return hit;
            }

            // Miss
            return PathTracerHit.Miss;
        }

        #endregion
    }
}