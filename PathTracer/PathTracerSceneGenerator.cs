using System.Numerics;

namespace PathTracer
{
    public static class PathTracerSceneGenerator
    {
        #region Static Methods

        public static void GenerateAbstract(PathTracerScene scene, Vector3 minimumBounds, Vector3 maximumBounds, int count, float alphaProbability, float lightProbability)
        {
            for (int i = 0; i < count; i++)
            {
                PathTracerMaterial material = new PathTracerMaterial();
                PathTracerColor color = new PathTracerColor();
                color.A = 1;
                color.R = RandomHelper.RandomFloat(0.1F, 0.9F);
                color.G = RandomHelper.RandomFloat(0.1F, 0.9F);
                color.B = RandomHelper.RandomFloat(0.1F, 0.9F);
                material.Color = color;
                material.Gloss = RandomHelper.RandomFloat();
                material.Reflectivity = RandomHelper.RandomFloat() * 0.5F;
                if (RandomHelper.RandomFloat() < alphaProbability)
                {
                    color.A = RandomHelper.RandomFloat(0.1F, 0.9F);
                }
                else if (RandomHelper.RandomFloat() < lightProbability)
                {
                    color.A = 1;
                    color.R = RandomHelper.RandomFloat(10, 20);
                    color.G = RandomHelper.RandomFloat(10, 20);
                    color.B = RandomHelper.RandomFloat(10, 20);
                    material.IsLight = true;
                }
                Vector3 p0 = new Vector3();
                p0.X = RandomHelper.RandomFloat(minimumBounds.X, maximumBounds.X);
                p0.Y = RandomHelper.RandomFloat(minimumBounds.Y, maximumBounds.Y);
                p0.Z = RandomHelper.RandomFloat(minimumBounds.Z, maximumBounds.Z);
                Vector3 p1 = new Vector3();
                p1.X = RandomHelper.RandomFloat(minimumBounds.X, maximumBounds.X);
                p1.Y = RandomHelper.RandomFloat(minimumBounds.Y, maximumBounds.Y);
                p1.Z = RandomHelper.RandomFloat(minimumBounds.Z, maximumBounds.Z);
                Vector3 p2 = new Vector3();
                p2.X = RandomHelper.RandomFloat(minimumBounds.X, maximumBounds.X);
                p2.Y = RandomHelper.RandomFloat(minimumBounds.Y, maximumBounds.Y);
                p2.Z = RandomHelper.RandomFloat(minimumBounds.Z, maximumBounds.Z);
                PathTracerTriangle triangle = new PathTracerTriangle(p0, p1, p2, material);
                scene.Triangles.Add(triangle);
            }
        }

