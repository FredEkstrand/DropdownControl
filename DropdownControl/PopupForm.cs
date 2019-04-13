using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;


namespace Ekstrand.Windows.Forms
{
    /// <summary>
    /// Represents a dialog box for hosting a control that makes up an application's user interface.
    /// </summary>
    public partial class PopupForm : Form, IMessageFilter
    {
        #region Fields

        private const int HTCAPTION = 2;
        private const int WM_NCLBUTTONDOWN = 161;
        private const int WM_NCMOUSELEAVE = 674;
        private Control _parent;
        private bool _sendingActivateMessage = false;
        private bool _showDropShadow = true;
        private Size _size = new Size(0, 0);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the PopupForm class.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="item"></param>
        public PopupForm(Control parent, Control item)
        {
            InitializeComponent();
            _parent = parent;

            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            FormBorderStyle = FormBorderStyle.None;
            
            Application.AddMessageFilter(this);

            if (item != null)
            {
                Padding = new Padding(1);
                item.Location = new Point(1, 1);
                Controls.Add(item);
                Size = new Size(item.Width, item.Height);
            }
            else
            {
                AutoSize = false;
                Size = _size = new Size(_parent.PreferredSize.Width + 5, _parent.PreferredSize.Height);
                BackColor = SystemColors.ControlDark;
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or set a value specifying if pop-up window is to have a drop shadow.
        /// </summary>
        public bool ShowDropShadow
        {
            get
            {
                return _showDropShadow;
            }

            set
            {
                if (value != _showDropShadow)
                {
                    _showDropShadow = value;
                    CreateHandle();
                }
            }
        }

        /// <summary>
        /// Gets the required creation parameters when the control handle is created.
        /// </summary>
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

        #endregion Properties

        #region Methods

        /// <summary>
        /// Filters out a message before it is dispatched.
        /// </summary>
        /// <param name="m">The message to be dispatched. You cannot modify this message.</param>
        /// <returns>
        /// Boolean true to filter the message and stop it from being dispatched; 
        /// false to allow the message to continue to the next filter or control.
        /// </returns>
        public bool PreFilterMessage(ref Message m)
        {
            if (Visible && (ActiveForm == null || !ActiveForm.Equals(this)))
            {
                Visible = false;
                Close();
            }

            if (m.Msg == WM_NCLBUTTONDOWN)
            {
                if ((int)m.WParam == HTCAPTION)
                {
                    Visible = false;
                }

            }

            if (m.Msg == WM_NCMOUSELEAVE)
            {
                if (!Visible)
                {
                    Close();
                }
            }

            return false;
        }

        /// <summary>
        /// Raises the Closing event.
        /// </summary>
        /// <param name="e">A CancelEventArgs that contains the event data.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (Controls.Count > 0)
            {
                Application.RemoveMessageFilter(this);
                Controls.RemoveAt(0); //prevent the control from being disposed                
            }

            base.OnClosing(e);
        }

        /// <summary>
        /// Raises the Paint event.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_size.Width != 0)
            {
                Size = _size;
            }

            e.Graphics.DrawRectangle(SystemPens.HotTrack, new Rectangle(0, 0, ClientSize.Width - 1, ClientSize.Height - 1));

        }

        private void WmPrintClient(ref Message m)
        {
            using (PaintEventArgs e = new PrintPaintEventArgs(m, m.WParam, ClientRectangle))
            {
                OnPrint(e);
            }
        }

        /// <summary>
        /// Processes Windows messages.
        /// </summary>
        /// <param name="m">The Windows Message to process.</param>
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
                                    // we're activating - notify the previous Form(s) that we're activating.
                                    // So they can change their UI cues as needed.
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

                case NativeMethods.WM_PRINTCLIENT:
                    {
                        WmPrintClient(ref m);
                    }
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }

        }

        #endregion Methods

        // taken from MS reference source: winforms\Managed\System\WinForms\Control.cs
        private sealed class PrintPaintEventArgs : PaintEventArgs
        {
            Message m;
            
            internal PrintPaintEventArgs(Message m, IntPtr dc, Rectangle clipRect) : base(Graphics.FromHdc(dc), clipRect)
            {
                this.m = m;
            }

            internal Message Message
            {
                get
                {
                    return m;
                }
            }
        }
    }
}
