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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tbxView = new System.Windows.Forms.TextBox();
            this.dropdownControl2 = new Ekstrand.Windows.Forms.DropdownControl();
            this.dropdownControl1 = new Ekstrand.Windows.Forms.DropdownControl();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(71, 174);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(474, 81);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Off Screen";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // tbxView
            // 
            this.tbxView.Location = new System.Drawing.Point(474, 45);
            this.tbxView.Name = "tbxView";
            this.tbxView.Size = new System.Drawing.Size(167, 20);
            this.tbxView.TabIndex = 5;
            // 
            // dropdownControl2
            // 
            this.dropdownControl2.ButtonSide = Ekstrand.Windows.Forms.DropDownButtonSide.Right;
            this.dropdownControl2.ClientControl = null;
            this.dropdownControl2.Disable = false;
            this.dropdownControl2.Location = new System.Drawing.Point(270, 44);
            this.dropdownControl2.Name = "dropdownControl2";
            this.dropdownControl2.Size = new System.Drawing.Size(121, 21);
            this.dropdownControl2.TabIndex = 3;
            this.dropdownControl2.Text = "dropdownControl2";
            // 
            // dropdownControl1
            // 
            this.dropdownControl1.ButtonSide = Ekstrand.Windows.Forms.DropDownButtonSide.Right;
            this.dropdownControl1.ClientControl = null;
            this.dropdownControl1.Disable = false;
            this.dropdownControl1.Location = new System.Drawing.Point(45, 44);
            this.dropdownControl1.Name = "dropdownControl1";
            this.dropdownControl1.Size = new System.Drawing.Size(126, 21);
            this.dropdownControl1.TabIndex = 0;
            this.dropdownControl1.Text = "dropdownControl1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tbxView);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dropdownControl2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dropdownControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Ekstrand.Windows.Forms.DropdownControl dropdownControl1;
        private System.Windows.Forms.Button button1;
        private Ekstrand.Windows.Forms.DropdownControl dropdownControl2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox tbxView;
    }
}

