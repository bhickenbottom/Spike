using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace PathTracer
{
    public class PathTracerEngine
    {
        #region Methods

        public void Render(PathTracerScene scene, PathTracerOptions options, IPathTracerFrameRecorder frameRecorder)
        {
            // Preconditions
            if (scene == null)
            {
                throw new ArgumentNullException();
            }
            if (scene.Camera == null)
            {
                throw new ArgumentNullException();
            }
            if (scene.BackgroundMaterial == null)
            {
                throw new ArgumentNullException();
            }
            if (options == null)
            {
                throw new ArgumentNullException();
            }

            // Dimensions
            int width = options.Width;
            int height = options.Height;

            // Pixel Count
            int pixelCount = width * height;

            // Frames
            for (int frame = 0; frame < options.FrameCount; frame++)
            {
                // Add Frame
                frameRecorder.AddFrame(width, height);

                // Percent
                int pixelsProcessed = 0;
                Stopwatch stopwatch = Stopwatch.StartNew();
                options.PercentageDisplay?.Invoke(pixelCount, 0, stopwatch.Elapsed);

                // Pixels
                ParallelOptions parallelOptions = new ParallelOptions();
                parallelOptions.MaxDegreeOfParallelism = options.MaxDegreeOfParallelism;
                Parallel.For(0, pixelCount, (pixelIndex) =>
                {
                    // Pixel Sample Rate
                    if (RandomHelper.RandomFloat() <= options.PixelSampleRate)
                    {
                        // Hack to make it play well with others
                        if (pixelIndex % 64 == 0)
                        {
                            Thread.Sleep(1);
                        }

                        // Coordinates
                        int x = pixelIndex % width;
                        int y = pixelIndex / width;

                        // Pixel
                        PathTracerColor pixel = PathTracerColor.Black;

                        // Depth
                        float depth = float.MaxValue;

                        // Distance Traveled
                        float distanceTraveled = 0;

                        // Samples
                        int samplesPerPixel = options.SamplesPerPixel;
                        for (int sampleIndex = 0; sampleIndex < samplesPerPixel; sampleIndex++)
                        {
                            // Mask
                            PathTracerColor mask = PathTracerColor.White;

                            // Light
                            PathTracerColor samplePixel = PathTracerColor.Black;

                            // Ray
                            PathTracerRay ray = scene.Camera.GetRay(width, height, x, y);

                            // Bounce
                            int bounceCount = options.BounceCount;
                            for (int bounceIndex = 0; bounceIndex < bounceCount; bounceIndex++)
                            {
                                // Closest Hit
                                float closestHit = float.MaxValue;

                                // Continue?
                                bool continueBouncing = false;

                                // Next Position
                                Vector3 nextPosition = Vector3.Zero;

                                // Next Normal
                                Vector3 nextNormal = Vector3.Zero;

                                // Material
                                PathTracerMaterial material = scene.BackgroundMaterial;

                                // Triangles
                                foreach (PathTracerTriangle triangle in scene.Triangles)
                                {
                                    PathTracerHit hit = triangle.GetHit(ray);
                                    if (hit.IsHit)
                                    {
                                        if (RandomHelper.RandomFloat() > hit.Material.Color.A)
                                        {
                                            continue;
                                        }
                                        if (hit.Distance < closestHit)
                                        {
                                            // Closest Hit
                                            closestHit = hit.Distance;

                                            // Continue
                                            continueBouncing = true;

                                            // Next
                                            nextPosition = hit.Position;
                                            nextNormal = hit.Normal;

                                            // Material
                                            material = hit.Material;
                                        }
                                    }
                                }

                                // Depth
                                if (bounceIndex == 0 && closestHit < depth)
                                {
                                    depth = closestHit;
                                }

                                // Distance Traveled
                                distanceTraveled += closestHit;

                                // Material
                                if (material.IsLight)
                                {
                                    // Light
                                    PathTracerColor color = material.Color;
                                    color = PathTracerColor.Multiply(ref color, ref mask);
                                    samplePixel = PathTracerColor.Add(ref samplePixel, ref color);
                                }
                                else
                                {
                                    // Surface
                                    PathTracerColor color = material.Color;
                                    mask = PathTracerColor.Multiply(ref mask, ref color);
                                }

                                // Color Render Mode
                                if (options.RenderMode == PathTracerRenderMode.Color)
                                {
                                    samplePixel = mask;
                                    continueBouncing = false;
                                }

                                // Depth Render Mode
                                if (options.RenderMode == PathTracerRenderMode.Depth)
                                {
                                    float channel = depth / 1000;
                                    samplePixel = new PathTracerColor(1, channel, channel, channel);
                                    continueBouncing = false;
                                }

                                // Ambient Occlusion Render Mode
                                if (bounceIndex == 1 && options.RenderMode == PathTracerRenderMode.AmbientOcclusion)
                                {
                                    float channel = closestHit / 1000;
                                    samplePixel = new PathTracerColor(1, channel, channel, channel);
                                    continueBouncing = false;
                                }

                                // Depth Render Mode
                                if (options.RenderMode == PathTracerRenderMode.DistanceTraveled)
                                {
                                    float channel = distanceTraveled / (1000 * bounceCount);
                                    samplePixel = new PathTracerColor(1, channel, channel, channel);
                                }

                                // Normals Render Mode
                                if (options.RenderMode == PathTracerRenderMode.Normals)
                                {
                                    samplePixel = PathTracerColor.FromNormal(nextNormal);
                                    continueBouncing = false;
                                }

                                // Pure Reflection
                                Vector3 pureReflection;
                                pureReflection = Vector3.Reflect(ray.Direction, nextNormal);

                                // Pure Reflections Render Mode
                                if (options.RenderMode == PathTracerRenderMode.PureReflections)
                                {
                                    samplePixel = PathTracerColor.FromNormal(Vector3.Reflect(ray.Direction, nextNormal));
                                    continueBouncing = false;
                                }

                                // Random Reflection
                                float yaw = RandomHelper.RandomFloat(-1, 1) * MathHelper.PI * 0.5F;
                                float pitch = RandomHelper.RandomFloat(-1, 1) * MathHelper.PI * 0.5F;
                                float roll = RandomHelper.RandomFloat(-1, 1) * MathHelper.PI * 0.5F;
                                Quaternion q = Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
                                Vector3 randomReflection = Vector3.Transform(nextNormal, q);
                                randomReflection = Vector3.Normalize(randomReflection);

                                // Random Reflections Render Mode
                                if (options.RenderMode == PathTracerRenderMode.RandomReflections)
                                {
                                    samplePixel = PathTracerColor.FromNormal(randomReflection);
                                    continueBouncing = false;
                                }

                                // Actual Reflection
                                Vector3 actualReflection = MathHelper.Lerp(randomReflection, pureReflection, material.Gloss);

                                // Actual Reflections Render Mode
                                if (options.RenderMode == PathTracerRenderMode.ActualReflections)
                                {
                                    samplePixel = PathTracerColor.FromNormal(actualReflection);
                                    continueBouncing = false;
                                }

                                // Continue?
                                if (continueBouncing)
                                {
                                    // Position
                                    ray.Origin = nextPosition + nextNormal * FloatHelper.Epsilon;

                                    // Direction
                                    if (RandomHelper.RandomFloat() < material.Reflectivity)
                                    {
                                        // Reflect
                                        ray.Direction = pureReflection;
                                    }
                                    else
                                    {
                                        // Reflect
                                        ray.Direction = actualReflection;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }

                            // Add Sample
                            pixel = PathTracerColor.Add(ref pixel, ref samplePixel);
                        }

                        // Average Samples
                        float floatSamplesPerPixel = (float) samplesPerPixel;
                        pixel = PathTracerColor.Divide(ref pixel, ref floatSamplesPerPixel);

                        // Fog
                        float fogDistance = scene.FogDistance;
                        if (fogDistance > 0)
                        {
                            PathTracerColor fogColor = scene.FogColor;
                            float fogAmount = depth / fogDistance;
                            fogAmount = Math.Min(fogAmount, 1);
                            pixel.R = MathHelper.Lerp(pixel.R, fogColor.R, fogAmount);
                            pixel.G = MathHelper.Lerp(pixel.G, fogColor.G, fogAmount);
                            pixel.B = MathHelper.Lerp(pixel.B, fogColor.B, fogAmount);
                        }

                        // Shader
                        int shaderCount = options.Shaders.Count;
                        for (int shaderIndex = 0; shaderIndex < shaderCount; shaderIndex++)
                        {
                            pixel = options.Shaders[shaderIndex](scene, x, y, depth, pixel);
                        }

                        // Set Pixel
                        frameRecorder.SetPixel(x, y, pixel);
                    }

                    // Increment
                    int incrementedValue = Interlocked.Increment(ref pixelsProcessed);

                    // Percent
                    float percent = incrementedValue / (float) pixelCount;
                    options.PercentageDisplay?.Invoke(pixelIndex, percent, stopwatch.Elapsed);
                });

                // Percent
                options.PercentageDisplay?.Invoke(pixelCount, 1, stopwatch.Elapsed);

                // Complete
                frameRecorder.Complete();

                // Frame Callback
                scene.Animator?.Invoke(frame);
            }
        }

        #endregion
    }
}