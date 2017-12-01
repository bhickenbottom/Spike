# Spike
## .NET Core Path Tracer

Spike is a C# .NET Core 2.0 path tracer with multithreading and SIMD, including a bit of code to create abstract art.

The sample image below is 4K with a bounce count of 8 and a sample count per pixel of 1024. It took approixmately 4 hours to render on my machine.

![Light Box](https://github.com/bhickenbottom/spike/raw/master/PathTracer/Samples/LightBox.png)

This is a simple prototype and isn't physically accurate, but the code is clean and it's fun to play around with.

## Getting Started

Spike is a simple console app, but it should be relatively easy to refactor into a separate assembly.

It its default state, the console app will generate 10 4K abstract art images and save them to the `Png` directory.

The images only use 8 samples per pixel and take appoximately 5 minutes a piece on my machine.

![Abstract](https://github.com/bhickenbottom/spike/raw/master/PathTracer/Samples/Abstract1.png)

![Abstract](https://github.com/bhickenbottom/spike/raw/master/PathTracer/Samples/Abstract2.png)

![Abstract](https://github.com/bhickenbottom/spike/raw/master/PathTracer/Samples/Abstract3.png)

## Rendering your own scene

The first step is to create an instance of `PathTracerScene`.

You'll need to assign a `PathTracerCamera` to your scene and the `PathTracerTriangles` you want to render. You'll also need to assign `PathTracerMaterial` instances to your triangles and the `BackgroundMaterial` property of your scene.

A material has a `Color` property and an `IsLight` property. Materials also include properties for `Gloss` and `Reflectivity`. Colors with alpha values are supported.

After you create your scene you'll want to set some rendering options. These options include `Width`, `Height`, `BounceCount`, `PixelSampleRate`, `SamplesPerPixel`, `MaxDegreeOfParallelism`, and `FrameCount` (for animations). You can also include a delegate for displaying progress. `ConsolePercentageDisplay` will write this information to the console.

Next, you'll want to create a frame recorder. Simply create an instance of `PngPathTracerFrameRecorder` or create your own.

Finally create an instance of `PathTracerEngine` and call `Render`, passing in your scene, options, and frame recorder.

## Render Modes

The `PathTracerOptions` class has a `RenderMode` that accepts a `PathTracerRenderMode` enum. These are great for debugging purposes. Sample images are listed below.

### PathTracer

![PathTracer](https://github.com/bhickenbottom/spike/raw/master/PathTracer/Samples/RenderModePathTracer.png)

### Color

![Color](https://github.com/bhickenbottom/spike/raw/master/PathTracer/Samples/RenderModeColor.png)

### Depth

![Depth](https://github.com/bhickenbottom/spike/raw/master/PathTracer/Samples/RenderModeDepth.png)

### Normals

![Normals](https://github.com/bhickenbottom/spike/raw/master/PathTracer/Samples/RenderModeNormals.png)

### PureReflections

![PureReflections](https://github.com/bhickenbottom/spike/raw/master/PathTracer/Samples/RenderModePureReflections.png)

### RandomReflections

![RandomReflections](https://github.com/bhickenbottom/spike/raw/master/PathTracer/Samples/RenderModeRandomReflections.png)

### ActualReflections

![ActualReflections](https://github.com/bhickenbottom/spike/raw/master/PathTracer/Samples/RenderModeActualReflections.png)

### AmbientOcclusion

![AmbientOcclusion](https://github.com/bhickenbottom/spike/raw/master/PathTracer/Samples/RenderModeAmbientOcclusion.png)

### DistanceTraveled

![DistanceTraveled](https://github.com/bhickenbottom/spike/raw/master/PathTracer/Samples/RenderModeDistanceTraveled.png)

## Tips and tricks

* Use a `PathTracerScene.BackgroundMaterial` with `PathTracerMaterial.IsLight` set to true for ambient light.
* Use the `PathTracerScene.Animation` delegate to animate your scene between frames.
* Use `PathTracerSceneGenerator` to generate abstract art or the test scene shown above.
* Use the `PathTracerOptions.Shaders` delegate list to apply per-pixel effects to your rendered image.
