using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mandelbrot
{
    public partial class Form1 : Form
    {
        private const int WHEEL_DELTA = 120;
        private const double ZOOM_FACTOR_IN = 0.8;
        private const double ZOOM_FACTOR_OUT = 1.2;

        private ControlForm controlForm;

        public List<MandelbrotRendererBase> rendererList { get; private set; }
        private MandelbrotRendererBase currentRenderer;

        private UInt32[] bmpBits;
        private GCHandle gcHandle;
        private Bitmap bmp = null;

        private bool moving = false;
        private int moveX0, moveY0;

        public Form1()
        {
            InitializeComponent();
            Text = Application.ProductName;
            bmpBits = new UInt32[0];

            int colorPaletteSize = 1024;
            UInt32[] colorPalette = ColorScheme.CreateColorScheme(new UInt32[] { 0xffff0000, 0xff00ff00, 0xff0000ff, 0xffff00ff, 0xffff0000 }, colorPaletteSize);


            rendererList = new List<MandelbrotRendererBase>();
            rendererList.Add(new FloatRenderer.FloatRenderer(this, colorPalette, colorPaletteSize));
            rendererList.Add(new GMPRenderer.GMPRenderer(this, colorPalette, colorPaletteSize));
            rendererList.Add(new BigIntegerRenderer.BigIntegerRenderer(this, colorPalette, colorPaletteSize));
            rendererList.Add(new SimpleBigIntRenderer.SimpleBigIntRenderer(this, colorPalette, colorPaletteSize));
            currentRenderer = rendererList[0];

            foreach (var r in rendererList)
            {
                ResetInitialParams(r);
            }


            this.DoubleBuffered = true;
            this.MouseWheel += Form1_MouseWheel;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            controlForm = new ControlForm(this);
            controlForm.Show();
            Form1_SizeChanged(null, null);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            currentRenderer.TerminateThreads();
            FreeStuff();
            controlForm.Close();
        }

        private void FreeStuff()
        {
            if (bmp != null) { bmp.Dispose(); bmp = null; }
            if (gcHandle.IsAllocated) { gcHandle.Free(); }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            currentRenderer.TerminateThreads();

            int screenWidth = this.ClientSize.Width;
            int screenHeight = this.ClientSize.Height;
            if (screenWidth < 1) { screenWidth = 1; }
            if (screenHeight < 1) { screenHeight = 1; }

            int bufferLength = screenWidth * screenHeight;
            if (bufferLength > bmpBits.Length)
            {
                FreeStuff();
                bmpBits = new UInt32[bufferLength * 4];
                gcHandle = GCHandle.Alloc(bmpBits, GCHandleType.Pinned);
            }

            if (bmp != null) { bmp.Dispose(); }
            bmp = new Bitmap(screenWidth, screenHeight, screenWidth * 4, PixelFormat.Format32bppArgb, gcHandle.AddrOfPinnedObject());

            currentRenderer.UpdateBitmapBits(bmpBits);
            currentRenderer.UpdateScreenDimensions(screenWidth, screenHeight);
            OnParametersChanged();
        }

        public void SetRenderer(int index)
        {
            currentRenderer.TerminateThreads();
            currentRenderer = rendererList[index];
            Form1_SizeChanged(null, null);
        }

        private void ResetInitialParams(MandelbrotRendererBase renderer)
        {
            renderer.SetInitialParams(-2.0, -1.2, 3.0);
        }

        public void ResetInitialParams()
        {
            ResetInitialParams(currentRenderer);
        }

        public void OnParametersChanged()
        {
            currentRenderer.TerminateThreads();
            currentRenderer.Draw((int)controlForm.udIterations.Value, (int)controlForm.udNumThreads.Value);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (bmp == null) { return; }
            e.Graphics.DrawImageUnscaled(bmp, 0, 0);
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            moving = true;
            moveX0 = e.X;
            moveY0 = e.Y;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            controlForm.txtInfo.Text = currentRenderer.GetCoordinateStr(e.X, e.Y);
            if (!moving) { return; }
            currentRenderer.Move(e.X - moveX0, e.Y - moveY0);
            moveX0 = e.X;
            moveY0 = e.Y;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            moving = false;
        }

        void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            double factor = 1.0;
            if (e.Delta > 0) { factor = Math.Pow(ZOOM_FACTOR_IN, (double)e.Delta / (double)WHEEL_DELTA); }
            else if (e.Delta < 0) { factor = Math.Pow(ZOOM_FACTOR_OUT, (double)-e.Delta / (double)WHEEL_DELTA); }
            currentRenderer.Zoom(e.X, e.Y, factor);
        }

    }
}
