using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DropdownControlTester
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            SetlableText();
        }

        private void SetlableText()
        {
            label1.Text = "1. When the dropdown control is open there is no task-bar entry.\n" +
                          "2. UI cues haven't changed for Test Child Form when the pop-up window is open.\n" +
                          "3. The pop-up window closes when the user click out side any where.\n" +
                          "4. The pop-up window closes when the user drags the window.";
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private Form1 _form = null;
        private void BtnOpen_Click(object sender, EventArgs e)
        {
            if (_form == null)
            {
                _form = new Form1();
                _form.FormClosed += _form_FormClosed;
                _form.Location = new Point(this.Location.X+60,this.Location.Y+118);
                _form.Show(this);               
            }
        }

        private void _form_FormClosed(object sender, FormClosedEventArgs e)
        {
            _form.FormClosed -= _form_FormClosed;
            _form.Dispose();
            _form = null;
        }
    }
}
