namespace Mandelbrot
{
    partial class ControlForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlForm));
            this.udIterations = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.udNumThreads = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.cbRenderer = new System.Windows.Forms.ComboBox();
            this.btnReset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.udIterations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udNumThreads)).BeginInit();
            this.SuspendLayout();
            // 
            // udIterations
            // 
            this.udIterations.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            resources.ApplyResources(this.udIterations, "udIterations");
            this.udIterations.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.udIterations.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.udIterations.Name = "udIterations";
            this.udIterations.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.udIterations.ValueChanged += new System.EventHandler(this.udIterations_ValueChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtInfo
            // 
            resources.ApplyResources(this.txtInfo, "txtInfo");
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // udNumThreads
            // 
            resources.ApplyResources(this.udNumThreads, "udNumThreads");
            this.udNumThreads.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.udNumThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udNumThreads.Name = "udNumThreads";
            this.udNumThreads.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.udNumThreads.ValueChanged += new System.EventHandler(this.udIterations_ValueChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cbRenderer
            // 
            resources.ApplyResources(this.cbRenderer, "cbRenderer");
            this.cbRenderer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRenderer.FormattingEnabled = true;
            this.cbRenderer.Name = "cbRenderer";
            // 
            // btnReset
            // 
            resources.ApplyResources(this.btnReset, "btnReset");
            this.btnReset.Name = "btnReset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // ControlForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.cbRenderer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.udNumThreads);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.udIterations);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ControlForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            ((System.ComponentModel.ISupportInitialize)(this.udIterations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udNumThreads)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.NumericUpDown udIterations;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.NumericUpDown udNumThreads;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbRenderer;
        private System.Windows.Forms.Button btnReset;
    }
}