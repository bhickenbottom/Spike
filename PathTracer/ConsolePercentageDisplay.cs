using System;
using System.Text;

namespace PathTracer
{
    public class ConsolePercentageDisplay
    {
        #region Constructors

        public ConsolePercentageDisplay()
        {
            this.lastBars = -1;
        }

        #endregion

        #region Fields

        private int lastBars;

        #endregion

        #region Methods

        public void Display(int pixelCount, float percent, TimeSpan elapsed)
        {
            if (percent >= 1)
            {
                lastBars = -1;
            }
            int bars = (int) Math.Floor(percent * 100);
            if (bars > lastBars)
            {
                lastBars = bars;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("[");
                for (int barIndex = 0; barIndex < 100; barIndex++)
                {
                    if (barIndex > bars)
                    {
                        stringBuilder.Append(" ");
                    }
                    else
                    {
                        stringBuilder.Append("=");
                    }
                }
                stringBuilder.Append("]");
                if (percent > 0.01F)
                {
                    stringBuilder.Append(" ");
                    double remaining = 1 / percent * elapsed.TotalMinutes - elapsed.TotalMinutes;
                    int remainingInt = (int) Math.Ceiling(remaining);
                    stringBuilder.AppendFormat("{0} minutes remaining", remainingInt);
                }
                Console.WriteLine(stringBuilder);
            }
        }

        #endregion
    }
}