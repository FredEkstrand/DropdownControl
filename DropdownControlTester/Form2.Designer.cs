namespace DropdownControlTester
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.groupBox1 = new Ekstrand.Windows.Forms.GroupBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new Ekstrand.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BorderElements.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.BorderElements.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.groupBox1.BorderElements.BorderCorners = ((Ekstrand.Windows.Forms.BorderCorners)((((Ekstrand.Windows.Forms.BorderCorners.TopLeft | Ekstrand.Windows.Forms.BorderCorners.TopRight) 
            | Ekstrand.Windows.Forms.BorderCorners.BottomLeft) 
            | Ekstrand.Windows.Forms.BorderCorners.BottomRight)));
            this.groupBox1.BorderElements.DashPattern = null;
            this.groupBox1.BorderElements.Radius = 5;
            this.groupBox1.Controls.Add(this.btnExit);
            this.groupBox1.Controls.Add(this.btnOpen);
            this.groupBox1.DisabledTextColor = System.Drawing.SystemColors.GrayText;
            this.groupBox1.HeaderElements.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.HeaderElements.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.groupBox1.HeaderElements.BorderCorners = Ekstrand.Windows.Forms.BorderCorners.None;
            this.groupBox1.HeaderElements.GradientEndColor = System.Drawing.Color.Empty;
            this.groupBox1.HeaderElements.GradientStartColor = System.Drawing.Color.Empty;
            this.groupBox1.HeaderElements.TextAlignment = Ekstrand.Windows.Forms.BorderTextAlignment.TopCenter;
            this.groupBox1.InsideBorderElements.GradientEndColor = System.Drawing.Color.Empty;
            this.groupBox1.InsideBorderElements.GradientStartColor = System.Drawing.Color.Empty;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(172, 63);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Actions";
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(8, 23);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(89, 23);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = " lable1";
            // 
            // groupBox2
            // 
            this.groupBox2.BorderElements.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.BorderElements.BorderColor = System.Drawing.SystemColors.Highlight;
            this.groupBox2.BorderElements.BorderCorners = Ekstrand.Windows.Forms.BorderCorners.None;
            this.groupBox2.BorderElements.DashPattern = null;
            this.groupBox2.BorderElements.Width = 2;
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.DisabledTextColor = System.Drawing.SystemColors.GrayText;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.GroupBoxStyle = Ekstrand.Windows.Forms.GroupBoxStyle.Enhance;
            this.groupBox2.HeaderElements.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.HeaderElements.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.groupBox2.HeaderElements.BorderCorners = Ekstrand.Windows.Forms.BorderCorners.None;
            this.groupBox2.HeaderElements.GradientEndColor = System.Drawing.Color.Empty;
            this.groupBox2.HeaderElements.GradientStartColor = System.Drawing.Color.Empty;
            this.groupBox2.HeaderElements.Width = 0;
            this.groupBox2.InsideBorderElements.GradientEndColor = System.Drawing.Color.Empty;
            this.groupBox2.InsideBorderElements.GradientStartColor = System.Drawing.Color.Empty;
            this.groupBox2.Location = new System.Drawing.Point(20, 329);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(588, 109);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Notes:";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form2";
            this.Text = "Form2";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Ekstrand.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label label1;
        private Ekstrand.Windows.Forms.GroupBox groupBox2;
    }
}