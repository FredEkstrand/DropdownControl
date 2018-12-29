using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Ekstrand.Windows.Forms
{
    public partial class PopupWindow : Form, IMessageFilter
    {

        #region Fields

        private Control _parent;
        private bool _sendingActivateMessage = false;
        private Size _size = new Size(0, 0);
        private Rectangle _bounds;
        private const int WM_LBUTTONDOWN = 513;
        private const int WM_RBUTTONDOWN = 516;
        private const int WM_MBUTTONDOWN = 519;

        #endregion Fields

        #region Constructors

        public PopupWindow(Control parent, Control item)
        {
            InitializeComponent();
            _parent = parent;

            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            FormBorderStyle = FormBorderStyle.None;
            Application.AddMessageFilter(this);
            Activated += PopupWindow_Activated;
            

            if (item != null)
            {
                Padding = new Padding(1);
                item.Location = new Point(1, 1);
                Controls.Add(item);   
            }
            else
            {
                AutoSize = false;
                Size = _size = new Size(_parent.PreferredSize.Width + 5, _parent.PreferredSize.Height);
                BackColor = SystemColors.ControlDark;
            }
        }

        private void PopupWindow_Activated(object sender, EventArgs e)
        {
           if(Controls.Count > 0)
            {
                Controls[0].Visible = true;
            }
        }


        #endregion Constructors

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (_showDropShadow)
                {
                    cp.ClassStyle |= NativeMethods.CS_DROPSHADOW;
                }
                return cp;
            }
        }

        private bool _showDropShadow = true;

        public bool ShowDropShadow
        {
            get
            {
                return _showDropShadow;
            }

            set
            {
                if(value != _showDropShadow)
                {
                    _showDropShadow = value;
                    CreateHandle();
                }
            }
        }
        

    #region Methods

    public bool PreFilterMessage(ref Message m)
        {
            if (Visible && (ActiveForm == null || !ActiveForm.Equals(this)))
            {
                Visible = false;
                Close();
            }


            return false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_size.Width != 0)
            {
                Size = _size;
                _bounds = ClientRectangle;
            }

            e.Graphics.DrawRectangle(SystemPens.HotTrack, new Rectangle(0, 0, ClientSize.Width - 1, ClientSize.Height - 1));
             
        }

        protected void OnPopupStateChange()
        {
            PopupWindowStateChange?.Invoke();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_NCACTIVATE:
                    {
                        if (m.WParam != IntPtr.Zero /*activating*/)
                        {
                            if (!_sendingActivateMessage)
                            {
                                _sendingActivateMessage = true;
                                try
                                {
                                    // we're activating - notify the previous guy that we're activating.
                                    HandleRef activeHwndHandleRef = new HandleRef(this, _parent.FindForm().Handle);
                                    UnsafeNativeMethods.SendMessage(activeHwndHandleRef, NativeMethods.WM_NCACTIVATE, true, -1);
                                    bool result = SafeNativeMethods.RedrawWindow(activeHwndHandleRef, null, NativeMethods.NullHandleRef, NativeMethods.RDW_FRAME | NativeMethods.RDW_INVALIDATE);
                                    m.WParam = (IntPtr)1;
                                }
                                finally
                                {
                                    _sendingActivateMessage = false;
                                }
                            }

                            base.DefWndProc(ref m);
                            return;

                        }
                        else
                        {
                            base.WndProc(ref m);
                        }

                        base.WndProc(ref m);

                    }
                    break;

                default:
                base.WndProc(ref m);
                break;
            }

        }


        protected override void OnClosing(CancelEventArgs e)
        {
            if (Controls.Count > 0)
            {             
                Application.RemoveMessageFilter(this);
                Controls.RemoveAt(0); //prevent the control from being disposed
                OnPopupStateChange();

            }

            base.OnClosing(e);
        }


        #endregion Methods

        #region Delegates + Events

        public delegate void PopupWindowStateArgs();

        public event PopupWindowStateArgs PopupWindowStateChange;

        #endregion Delegates + Events

    }

    /*
    * Change the design mode behavior of the DropDownControl
    * in the designer
    */
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    internal class PopupWindowDesigner : ControlDesigner
    {

        #region Constructors

        private PopupWindowDesigner()
        {
            base.AutoResizeHandles = true;
        }

        #endregion Constructors

        #region Properties

        public override SelectionRules SelectionRules
        {
            get
            {
                return SelectionRules.LeftSizeable | SelectionRules.RightSizeable | SelectionRules.BottomSizeable | SelectionRules.Moveable;
            }
        }

        #endregion Properties

    }
}
