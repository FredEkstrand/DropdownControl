namespace DropdownControlTester
{
    partial class Form1
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
            this.tbxView = new System.Windows.Forms.TextBox();
            this.dropdownControl2 = new Ekstrand.Windows.Forms.DropdownControl();
            this.dropdownControl1 = new Ekstrand.Windows.Forms.DropdownControl();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbxView
            // 
            this.tbxView.Location = new System.Drawing.Point(271, 13);
            this.tbxView.Name = "tbxView";
            this.tbxView.Size = new System.Drawing.Size(167, 20);
            this.tbxView.TabIndex = 5;
            // 
            // dropdownControl2
            // 
            this.dropdownControl2.ButtonSide = Ekstrand.Windows.Forms.DropdownButtonSide.Right;
            this.dropdownControl2.ClientControl = null;
            this.dropdownControl2.Disable = false;
            this.dropdownControl2.Location = new System.Drawing.Point(144, 12);
            this.dropdownControl2.Name = "dropdownControl2";
            this.dropdownControl2.ShowDropShadow = true;
            this.dropdownControl2.Size = new System.Drawing.Size(121, 21);
            this.dropdownControl2.TabIndex = 3;
            this.dropdownControl2.Text = "dropdownControl2";
            // 
            // dropdownControl1
            // 
            this.dropdownControl1.ButtonSide = Ekstrand.Windows.Forms.DropdownButtonSide.Right;
            this.dropdownControl1.ClientControl = null;
            this.dropdownControl1.Disable = false;
            this.dropdownControl1.Location = new System.Drawing.Point(12, 12);
            this.dropdownControl1.Name = "dropdownControl1";
            this.dropdownControl1.ShowDropShadow = true;
            this.dropdownControl1.Size = new System.Drawing.Size(126, 21);
            this.dropdownControl1.TabIndex = 0;
            this.dropdownControl1.Text = "dropdownControl1";
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(170, 115);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 150);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.tbxView);
            this.Controls.Add(this.dropdownControl2);
            this.Controls.Add(this.dropdownControl1);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Test Child Form";
            this.dropdownControl1.ResumeLayout(false);
            this.dropdownControl2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Ekstrand.Windows.Forms.DropdownControl dropdownControl1;
        private Ekstrand.Windows.Forms.DropdownControl dropdownControl2;
        private System.Windows.Forms.TextBox tbxView;
        private System.Windows.Forms.Button btnExit;
    }
}

