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
            this.udIterations.Location = new System.Drawing.Point(12, 25);
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
            this.udIterations.Size = new System.Drawing.Size(102, 20);
            this.udIterations.TabIndex = 1;
            this.udIterations.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.udIterations.ValueChanged += new System.EventHandler(this.udIterations_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Iterations:";
            // 
            // txtInfo
            // 
            this.txtInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInfo.Location = new System.Drawing.Point(12, 164);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInfo.Size = new System.Drawing.Size(174, 124);
            this.txtInfo.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Current coordinates:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Threads:";
            // 
            // udNumThreads
            // 
            this.udNumThreads.Location = new System.Drawing.Point(12, 68);
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
            this.udNumThreads.Size = new System.Drawing.Size(102, 20);
            this.udNumThreads.TabIndex = 3;
            this.udNumThreads.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.udNumThreads.ValueChanged += new System.EventHandler(this.udIterations_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Renderer:";
            // 
            // cbRenderer
            // 
            this.cbRenderer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRenderer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRenderer.FormattingEnabled = true;
            this.cbRenderer.Location = new System.Drawing.Point(12, 111);
            this.cbRenderer.Name = "cbRenderer";
            this.cbRenderer.Size = new System.Drawing.Size(174, 21);
            this.cbRenderer.TabIndex = 9;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(12, 294);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(174, 28);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "&Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // ControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(198, 334);
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
            this.Text = "Controls";
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