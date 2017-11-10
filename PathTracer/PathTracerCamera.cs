using System;
using System.Numerics;

namespace PathTracer
{
    public class PathTracerCamera
    {
        #region Constructors

        public PathTracerCamera()
        {
            this.AntiAliased = true;
            this.Distance = 1;
            this.HorizontalFieldOfView = AngleHelper.ToRadians(60);
        }

        #endregion

        #region Properties

        public bool AntiAliased { get; set; }

        public Vector3 Direction { get; set; }

        public float Distance { get; set; }

        public double HorizontalFieldOfView { get; set; }

        public Vector3 Position { get; set; }

        public Vector3 Up { get; set; }

        #endregion

        #region Virtual Methods

        public virtual PathTracerRay GetRay(int width, int height, int x, int y)
        {
            // Camera Settings
            Vector3 position = this.Position;
            Vector3 direction = this.Direction;
            Vector3 up = this.Up;
            float negativeDistance = -this.Distance;

            // Z Axis
            Vector3 cameraZAxis;
            cameraZAxis = Vector3.Negate(direction);
            cameraZAxis = Vector3.Normalize(cameraZAxis);

            // X Axis
            Vector3 cameraXAxis;
            cameraXAxis = Vector3.Cross(up, cameraZAxis);
            cameraXAxis = Vector3.Normalize(cameraXAxis);

            // Y Axis
            Vector3 cameraYAxis;
            cameraYAxis = Vector3.Cross(cameraZAxis, cameraXAxis);
            cameraYAxis = Vector3.Normalize(cameraYAxis);

            // Sample X and Y
            float sampleX = x;
            float sampleY = y;
            if (this.AntiAliased)
            {
                sampleX += RandomHelper.RandomFloat();
                sampleY += RandomHelper.RandomFloat();
            }

            // Ratios
            float widthRatio = sampleX / width;
            float heightRatio = sampleY / height;

            // Offsets
            float offsetWidth = -1 + 2 * widthRatio;
            float offsetHeight = (1 - 2 * heightRatio) * (height / (float) width);

            // HorizontalFieldOfView
            float scale = (float) (this.Distance * Math.Tan(this.HorizontalFieldOfView));
            offsetWidth *= scale;
            offsetHeight *= scale;

            // Ray Position
            Vector3 rayPosition;
            Vector3 rayPositionX;
            Vector3 rayPositionY;
            Vector3 rayPositionZ;
            rayPositionX = Vector3.Multiply(cameraXAxis, offsetWidth);
            rayPositionY = Vector3.Multiply(cameraYAxis, offsetHeight);
            rayPositionZ = Vector3.Multiply(cameraZAxis, negativeDistance);
            rayPosition = Vector3.Add(rayPositionX, rayPositionY);
            rayPosition = Vector3.Add(rayPosition, rayPositionZ);
            rayPosition = Vector3.Normalize(rayPosition);

            // Ray
            PathTracerRay ray = new PathTracerRay();
            ray.Origin = position;
            ray.Direction = rayPosition;
            return ray;
        }

        #endregion
    }
}