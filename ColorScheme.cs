using System;
using System.Drawing;

namespace Mandelbrot
{
    public class ColorScheme
    {

        public static UInt32[] CreateColorScheme(UInt32[] colorArray, int numElements)
        {
            int elementsPerStep = numElements / (colorArray.Length - 1);
            UInt32[] colors = new UInt32[numElements];

            float r = 0f, g = 0f, b = 0f;
            float rInc = 0f, gInc = 0f, bInc = 0f;
            int cIndex = 0;
            int cCounter = 0;

            for (int i = 0; i < numElements; i++)
            {
                if (cCounter == 0)
                {
                    b = colorArray[cIndex] & 0xff;
                    g = (colorArray[cIndex] & 0xff00) >> 8;
                    r = (colorArray[cIndex] & 0xff0000) >> 16;
                    if (cIndex < colorArray.Length - 1)
                    {
                        bInc = ((float)(colorArray[cIndex + 1] & 0xff) - b) / (float)elementsPerStep;
                        gInc = ((float)((colorArray[cIndex + 1] & 0xff00) >> 8) - g) / (float)elementsPerStep;
                        rInc = ((float)((colorArray[cIndex + 1] & 0xff0000) >> 16) - r) / (float)elementsPerStep;
                    }
                    cIndex++;
                    cCounter = elementsPerStep;
                }
                colors[i] = (UInt32)0xff000000 | ((UInt32)b << 16) | ((UInt32)g << 8) | ((UInt32)r);
                b = b + bInc;
                g = g + gInc;
                r = r + rInc;
                if (b < 0f) { b = 0f; }
                if (g < 0f) { g = 0f; }
                if (r < 0f) { r = 0f; }
                if (b > 255f) { b = 255f; }
                if (g > 255f) { g = 255f; }
                if (r > 255f) { r = 255f; }
                cCounter--;
            }
            return colors;
        }

        public static UInt32[] CreateColorScheme(Color[] colorArray, int numElements)
        {
            UInt32[] colors = new UInt32[colorArray.Length];
            UInt32 r, g, b;
            for (int i = 0; i < colorArray.Length; i++)
            {
                colors[i] = (UInt32)colorArray[i].ToArgb();
                b = colors[i] & 0xff;
                g = (colors[i] & 0xff00) >> 8;
                r = (colors[i] & 0xff0000) >> 16;
                colors[i] = 0xff000000 | (b << 16) | (g << 8) | r;
            }
            return CreateColorScheme(colors, numElements);
        }

    }
}
