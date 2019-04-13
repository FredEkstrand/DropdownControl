using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ekstrand.Windows.Forms
{
    /// <summary>
    /// Represents a Windows Dropdown Control.
    /// </summary>
    [ToolboxBitmap(typeof(DropdownControl), "Resources.DropdownControl")]
    [ToolboxItem(true)]
    [Designer(typeof(DropdownControlDesigner))]
    public class DropdownControl : Control
    {

        #region Fields

        private const int Disabled = 0x00100;
        private const int Have_Focus = 0x00010;
        private const int Mouse_Down = 0x00004;
        private const int Mouse_Enter = 0x00001;
        private const int Mouse_Leave = 0x00002;
        private const int Mouse_Up = 0x00008;
        private const int NonButtonArea = 0x00080;
        private const int Popup_Shown = 0x00040;
        private const int Show_Popup = 0x00020;
        private static readonly object EventDrawTextArea = new object();
        private static readonly object EventPopupWindow = new object();
        private Rectangle _buttonRectangle;
        private DropdownButtonSide _buttonSide;
        private DockSide _dockSide;
        private DropdownState _dropdownState;
        private Control _hostControl;
        private BitVector32 _internalState = new BitVector32(0);
        private PopupForm _popupForm;
        private int _preferredHeight = 21;
        private bool _showDropShadow;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Dropdown Control class.
        /// </summary>
        public DropdownControl()
        {
            SetStyle(ControlStyles.ResizeRedraw |
                      ControlStyles.OptimizedDoubleBuffer |
                      ControlStyles.Selectable |
                      ControlStyles.UserMouse |
                      ControlStyles.UserPaint |
                      ControlStyles.AllPaintingInWmPaint,
                      true);

            _dropdownState = DropdownState.Normal;
            _dockSide = DockSide.Left;
            _hostControl = null;
            _showDropShadow = true;

            ControlAdded += ClientControlAdded;
            LostFocus += ControlLostFocus;
            GotFocus += ControlGotFocus;
            TextChanged += ControlTextChanged;
        }

        /// <summary>
        /// Initializes a new instance of the Dropdown Control class.
        /// </summary>
        /// <param name="item">Control to be hosted in the pop-up form.</param>
        public DropdownControl(Control item) : base()
        {
            _hostControl = item;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets a value specifying the location of the Dropdown Control button.
        /// </summary>
        [
                   Category("Appearance"),
           Description("Drop Down Button Side"),
           Browsable(true),
           SettingsBindable(true),
           EditorBrowsable(EditorBrowsableState.Always),
           DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
        ]
        public DropdownButtonSide ButtonSide
        {
            get
            {
                return _buttonSide;
            }

            set
            {
                if (_buttonSide != value)
                {
                    _buttonSide = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the control to be hosted in the pop-up window.
        /// </summary>
        [
                    Browsable(false),
            Description("Control to be displayed in pop-up window")
        ]
        public Control ClientControl
        {
            get
            {
                return _hostControl;
            }

            set
            {
                _hostControl = value;
            }
        }

        /// <summary>
        /// Gets or sets a value specifying to disable the control.
        /// </summary>
        [
           Category("Behavior"),
           Description("Disable Control"),
           Browsable(true),
           DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
        ]
        public bool Disable
        {
            get
            {
                return GetFlag(Disabled);
            }

            set
            {
                SetFlag(Disabled, value);

                if (GetFlag(Disabled))
                {
                    _dropdownState = DropdownState.Disabled;
                    Invalidate();
                }
                else
                {
                    if (_dropdownState == DropdownState.Disabled)
                    {
                        _dropdownState = DropdownState.Normal;
                        Invalidate();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the preferred height of the Dropdown Control
        /// </summary>
        [
            Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
            Description("Preferred Height")
        ]
        public int PreferredHeight
        {
            get
            {   // ripped from MS source code to set height of control based on font size.
                Size textSize = TextRenderer.MeasureText("jj^", this.Font, new Size(int.MaxValue, (int)(FontHeight * 1.25)), TextFormatFlags.SingleLine);
                _preferredHeight = (short)(textSize.Height + SystemInformation.BorderSize.Height * 8 + Padding.Size.Height);
                return _preferredHeight;
            }
        }

        /// <summary>
        /// Gets or set a value specifying if pop-up window is to have a drop shadow.
        /// </summary>
        [
           Category("Appearance"),
           Description("Show drop shadow on Pop-up window"),
           Browsable(true),
        ]
        public bool ShowDropShadow
        {
            get { return _showDropShadow; }
            set { _showDropShadow = value; }
        }

        /// <summary>
        /// Gets a rectangle specifying the text area bounds.
        /// </summary>
        public Rectangle TextBounds
        {
            get
            {
                return DropdownRenderer.TextBoxBounds(ClientRectangle, ButtonSide);
            }
        }

        /// <summary>
        /// Gets the default minimum size of the control.
        /// </summary>
        protected override Size DefaultMinimumSize
        {
            get
            {
                return new Size(21, 20);
            }
        }

        /// <summary>
        /// Gets the default size of the control.
        /// </summary>
        protected override Size DefaultSize
        {
            get
            {
                return new Size(121, _preferredHeight);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Specifying pop-up window location relative to the control position on the form.
        /// </summary>
        /// <returns>Rectangle with the location on the screen to place the pop-up form.</returns>
        protected virtual Rectangle GetDropdownBounds()
        {
            Size inflatedDropSize = new Size(_popupForm.Width + 2, _popupForm.Height + 2);
            Rectangle screenBounds = _dockSide == DockSide.Left ?
                new Rectangle(this.Parent.PointToScreen(new Point(this.Bounds.X, this.Bounds.Bottom)), inflatedDropSize)
                : new Rectangle(this.Parent.PointToScreen(new Point(this.Bounds.Right - _popupForm.Width, this.Bounds.Bottom)), inflatedDropSize);
            Rectangle workingArea = Screen.GetWorkingArea(screenBounds);

            //make sure we're completely in the top-left working area
            if (screenBounds.X < workingArea.X) screenBounds.X = workingArea.X;
            if (screenBounds.Y < workingArea.Y) screenBounds.Y = workingArea.Y;

            //make sure we're not extended past the working area's right /bottom edge
            if (screenBounds.Right > workingArea.Right && workingArea.Width > screenBounds.Width)
                screenBounds.X = workingArea.Right - screenBounds.Width;
            if (screenBounds.Bottom > workingArea.Bottom && workingArea.Height > screenBounds.Height)
                screenBounds.Y = workingArea.Bottom - screenBounds.Height;

            return screenBounds;
        }

        /// <summary> 
        /// Raises the DrawTextArea event for the specified control.
        /// </summary>
        /// <param name="dtae">An DrawTextAreaEventArgs that contains the event data.</param>
        protected void InvokeDrawTextArea(DrawTextAreaEventArgs dtae)
        {
            OnDrawTextArea(dtae);
        }

        /// <summary>
        /// Raises the DrawTextArea event of the control.
        /// </summary>
        /// <param name="e">A DrawTextAreaEventArgs that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnDrawTextArea(DrawTextAreaEventArgs e)
        {
            DrawTextAreaEventHandler handler = (DrawTextAreaEventHandler)Events[EventDrawTextArea];
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Raises the MouseDown event.
        /// </summary>
        /// <param name="e">A MouseEventArgs that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            SetFlag(Mouse_Up, false);
            SetFlag(Mouse_Down, true);

            if (GetFlag(Disabled))
            {
                _dropdownState = DropdownState.Disabled;
                Invalidate();
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                if (_buttonRectangle.Contains(this.PointToClient(Cursor.Position)))
                {
                    SetFlag(NonButtonArea, false);
                    _dropdownState = DropdownState.Pressed;
                    Invalidate();
                }
                else
                {
                    SetFlag(NonButtonArea, true);
                    _dropdownState = DropdownState.Default;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Raises the MouseEnter event.
        /// </summary>
        /// <param name="e">A MouseEventArgs that contains the event data.</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            SetFlag(Mouse_Leave, false);

            if (GetFlag(Show_Popup))
            {
                return;
            }

            SetFlag(Popup_Shown, false);
            SetFlag(Mouse_Enter, true);
            _dropdownState = DropdownState.Hot;
            Invalidate();
        }

        /// <summary>
        /// Raises the MouseLeave event.
        /// </summary>
        /// <param name="e">A MouseEventArgs that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            SetFlag(Mouse_Enter, false);
            SetFlag(Mouse_Leave, true);

            if (GetFlag(Show_Popup))
            {// pop up is open
                _dropdownState = DropdownState.Pressed;
                Invalidate();
            }
            else
            {// pop up is closed
                if (GetFlag(Have_Focus))
                {
                    _dropdownState = DropdownState.Default;
                    Invalidate();
                }
                else
                {// lost focus
                    _dropdownState = DropdownState.Normal;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Raises the MouseMove event.
        /// </summary>
        /// <param name="e">A MouseEventArgs that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (GetFlag(Show_Popup))
            {
                return;
            }

            if (_buttonRectangle.Contains(this.PointToClient(Cursor.Position)))
            {// button area of control
                if (_dropdownState != DropdownState.Hot)
                {
                    _dropdownState = DropdownState.Hot;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Raises the MouseUp event.
        /// </summary>
        /// <param name="e">A MouseEventArgs that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            SetFlag(Mouse_Down, false);
            SetFlag(Mouse_Up, true);

            if (!GetFlag(Have_Focus))
            {
                _dropdownState = DropdownState.Normal;
                Invalidate();
                return;
            }

            if (GetFlag(Mouse_Up) && GetFlag(Mouse_Enter) && GetFlag(Popup_Shown))
            {
                SetFlag(Popup_Shown, false);
                return;
            }

            if (GetFlag(Popup_Shown))
            {
                SetFlag(Popup_Shown, false);
                if (!GetFlag(Mouse_Enter))
                {
                    return;
                }
            }

            if (!GetFlag(Show_Popup) && !GetFlag(NonButtonArea))
            {
                _dropdownState = DropdownState.Pressed;
                Invalidate();
                OpenPopupWindow();
                AnimateDropdownControl();
            }
            else
            {
                _dropdownState = DropdownState.Hot;
                Invalidate();
            }
        }

        /// <summary>
        /// Raises the Paint event.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _buttonRectangle = DropdownRenderer.ButtonBounds(ClientRectangle, ButtonSide);

            if (GetFlag(Have_Focus) && _dropdownState == DropdownState.Hot)
            {
                DropdownRenderer.DrawDropDownControl(this, e.Graphics, DropdownState.Default);
                DropdownRenderer.DrawDropDownButton(this, e.Graphics, _dropdownState);
            }
            else
            {
                DropdownRenderer.DrawDropDownControl(this, e.Graphics, _dropdownState);
            }

            InvokeDrawTextArea(new DrawTextAreaEventArgs(e.Graphics, DropdownRenderer.TextBoxBounds(ClientRectangle, ButtonSide)));
        }

        /// <summary>
        /// Raises the Resize event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            this.Bounds = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, PreferredHeight);
            base.OnResize(e);
        }

        /// <summary>
        /// Opens the pop-up form
        /// </summary>
        protected void OpenPopupWindow()
        {
            if (_popupForm == null)
            {
                SetFlag(Show_Popup, true);

                _popupForm = new PopupForm(this, _hostControl);
                _popupForm.Bounds = GetDropdownBounds();
                _popupForm.ShowDropShadow = ShowDropShadow;
                _popupForm.FormClosing += PopupWindowFormClosing;
                _popupForm.FormClosed += PopupWindowFormClosed;
                _popupForm.Show(this);
            }
        }

        private void AnimateDropdownControl()
        {
            if (_popupForm.Visible)
            {
                _popupForm.Hide();
            }
            WinOSAnimation.AnimateControl(_popupForm, 50, AnimationTypes.VER_POSITIVE | AnimationTypes.SLIDE);
        }

        private void ClientControlAdded(object sender, ControlEventArgs e)
        {
            /* 
             * At design time when adding a client control to this control would result it being placed in this control collection.
             * This behavior can not be overridden because it is done at the base class level. So, we'll play nice and move
             * the client control over to its proper place.
             */
            if (this.Controls.Count > 0)
            {
                if (this.Controls.Count > 1)
                {
                    Controls.Clear();
                    throw new InvalidOperationException("This control can only host one control");
                }

                _hostControl = Controls[0];
                Controls.Clear();
            }
        }

        private void ClosePopUpForm()
        {
            SetFlag(Show_Popup, false);
            SetFlag(Popup_Shown, true);
            if (!_popupForm.IsDisposed)
            {
                _popupForm.FormClosed -= PopupWindowFormClosed;
                _popupForm.FormClosing -= PopupWindowFormClosing;
                _popupForm.Dispose();
                OnPopupWindowClosed(new EventArgs());
            }
            _popupForm = null;

        }

        private void ControlGotFocus(object sender, EventArgs e)
        {
            SetFlag(Have_Focus, true);
        }

        private void ControlLostFocus(object sender, EventArgs e)
        {
            SetFlag(Have_Focus, false);

            if (!GetFlag(Show_Popup) && GetFlag(Mouse_Leave))
            {
                _dropdownState = DropdownState.Normal;
                Invalidate();
            }

        }

        private void ControlTextChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private bool GetFlag(int flag)
        {
            return _internalState[flag];
        }

        /// <summary>
        /// Raises the PopupWindowClosed event of the control.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        private void OnPopupWindowClosed(EventArgs e)
        {
             PopupWindowClosedHandler handler = (PopupWindowClosedHandler)Events[EventPopupWindow];
            if (handler != null) handler(this, e);
        }
        private void OnPopupWindowStateChange()
        {
            _dropdownState = DropdownState.Default;
            Invalidate();
        }

        private void PopupWindowFormClosed(object sender, FormClosedEventArgs e)
        {
            ClosePopUpForm();
        }

        private void PopupWindowFormClosing(object sender, FormClosingEventArgs e)
        {
            _dropdownState = DropdownState.Default;
            Invalidate();
        }

        private void SetFlag(int flag, bool value)
        {
            _internalState[flag] = value;
        }

        #endregion Methods

        #region Delegates + Events

        /// <summary>
        /// Provides data for the Paint text area event.
        /// </summary>
        [Category("Appearance"), Description("Occurs for owner drawn text area.")]
        public event DrawTextAreaEventHandler DrawTextArea
        {
            add
            {
                Events.AddHandler(EventDrawTextArea, value);
            }
            remove
            {
                Events.RemoveHandler(EventDrawTextArea, value);
            }
        }

        /// <summary>
        /// Represents the method that will handle the Paint Text Area event of the Dropdown Control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A DrawTextAreaEventArgs that contains the event data.</param>
        public delegate void DrawTextAreaEventHandler(object sender, DrawTextAreaEventArgs e);

        /// <summary>
        /// Provides data for the Popup Window Closed event.
        /// </summary>
        [Category("Behavior"), Description("Occurs whenever the Popup Window closes.")]
        public event  PopupWindowClosedHandler PopupWindowClosed
        { 
            add
            {
                Events.AddHandler(EventPopupWindow, value);
            }
            remove
            {
                Events.RemoveHandler(EventPopupWindow, value);
            }
        }
        /// <summary>
        /// Represents the method that will handle the Popup Window Closed event of the Dropdown Control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A EventArgs that contains the event data.</param>
        public delegate void PopupWindowClosedHandler(object sender, EventArgs e);

        #endregion Delegates + Events

    }
}
