using System;
using System.Drawing;

namespace Mandelbrot
{
    public class ColorScheme
    {

        public static ulong[] CreateColorScheme(ulong[] colorArray, int numElements)
        {
            int elementsPerStep = numElements / (colorArray.Length - 1);
            ulong[] colors = new ulong[numElements];

            float r = 0f, g = 0f, b = 0f;
            float rInc = 0f, gInc = 0f, bInc = 0f;
            int cIndex = 0;
            int cCounter = 0;

            for (int i = 0; i < numElements; i++)
            {
                if (cCounter == 0)
                {
                    b = colorArray[cIndex] & 0xffff;
                    g = (colorArray[cIndex] & 0xffff0000) >> 16;
                    r = (colorArray[cIndex] & 0xffff00000000) >> 32;
                    if (cIndex < colorArray.Length - 1)
                    {
                        bInc = ((float)(colorArray[cIndex + 1] & 0xffff) - b) / (float)elementsPerStep;
                        gInc = ((float)((colorArray[cIndex + 1] & 0xffff0000) >> 16) - g) / (float)elementsPerStep;
                        rInc = ((float)((colorArray[cIndex + 1] & 0xffff00000000) >> 32) - r) / (float)elementsPerStep;
                    }
                    cIndex++;
                    cCounter = elementsPerStep;
                }
                colors[i] = (ulong)0x1fff000000000000 | (((ulong)b & 0x1fff) << 32) | (((ulong)g & 0x1fff) << 16) | (((ulong)r & 0x1fff));
                b = b + bInc;
                g = g + gInc;
                r = r + rInc;
                if (b < 0f) { b = 0f; }
                if (g < 0f) { g = 0f; }
                if (r < 0f) { r = 0f; }
                if (b > 8191f) { b = 8191f; }
                if (g > 8191f) { g = 8191f; }
                if (r > 8191f) { r = 8191f; }
                cCounter--;
            }
            return colors;
        }

        public static ulong[] CreateColorScheme(Color[] colorArray, int numElements)
        {
            ulong[] colors = new ulong[colorArray.Length];
            ulong r, g, b;
            for (int i = 0; i < colorArray.Length; i++)
            {
                uint color = (uint)colorArray[i].ToArgb();
                b = (color & 0xff) * 8191 / 256;
                g = ((color & 0xff00) >> 8) * 8191 / 256;
                r = ((color & 0xff0000) >> 16) * 8191 / 256;
                colors[i] = 0x1fff000000000000 | ((b & 0x1fff) << 32) | ((g & 0x1fff) << 16) | ((r & 0x1fff));
            }
            return CreateColorScheme(colors, numElements);
        }

    }
}
