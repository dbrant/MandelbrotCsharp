﻿using System;
using System.Windows.Forms;

namespace Mandelbrot.BigIntegerRenderer
{
    public class BigIntegerRenderer : MandelbrotRendererBase
    {
        private BigDecimal xorigin, yorigin, xextent;
        private BigDecimal xmin, xmax, ymin, ymax;

        public BigIntegerRenderer(Form parentContext, ulong[] colorPalette, int colorPaletteSize)
            : base(parentContext, colorPalette, colorPaletteSize)
        {
        }

        public override string ToString()
        {
            return "System.Numerics.BigInteger";
        }

        public override void SetInitialParams(double xorigin, double yorigin, double xextent) {
            this.xorigin = xorigin;
            this.yorigin = yorigin;
            this.xextent = xextent;
        }

        protected override void DrawInternal(object threadParams)
        {
            var tParams = (MandelThreadParams)threadParams;
            double ratio = (double)screenWidth / (double)screenHeight;
            xmin = xorigin;
            ymin = yorigin;
            xmax = xmin + xextent;
            ymax = ymin + xextent / ratio;

            int maxY = tParams.startY + tParams.startHeight;
            int maxX = tParams.startX + tParams.startWidth;
            BigDecimal x, y, x0, y0;
            BigDecimal xsq, ysq;
            BigDecimal xscale = (xmax - xmin) / screenWidth;
            BigDecimal yscale = (ymax - ymin) / screenHeight;
            int iteration;
            int iterScale = 1;
            int px, py;
            BigDecimal bailout = 4.0;

            BigDecimal[] x0row = new BigDecimal[maxX - tParams.startX];
            for (int i = 0; i < x0row.Length; i++)
            {
                x0row[i] = xscale;
                x0row[i].Multiply(i + tParams.startX);
                x0row[i].Add(xmin);
            }

            x = 0; y = 0;
            xsq = 0; ysq = 0;

            if (numIterations < colorPaletteSize) { iterScale = colorPaletteSize / numIterations; }

            for (py = tParams.startY; py < maxY; py++)
            {
                y0 = yscale;
                y0.Multiply(py);
                y0.Add(ymin);
                y0.Truncate();

                for (px = tParams.startX; px < maxX; px++)
                {
                    x0 = x0row[px - tParams.startX];

                    x.Zero();
                    y.Zero();
                    xsq.Zero();
                    ysq.Zero();
                    iteration = 0;

                    while (xsq + ysq < bailout)
                    {
                        y.Multiply(x);
                        y.Truncate();
                        y.Add(y);
                        y.Add(y0);

                        x = xsq;
                        x.Add(-ysq);
                        x.Add(x0);
                        x.Truncate();

                        xsq = x;
                        xsq.Multiply(x);

                        ysq = y;
                        ysq.Multiply(y);

                        if (iteration++ > numIterations) { break; }
                    }

                    if (iteration >= numIterations)
                    {
                        bitmapBits[py * screenWidth + px] = 0x1fff000000000000;
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
            BigDecimal xpos = xmin + ((BigDecimal)mouseX * (xmax - xmin) / (BigDecimal)screenWidth);
            BigDecimal ypos = ymin + ((BigDecimal)mouseY * (ymax - ymin) / (BigDecimal)screenHeight);
            return xpos.ToString() + ", " + ypos.ToString();
        }

        public override void Move(int moveX, int moveY)
        {
            TerminateThreads();
            xorigin -= (BigDecimal)(moveX) * (xmax - xmin) / (BigDecimal)screenWidth;
            yorigin -= (BigDecimal)(moveY) * (ymax - ymin) / (BigDecimal)screenHeight;
            Draw(numIterations, numThreads);
        }

        public override void Zoom(int posX, int posY, double factor)
        {
            BigDecimal xpos = xmin + ((BigDecimal)posX * (xmax - xmin) / (BigDecimal)screenWidth);
            BigDecimal ypos = ymin + ((BigDecimal)posY * (ymax - ymin) / (BigDecimal)screenHeight);
            BigDecimal xOffsetRatio = (xpos - xmin) / (xmax - xmin);
            BigDecimal yOffsetRatio = (ypos - ymin) / (ymax - ymin);

            BigDecimal newXextent = (xmax - xmin);
            newXextent *= factor;

            BigDecimal newYextent = (ymax - ymin);
            newYextent *= factor;

            TerminateThreads();
            xextent = newXextent;
            xorigin = xpos - xextent * xOffsetRatio;
            yorigin = ypos - newYextent * yOffsetRatio;
            Draw(numIterations, numThreads);
        }

    }

}