        public static void GenerateLightBox(PathTracerScene scene, Vector3 lowerBounds, Vector3 upperBounds)
        {
            // Dimensions
            Vector3 boxArea = new Vector3(upperBounds.X - lowerBounds.X, upperBounds.Y - lowerBounds.Y, upperBounds.Z - lowerBounds.Z);

            // Colors
            PathTracerColor red = new PathTracerColor(1, 1, 0, 0.392F);
            PathTracerColor safeRed = PathTracerColor.MakeMaterialSafe(ref red);
            PathTracerColor yellow = new PathTracerColor(1, 1, 0.784F, 0);
            PathTracerColor safeYellow = PathTracerColor.MakeMaterialSafe(ref yellow);
            PathTracerColor green = new PathTracerColor(1, 0.196F, 0.784F, 0);
            PathTracerColor safeGreen = PathTracerColor.MakeMaterialSafe(ref green);
            PathTracerColor blue = new PathTracerColor(1, 0.392F, 0.294F, 1);
            PathTracerColor safeBlue = PathTracerColor.MakeMaterialSafe(ref blue);

            // Back Wall
            PathTracerMaterial backWallMaterial = new PathTracerMaterial();
            backWallMaterial.Color = safeYellow;
            Vector3 backWallTopLeft = new Vector3(lowerBounds.X, upperBounds.Y, lowerBounds.Z);
            Vector3 backWallTopRight = new Vector3(upperBounds.X, upperBounds.Y, lowerBounds.Z);
            Vector3 backWallBottomLeft = new Vector3(lowerBounds.X, lowerBounds.Y, lowerBounds.Z);
            Vector3 backWallBottomRight = new Vector3(upperBounds.X, lowerBounds.Y, lowerBounds.Z);
            PathTracerTriangle backWallTriangle1 = new PathTracerTriangle(backWallTopLeft, backWallTopRight, backWallBottomRight, backWallMaterial);
            scene.Triangles.Add(backWallTriangle1);
            PathTracerTriangle backWallTriangle2 = new PathTracerTriangle(backWallTopLeft, backWallBottomRight, backWallBottomLeft, backWallMaterial);
            scene.Triangles.Add(backWallTriangle2);

            // Front Wall
            PathTracerMaterial frontWallMaterial = new PathTracerMaterial();
            frontWallMaterial.Color = safeYellow;
            Vector3 frontWallTopLeft = new Vector3(upperBounds.X, upperBounds.Y, upperBounds.Z);
            Vector3 frontWallTopRight = new Vector3(lowerBounds.X, upperBounds.Y, upperBounds.Z);
            Vector3 frontWallBottomLeft = new Vector3(upperBounds.X, lowerBounds.Y, upperBounds.Z);
            Vector3 frontWallBottomRight = new Vector3(lowerBounds.X, lowerBounds.Y, upperBounds.Z);
            PathTracerTriangle frontWallTriangle1 = new PathTracerTriangle(frontWallTopLeft, frontWallTopRight, frontWallBottomRight, frontWallMaterial);
            scene.Triangles.Add(frontWallTriangle1);
            PathTracerTriangle frontWallTriangle2 = new PathTracerTriangle(frontWallTopLeft, frontWallBottomRight, frontWallBottomLeft, frontWallMaterial);
            scene.Triangles.Add(frontWallTriangle2);

            // Left Wall
            PathTracerMaterial leftWallMaterial = new PathTracerMaterial();
            leftWallMaterial.Color = safeRed;
            leftWallMaterial.Gloss = 0;
            Vector3 leftWallTopLeft = new Vector3(lowerBounds.X, upperBounds.Y, upperBounds.Z);
            Vector3 leftWallTopRight = new Vector3(lowerBounds.X, upperBounds.Y, lowerBounds.Z);
            Vector3 leftWallBottomLeft = new Vector3(lowerBounds.X, lowerBounds.Y, upperBounds.Z);
            Vector3 leftWallBottomRight = new Vector3(lowerBounds.X, lowerBounds.Y, lowerBounds.Z);
            PathTracerTriangle leftWallTriangle1 = new PathTracerTriangle(leftWallTopLeft, leftWallTopRight, leftWallBottomRight, leftWallMaterial);
            scene.Triangles.Add(leftWallTriangle1);
            PathTracerTriangle leftWallTriangle2 = new PathTracerTriangle(leftWallTopLeft, leftWallBottomRight, leftWallBottomLeft, leftWallMaterial);
            scene.Triangles.Add(leftWallTriangle2);

            // Right Wall
            PathTracerMaterial rightWallMaterial = new PathTracerMaterial();
            rightWallMaterial.Color = safeGreen;
            Vector3 rightWallTopLeft = new Vector3(upperBounds.X, upperBounds.Y, lowerBounds.Z);
            Vector3 rightWallTopRight = new Vector3(upperBounds.X, upperBounds.Y, upperBounds.Z);
            Vector3 rightWallBottomLeft = new Vector3(upperBounds.X, lowerBounds.Y, lowerBounds.Z);
            Vector3 rightWallBottomRight = new Vector3(upperBounds.X, lowerBounds.Y, upperBounds.Z);
            PathTracerTriangle rightWallTriangle1 = new PathTracerTriangle(rightWallTopLeft, rightWallTopRight, rightWallBottomRight, rightWallMaterial);
            scene.Triangles.Add(rightWallTriangle1);
            PathTracerTriangle rightWallTriangle2 = new PathTracerTriangle(rightWallTopLeft, rightWallBottomRight, rightWallBottomLeft, rightWallMaterial);
            scene.Triangles.Add(rightWallTriangle2);

            // Floor
            PathTracerMaterial floorMaterial = new PathTracerMaterial();
            floorMaterial.Color = safeBlue;
            floorMaterial.Gloss = 0.75F;
            Vector3 floorTopLeft = new Vector3(lowerBounds.X, lowerBounds.Y, lowerBounds.Z);
            Vector3 floorTopRight = new Vector3(upperBounds.X, lowerBounds.Y, lowerBounds.Z);
            Vector3 floorBottomLeft = new Vector3(lowerBounds.X, lowerBounds.Y, upperBounds.Z);
            Vector3 floorBottomRight = new Vector3(upperBounds.X, lowerBounds.Y, upperBounds.Z);
            PathTracerTriangle floorTriangle1 = new PathTracerTriangle(floorTopLeft, floorTopRight, floorBottomRight, floorMaterial);
            scene.Triangles.Add(floorTriangle1);
            PathTracerTriangle floorTriangle2 = new PathTracerTriangle(floorTopLeft, floorBottomRight, floorBottomLeft, floorMaterial);
            scene.Triangles.Add(floorTriangle2);

            // Ceiling
            PathTracerMaterial ceilingMaterial = new PathTracerMaterial();
            ceilingMaterial.Color = PathTracerColor.LightGray;
            Vector3 ceilingTopLeft = new Vector3(lowerBounds.X, upperBounds.Y, upperBounds.Z);
            Vector3 ceilingTopRight = new Vector3(upperBounds.X, upperBounds.Y, upperBounds.Z);
            Vector3 ceilingBottomLeft = new Vector3(lowerBounds.X, upperBounds.Y, lowerBounds.Z);
            Vector3 ceilingBottomRight = new Vector3(upperBounds.X, upperBounds.Y, lowerBounds.Z);
            PathTracerTriangle ceilingTriangle1 = new PathTracerTriangle(ceilingTopLeft, ceilingTopRight, ceilingBottomRight, ceilingMaterial);
            scene.Triangles.Add(ceilingTriangle1);
            PathTracerTriangle ceilingTriangle2 = new PathTracerTriangle(ceilingTopLeft, ceilingBottomRight, ceilingBottomLeft, ceilingMaterial);
            scene.Triangles.Add(ceilingTriangle2);

            // Test Object
            PathTracerMaterial testObjectMaterial = new PathTracerMaterial();
            testObjectMaterial.Color = new PathTracerColor(1, 4, 4, 4);
            testObjectMaterial.IsLight = true;
            Vector3 testObjectTopLeft = new Vector3(boxArea.X * -0.3F, boxArea.Y * 0.25F, boxArea.Z * -0.25F);
            Vector3 testObjectTopRight = new Vector3(boxArea.X * -0.3F, boxArea.Y * 0.25F, boxArea.Z * -0.5F);
            Vector3 testObjectBottomLeft = new Vector3(boxArea.X * -0.3F, lowerBounds.Y, boxArea.Z * -0.25F);
            Vector3 testObjectBottomRight = new Vector3(boxArea.X * -0.3F, lowerBounds.Y, boxArea.Z * -0.5F);
            PathTracerTriangle testObjectTriangle1 = new PathTracerTriangle(testObjectTopLeft, testObjectTopRight, testObjectBottomRight, testObjectMaterial);
            scene.Triangles.Add(testObjectTriangle1);
            PathTracerTriangle testObjectTriangle2 = new PathTracerTriangle(testObjectTopLeft, testObjectBottomRight, testObjectBottomLeft, testObjectMaterial);
            scene.Triangles.Add(testObjectTriangle2);

            // Test Object 2
            PathTracerMaterial testObject2Material = new PathTracerMaterial();
            testObject2Material.Color = safeBlue;
            Vector3 testObject2TopLeft = new Vector3(boxArea.X * 0.3F, boxArea.Y * 0.25F, boxArea.Z * -0.5F);
            Vector3 testObject2TopRight = new Vector3(boxArea.X * 0.3F, boxArea.Y * 0.25F, boxArea.Z * -0.25F);
            Vector3 testObject2BottomLeft = new Vector3(boxArea.X * 0.3F, lowerBounds.Y, boxArea.Z * -0.5F);
            Vector3 testObject2BottomRight = new Vector3(boxArea.X * 0.3F, lowerBounds.Y, boxArea.Z * -0.25F);
            PathTracerTriangle testObject2Triangle1 = new PathTracerTriangle(testObject2TopLeft, testObject2TopRight, testObject2BottomRight, testObject2Material, false);
            scene.Triangles.Add(testObject2Triangle1);
            PathTracerTriangle testObject2Triangle2 = new PathTracerTriangle(testObject2TopLeft, testObject2BottomRight, testObject2BottomLeft, testObject2Material, false);
            scene.Triangles.Add(testObject2Triangle2);
        }

        #endregion
    }
}