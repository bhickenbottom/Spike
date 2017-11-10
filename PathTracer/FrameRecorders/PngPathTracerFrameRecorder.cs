using System.IO;
using ImageSharp;

namespace PathTracer.FrameRecorders
{
    public class PngPathTracerFrameRecorder : IPathTracerFrameRecorder
    {
        #region Constructors

        public PngPathTracerFrameRecorder()
        {
            this.FrameNumber = -1;
        }

        #endregion

        #region Properties

        public Image Frame { get; private set; }

        public int FrameNumber { get; private set; }

        #endregion

        #region Methods

        public void AddFrame(int width, int height)
        {
            this.FrameNumber++;
            this.Frame = new Image(width, height);
        }

        public void Complete()
        {
            if (this.Frame == null)
            {
                return;
            }
            if (!Directory.Exists("Png"))
            {
                Directory.CreateDirectory("Png");
            }
            using (FileStream fileStream = File.Open($"Png/{this.FrameNumber}.png", FileMode.Create))
            {
                this.Frame.SaveAsPng(fileStream);
            }
        }

        public void SetPixel(int x, int y, PathTracerColor color)
        {
            if (this.Frame == null)
            {
                return;
            }
            color.Clamp();
            Color frameColor = new Color(color.R, color.G, color.B, color.A);
            int index = y * this.Frame.Width + x;
            this.Frame.Pixels[index] = frameColor;
        }

        #endregion
    }
}