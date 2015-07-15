using Gnu.MP;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Mandelbrot.GMPRenderer
{
    public class GMPRenderer : MandelbrotRendererBase
    {
        private Real xorigin, yorigin, xextent;
        private Real xmin, xmax, ymin, ymax;

        public GMPRenderer(Form parentContext, UInt32[] colorPalette, int colorPaletteSize)
            : base(parentContext, colorPalette, colorPaletteSize)
        {
        }

        public override string ToString()
        {
            return "GnuMP arbitrary precision";
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
            Real x, y, x0, y0, temp = new Real();
            Real xscale = (xmax - xmin) / screenWidth;
            Real yscale = (ymax - ymin) / screenHeight;
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
                    x = new Real(x0);
                    y = new Real(y0);

                    while ((iteration < numIterations) && (x.DoubleValue * x.DoubleValue + y.DoubleValue * y.DoubleValue <= 4))
                    {
                        Real.MandelbrotOperations(ref x, ref y, ref x0, ref y0, ref temp);
                        iteration++;
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


        public override void Move(int moveX, int moveY)
        {
            //BigDecimal xpos = xmin + ((BigDecimal)e.X * (xmax - xmin) / (BigDecimal)screenWidth);
            //BigDecimal ypos = ymin + ((BigDecimal)e.Y * (ymax - ymin) / (BigDecimal)screenHeight);
            //controlForm.txtInfo.Text = xpos.ToString() + ", " + ypos.ToString();
            TerminateThreads();
            xorigin -= (Real)(moveX) * (xmax - xmin) / (Real)screenWidth;
            yorigin -= (Real)(moveY) * (ymax - ymin) / (Real)screenHeight;
            Draw(numIterations, numThreads);
        }

        public override void Zoom(int posX, int posY, double factor)
        {
            Real xpos = xmin + ((Real)posX * (xmax - xmin) / (Real)screenWidth);
            Real ypos = ymin + ((Real)posY * (ymax - ymin) / (Real)screenHeight);
            Real xOffsetRatio = (xpos - xmin) / (xmax - xmin);
            Real yOffsetRatio = (ypos - ymin) / (ymax - ymin);

            Real newXextent = (xmax - xmin);
            newXextent *= factor;

            Real newYextent = (ymax - ymin);
            newYextent *= factor;

            TerminateThreads();
            xextent = newXextent;
            xorigin = xpos - xextent * xOffsetRatio;
            yorigin = ypos - newYextent * yOffsetRatio;
            Draw(numIterations, numThreads);
        }

    }

}
