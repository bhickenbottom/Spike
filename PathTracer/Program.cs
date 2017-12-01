using System;
using System.Numerics;
using PathTracer.FrameRecorders;

namespace PathTracer
{
    internal class Program
    {
        #region Static Methods

        private static void Main(string[] args)
        {
            if (Vector.IsHardwareAccelerated)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Hardware Accelerated");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Not Hardware Accelerated");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            IPathTracerFrameRecorder frameRecorder = new PngPathTracerFrameRecorder();
            for (int i = 0; i < 10; i++)
            {
                // Scene
                PathTracerScene scene = new PathTracerScene();
                scene.Camera = new PathTracerCamera();
                scene.Camera.Position = new Vector3(0, 0, 100);
                scene.Camera.Direction = new Vector3(0, 0, -100);
                scene.Camera.Up = new Vector3(0, 1, 0);
                scene.Camera.AntiAliased = true;
                scene.BackgroundMaterial = new PathTracerMaterial();
                scene.BackgroundMaterial.IsLight = false;
                scene.BackgroundMaterial.Color = new PathTracerColor(1, 4, 4, 4);
                scene.FogColor = PathTracerColor.Black;
                scene.FogDistance = 0;

                // Scene Generation (Light Box)
                //PathTracerSceneGenerator.GenerateLightBox(scene, new Vector3(-200, -200, -200), new Vector3(200, 200, 200));

                // Scene Generation (Abstract)
                PathTracerSceneGenerator.GenerateAbstract(scene, new Vector3(-100, -100, -100), new Vector3(100, 100, 100), 40, 0.1F, 0.3333F);

                // Options
                ConsolePercentageDisplay consolePercentageDisplay = new ConsolePercentageDisplay();
                PathTracerOptions options = new PathTracerOptions();
                options.Width = 3840;
                options.Height = 2160;
                options.BounceCount = 8;
                options.SamplesPerPixel = 8;
                options.FrameCount = 1;
                options.MaxDegreeOfParallelism = 2;
                options.PercentageDisplay = consolePercentageDisplay.Display;
                options.RenderMode = PathTracerRenderMode.PathTracer;
                options.PixelSampleRate = 1F;

                // Engine
                PathTracerEngine engine = new PathTracerEngine();

                // Render
                engine.Render(scene, options, frameRecorder);
            }

            // Done
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        #endregion
    }
}