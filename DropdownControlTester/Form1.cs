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
                e.Graphics.FillRectangle(sb,e.TextArea);
            }

            using (SolidBrush sb = new SolidBrush(ExtendedColors.FromName(ColorName)))
            {
                e.Graphics.FillRectangle(sb, scolor);
            }

            TextRenderer.DrawText(e.Graphics, ColorName,this.Font,textArea,Color.Black,TextFormatFlags.EndEllipsis);

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

        // hold for later referance use.
        //private const string _offScreen = "Off Screen";
        //private const string _onScreen = "On Screen";
        //private Form2 _f2;
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    if(button2.Text == _offScreen)
        //    {
        //        if(_f2 == null)
        //        {
        //            _f2 = new Form2();
        //            _f2.Show();
        //            _f2.DesktopLocation = new Point((_f2.Width+5) * -1,0);
        //        }
        //        _f2.DesktopLocation = new Point((_f2.Width + 5) * -1, 0);
        //        button2.Text = _onScreen;
        //        tbxView.Text = _f2.Location.ToString();
        //    }
        //    else
        //    {
        //        if(_f2 != null)
        //        {
        //            _f2.DesktopLocation = new Point(10, 0);
        //        }
        //        button2.Text = _offScreen;
        //    }
        //}

        //public Point OffScreen(Form form)
        //{
        //    Screen[] screens = Screen.AllScreens;
        //    Rectangle[] screenRec = new Rectangle[screens.Length];
        //    Rectangle formRec = new Rectangle(form.Left, form.Top, form.Width, form.Height);
            
        //    for(int i = 0; i < screens.Length; i++)
        //    {
        //        if(screens[i].WorkingArea.Contains(formRec))
        //        {
        //            Point point = new Point(formRec.Width - screens[i].WorkingArea.Width, formRec.Y);
        //            formRec = new Rectangle(point,formRec.Size);
        //            i = 0;
        //            continue;
        //        }
        //    }

        //    return new Point(formRec.X, formRec.Y);

        //    //foreach (Screen screen in screens)
        //    //{
        //    //    Rectangle formRectangle = new Rectangle(form.Left, form.Top,
        //    //                                             form.Width, form.Height);

        //    //    if (screen.WorkingArea.Contains(formRectangle))
        //    //    {
        //    //        screen.WorkingArea.Location
        //    //        return true;
        //    //    }
        //    //}

        //    //return false;
        //}
    }




    public static class EnumUtil
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
