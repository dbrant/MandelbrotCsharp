using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Mandelbrot
{
    public abstract class MandelbrotRendererBase
    {
        protected Form parentContext;
        protected UInt32[] bitmapBits;
        protected UInt32[] colorPalette;
        protected int colorPaletteSize;

        protected int numIterations;
        protected int screenWidth, screenHeight;

        protected List<Thread> currentThreads;
        protected int numThreads;
        protected bool terminateThreads = false;


        public MandelbrotRendererBase(Form parentContext, UInt32[] colorPalette, int colorPaletteSize)
        {
            this.parentContext = parentContext;
            this.colorPalette = colorPalette;
            this.colorPaletteSize = colorPaletteSize;
            currentThreads = new List<Thread>();
        }

        public abstract void SetInitialParams(double xorigin, double yorigin, double xextent);

        public void UpdateBitmapBits(UInt32[] bitmapBits)
        {
            this.bitmapBits = bitmapBits;
        }

        public void UpdateScreenDimensions(int width, int height)
        {
            screenWidth = width;
            screenHeight = height;
        }

        public void Draw(int numIterations, int numThreads)
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

        public void TerminateThreads()
        {
            terminateThreads = true;
            foreach (var t in currentThreads) { t.Join(); }
            currentThreads.Clear();
            terminateThreads = false;
        }

        protected struct MandelThreadParams
        {
            public Form parentForm;
            public int startX, startY, startWidth, startHeight;
        }

        protected abstract void DrawInternal(object threadParams);

        public abstract void Move(int moveX, int moveY);

        public abstract void Zoom(int posX, int posY, double factor);

        public abstract string GetCoordinateStr(int mouseX, int mouseY);

    }
}
