using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mandelbrot
{
    public partial class ControlForm : Form
    {
        public ControlForm(Form1 parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            this.Owner = parentForm;

            foreach (var r in parentForm.rendererList)
            {
                cbRenderer.Items.Add(r);
            }
            cbRenderer.SelectedIndex = 0;
            cbRenderer.SelectedIndexChanged += cbRenderer_SelectedIndexChanged;
        }

        private Form1 parentForm;

        private void udIterations_ValueChanged(object sender, EventArgs e)
        {
            parentForm.OnParametersChanged();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            parentForm.ResetInitialParams();
            parentForm.OnParametersChanged();
        }

        private void cbRenderer_SelectedIndexChanged(object sender, EventArgs e)
        {
            parentForm.SetRenderer(cbRenderer.SelectedIndex);
            parentForm.OnParametersChanged();
        }

    }
}
