using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ekstrand.Drawing;
using Ekstrand.Windows.Forms;

namespace DropdownControlTester
{
    public partial class Form1 : Form
    {
        ListView lvColorLst;

        public Form1()
        {
            InitializeComponent();
            dropdownControl2.DrawTextArea += DropdownControl2_DrawTextArea;


            lvColorLst = new ListView();
            lvColorLst.SelectedIndexChanged += LvColorLst_SelectedIndexChanged;
            lvColorLst.FullRowSelect = true;
            lvColorLst.GridLines = true;
            lvColorLst.View = View.Details;
            lvColorLst.BorderStyle = BorderStyle.None;
            lvColorLst.Margin = new Padding(0);
            lvColorLst.Columns.Add("  Color", 45);
            lvColorLst.Columns.Add("Color Name", 100);
            lvColorLst.Width = 145;
            lvColorLst.Height = 200;

            AddColors();           
            dropdownControl2.ClientControl = lvColorLst;
            dropdownControl2.Invalidate();

 
        }

        private void DropdownControl2_DrawTextArea(object sender, DrawTextAreaEventArgs e)
        {
            Rectangle bounds = e.TextArea;
            Rectangle scolor = new Rectangle(bounds.X, bounds.Y, 8, bounds.Height);
            Rectangle textArea = new Rectangle(bounds.X + 8, bounds.Y, bounds.Width - 8, bounds.Height);

            using (SolidBrush sb = new SolidBrush(SystemColors.HighlightText))
            {
                e.Graphics.FillRectangle(sb, e.TextArea);
            }

            using (SolidBrush sb = new SolidBrush(ExtendedColors.FromName(ColorName)))
            {
                e.Graphics.FillRectangle(sb, scolor);
            }

            TextRenderer.DrawText(e.Graphics, ColorName, this.Font, textArea, Color.Black, TextFormatFlags.EndEllipsis);

        }

        private string ColorName = string.Empty;
        private void LvColorLst_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = lvColorLst.SelectedItems;
            if(items.Count > 0)
            {
                ColorName = items[0].SubItems[1].Text;
                tbxView.Text = ColorName;              
            }

        }


        private void AddColors()
        {
            string[] sitems = new string[] { "", "" };
            int count = 0;
            foreach(KnownExtendedColors ec in EnumUtil.GetValues<KnownExtendedColors>())
            {
                sitems[1] = ec.ToString();
                ListViewItem lv = new ListViewItem(sitems);
                lv.SubItems[0].BackColor = ExtendedColors.FromKnownColor(ec);

                lvColorLst.Items.Add(lv);
                lvColorLst.Items[count].UseItemStyleForSubItems = false;
                count++;
                

            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

    public static class EnumUtil
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
