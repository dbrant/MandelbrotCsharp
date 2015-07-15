using System;
using System.Threading;
using System.Windows.Forms;

namespace Mandelbrot.FloatRenderer
{
    public class FloatRenderer : MandelbrotRendererBase
    {
        private double xorigin, yorigin, xextent;
        private double xmin, xmax, ymin, ymax;

        public FloatRenderer(Form parentContext, UInt32[] colorPalette, int colorPaletteSize)
            : base(parentContext, colorPalette, colorPaletteSize)
        {
        }

        public override string ToString()
        {
            return "Double-precision";
        }

        public override void SetInitialParams(double xorigin, double yorigin, double xextent)
        {
            this.xorigin = xorigin;
            this.yorigin = yorigin;
            this.xextent = xextent;
        }

        public override void Draw(int numIterations, int numThreads)
        {
            this.numIterations = numIterations;
            this.numThreads = numThreads;

            int yInc = screenHeight / numThreads;
            int yPos = 0;
            for (int i = 0; i < numThreads; i++)
            {
                var thread = new Thread(new ParameterizedThreadStart(DrawInternal));
                currentThreads.Add(thread);
                var p = new MandelThreadParams();
                p.parentForm = parentContext;
                p.startX = 0;
                p.startY = yPos;
                p.startWidth = screenWidth;
                if (i == numThreads - 1)
                {
                    p.startHeight = screenHeight - yPos;
                }
                else
                {
                    p.startHeight = yInc;
                }
                thread.Start(p);
                yPos += yInc;
            }
        }

        private void DrawInternal(object threadParams)
        {
            var tParams = (MandelThreadParams)threadParams;
            double ratio = (double)screenWidth / (double)screenHeight;
            xmin = xorigin;
            ymin = yorigin;
            xmax = xmin + xextent;
            ymax = ymin + xextent / ratio;

            int maxY = tParams.startY + tParams.startHeight;
            int maxX = tParams.startX + tParams.startWidth;
            double x, y, x0, y0;
            double xsq, ysq;
            double xscale = (xmax - xmin) / screenWidth;
            double yscale = (ymax - ymin) / screenHeight;
            int iteration;
            int iterScale = 1;
            int px, py;

            if (numIterations < colorPaletteSize) { iterScale = colorPaletteSize / numIterations; }

            for (py = tParams.startY; py < maxY; py++)
            {
                y0 = ymin + py * yscale;

                for (px = tParams.startX; px < maxX; px++)
                {
                    x0 = xmin + px * xscale;
                    iteration = 0;

                    xsq = ysq = x = y = 0.0f;
                    while (xsq + ysq < 4.0)
                    {
                        y = x * y;
                        y += y;
                        y += y0;
                        x = (xsq - ysq + x0);
                        xsq = x * x;
                        ysq = y * y;

                        if (iteration++ > numIterations) { break; }
                    }

                    if (iteration >= numIterations)
                    {
                        bitmapBits[py * screenWidth + px] = 0xFF000000;
                    }
                    else
                    {
                        bitmapBits[py * screenWidth + px] = colorPalette[(iteration * iterScale) % colorPaletteSize];
                    }
                }

                if (terminateThreads) { break; }
            }

            tParams.parentForm.BeginInvoke(new MethodInvoker(delegate()
            {
                tParams.parentForm.Invalidate();
            }));
        }


        public override string GetCoordinateStr(int mouseX, int mouseY)
        {
            double xpos = xmin + ((double)mouseX * (xmax - xmin) / (double)screenWidth);
            double ypos = ymin + ((double)mouseY * (ymax - ymin) / (double)screenHeight);
            return xpos.ToString() + ", " + ypos.ToString();
        }

        public override void Move(int moveX, int moveY)
        {
            TerminateThreads();
            xorigin -= (double)(moveX) * (xmax - xmin) / (double)screenWidth;
            yorigin -= (double)(moveY) * (ymax - ymin) / (double)screenHeight;
            Draw(numIterations, numThreads);
        }

        public override void Zoom(int posX, int posY, double factor)
        {
            double xpos = xmin + ((double)posX * (xmax - xmin) / (double)screenWidth);
            double ypos = ymin + ((double)posY * (ymax - ymin) / (double)screenHeight);
            double xOffsetRatio = (xpos - xmin) / (xmax - xmin);
            double yOffsetRatio = (ypos - ymin) / (ymax - ymin);

            double newXextent = (xmax - xmin);
            newXextent *= factor;

            double newYextent = (ymax - ymin);
            newYextent *= factor;

            TerminateThreads();
            xextent = newXextent;
            xorigin = xpos - xextent * xOffsetRatio;
            yorigin = ypos - newYextent * yOffsetRatio;
            Draw(numIterations, numThreads);
        }

    }

}
