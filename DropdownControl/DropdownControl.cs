using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Ekstrand.Windows.Forms
{

    public enum DockSideTypes
    {
        Left,
        Right
    }

    public enum DropDownButtonSide
    {
        Left = 1,
        Right = 0
    }

    [ToolboxBitmap(typeof(DropdownControl), "Resources.DropDownControl")]
    [ToolboxItem(true)]
    [Designer(typeof(DropdownControlDesigner))]
    public class DropdownControl : Control
    {

        #region Fields

        private const int Mouse_Enter = 0x00001;
        private const int Mouse_Leave = 0x00002;
        private const int Mouse_Down = 0x00004;
        private const int Mouse_Up = 0x00008;
        private const int Have_Focus = 0x00010;
        private const int Show_Popup = 0x00020;
        private const int Popup_Shown = 0x00040;
        private const int NonButtonArea = 0x00080;
        private const int Disabled = 0x00100;
        private static readonly object Event_SelectDraw = new object();
        private Rectangle _buttonRectangle;
        private DropDownButtonSide _buttonSide;
        private DockSideTypes _dockSide;
        private DropDownState _dropdownState;
        private Control _hostControl;
        private BitVector32 _internalState = new BitVector32(0);
        private PopupWindow _popupWindow;
        private int _preferredHeight = 21;
        private DropDownControlRenderer _renderer;

        #endregion Fields

        #region Constructors

        public DropdownControl(Control item) : base()
        {
            _hostControl = item;
        }

        public DropdownControl()
        {
            SetStyle(ControlStyles.ResizeRedraw |
                      ControlStyles.OptimizedDoubleBuffer |
                      ControlStyles.Selectable |
                      ControlStyles.UserMouse |
                      ControlStyles.UserPaint |
                      ControlStyles.AllPaintingInWmPaint,
                      true);

            _dropdownState = DropDownState.Normal;
            _renderer = new DropDownControlRenderer();
            _dockSide = DockSideTypes.Left;
            _hostControl = null;

            ControlAdded += ClientControlAdded;
            LostFocus += ControlLostFocus;
            GotFocus += ControlGotFocus;
            TextChanged += ControlTextChanged;


        }


        #endregion Constructors

        #region Properties

        [
           Category("Appearance"),
           Description("Drop Down Button Side"),
           Browsable(true),
           SettingsBindable(true),
           EditorBrowsable(EditorBrowsableState.Always),
           DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
        ]
        public DropDownButtonSide ButtonSide
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

        private bool _showDropShadow = true;
        [
           Category("Appearance"),
           Description("Show drop shadow on dropdown window"),
           Browsable(true),
        ]
        public bool ShowDropShadow
        {
            get { return _showDropShadow; }
            set { _showDropShadow = value; }
        }

        [
            Browsable(false),
            Description("Control to be displayed in popup window")
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

        [
           Category("Behavior"),
           Description("Desable Control"),
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
                    _dropdownState = DropDownState.Disabled;
                    Invalidate();
                }
                else
                {
                    if (_dropdownState == DropDownState.Disabled)
                    {
                        _dropdownState = DropDownState.Normal;
                        Invalidate();
                    }
                }
            }
        }

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

        protected override Size DefaultMinimumSize
        {
            get
            {
                return new Size(21, 20);
            }
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(121, _preferredHeight);
            }
        }

        #endregion Properties

        #region Methods

        protected virtual Rectangle GetDropDownBounds()
        {
            Size inflatedDropSize = new Size(_popupWindow.Width + 2, _popupWindow.Height + 2);
            Rectangle screenBounds = _dockSide == DockSideTypes.Left ?
                new Rectangle(this.Parent.PointToScreen(new Point(this.Bounds.X, this.Bounds.Bottom)), inflatedDropSize)
                : new Rectangle(this.Parent.PointToScreen(new Point(this.Bounds.Right - _popupWindow.Width, this.Bounds.Bottom)), inflatedDropSize);
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

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            SetFlag(Mouse_Up, false);
            SetFlag(Mouse_Down, true);

            if (GetFlag(Disabled))
            {
                _dropdownState = DropDownState.Disabled;
                Invalidate();
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                if (_buttonRectangle.Contains(this.PointToClient(Cursor.Position)))
                {
                    SetFlag(NonButtonArea, false);                   
                    _dropdownState = DropDownState.Pressed;
                    Invalidate();
                }
                else
                {
                    SetFlag(NonButtonArea, true);
                    _dropdownState = DropDownState.Default;
                    Invalidate();
                }
            }
        }

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
            _dropdownState = DropDownState.Hot;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            SetFlag(Mouse_Enter, false);
            SetFlag(Mouse_Leave, true);

            if (GetFlag(Show_Popup))
            {// pop up is open
                _dropdownState = DropDownState.Pressed;
                Invalidate();
            }
            else
            {// pop up is closed
                if (GetFlag(Have_Focus))
                {
                    _dropdownState = DropDownState.Default;
                    Invalidate();
                }
                else
                {// lost focus
                    _dropdownState = DropDownState.Normal;
                    Invalidate();
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            base.OnMouseMove(mevent);

            if (GetFlag(Show_Popup))
            {
                return;
            }

            if (_buttonRectangle.Contains(this.PointToClient(Cursor.Position)))
            {// button area of control
                if (_dropdownState != DropDownState.Hot)
                {
                    _dropdownState = DropDownState.Hot;
                    Invalidate();
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);

            SetFlag(Mouse_Down, false);
            SetFlag(Mouse_Up, true);

            if (!GetFlag(Have_Focus))
            {
                _dropdownState = DropDownState.Normal;
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
                _dropdownState = DropDownState.Pressed;
                Invalidate();
                OpenPopupWindow();
            }
            else
            {
                _dropdownState = DropDownState.Hot;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            _buttonRectangle = _renderer.ButtonBounds(ClientRectangle);
            _renderer.DropDownButtonSide = ButtonSide;

            if (GetFlag(Have_Focus) && _dropdownState == DropDownState.Hot)
            {
                _renderer.DrawDropDownControl(g, ClientRectangle, Text, Font, ForeColor, BackColor, DropDownState.Default);
                _renderer.DrawDropDownButton(g, ClientRectangle, _dropdownState);
            }
            else
            {
                _renderer.DrawDropDownControl(g, ClientRectangle, Text, Font, ForeColor, BackColor, _dropdownState);
            }

            OnSelectDrawArea(new DrawTextAreaEventArgs(e.Graphics,_renderer.TextBoxBounds(ClientRectangle)));            

        }

        protected override void OnResize(EventArgs e)
        {
            this.Bounds = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, PreferredHeight);
            base.OnResize(e);
        }

        protected void OpenPopupWindow()
        {
            if (_popupWindow == null)
            {
                SetFlag(Show_Popup, true);

                _popupWindow = new PopupWindow(this, _hostControl);
                _popupWindow.Bounds = GetDropDownBounds();
                _popupWindow.ShowDropShadow = ShowDropShadow;
                _popupWindow.PopupWindowStateChange += PopupWindowStateChange;
                _popupWindow.FormClosed += _popupWindow_FormClosed;
                _popupWindow.Show(this);
            }
        }

        

        private void ClientControlAdded(object sender, ControlEventArgs e)
        {
            /* 
             * At design time when adding a client control to this control would result it being placed in this control collection.
             * This behavior can not be overridden bacause it is done at the base class level. So, we'll play nice and move
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

        private void _popupWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            SetFlag(Show_Popup, false);
            SetFlag(Popup_Shown, true);
            if (!_popupWindow.IsDisposed)
            {

                _popupWindow.FormClosed -= _popupWindow_FormClosed;
                _popupWindow.PopupWindowStateChange -= PopupWindowStateChange;
                _popupWindow.Dispose();
            }
            _popupWindow = null;
            LogText("_popupWindow_FormClosed: " + _ShowStatsCounter);

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
                _dropdownState = DropDownState.Normal;
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


        private void PopupWindowStateChange()
        {
            _dropdownState = DropDownState.Default;
            Invalidate();
        }

        private void SetFlag(int flag, bool value)
        {
            _internalState[flag] = value;
        }


        private void OnSelectDrawArea(DrawTextAreaEventArgs e)
        {
            DrawTextArea?.Invoke(this, e);
        }

        #endregion Methods

        #region Delegates + Events

        [Category("Appearance"), Description("Occurs for owner drawn text area.")]
        public event EventHandler<DrawTextAreaEventArgs> DrawTextArea;

        #endregion Delegates + Events

        #region testing

        private void LogText(string s)
        {
            WriteToLog(s + "\n");
        }
        private void UIStatus(string s)
        {
            WriteToLog("UI state: " + s + "\n");
            Console.WriteLine("UI state: " + s + "\n");
        }

        private static int _ShowStatsCounter = 0;

        private void ShowStates(string loc)
        {

            _ShowStatsCounter++;
            StringBuilder sb = new StringBuilder();

            if (loc != string.Empty)
            {
                sb.Append("Sample: " + _ShowStatsCounter + " Loc: " + loc + "\n");
            }
            else
            {
                sb.Append("Sample: " + _ShowStatsCounter + "\n");
            }

            sb.Append("----------------------------------------------------\n");
            sb.Append("Have_Focus: " + GetFlag(Have_Focus) + "\n");
            sb.Append("Mouse_Down: " + GetFlag(Mouse_Down) + "\n");
            sb.Append("Mouse_Enter: " + GetFlag(Mouse_Enter) + "\n");
            sb.Append("Mouse_Leave: " + GetFlag(Mouse_Leave) + "\n");
            sb.Append("Mouse_Up: " + GetFlag(Mouse_Up) + "\n");
            sb.Append("NonButtonArea: " + GetFlag(NonButtonArea) + "\n");
            sb.Append("Popup_Shown: " + GetFlag(Popup_Shown) + "\n");
            sb.Append("Show_Popup: " + GetFlag(Show_Popup) + "\n");
            sb.Append("====================================================\n");

            WriteToLog(sb.ToString());
            Console.WriteLine(sb.ToString());
        }

        private bool clearFile = true;
        private readonly string location = @"C:\SoftwareProjects\DropdownControl\Output.txt";
        private void WriteToLog(string s)
        {
            if (clearFile)
            {
                clearFile = false;
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(location, false))
                {
                    sw.WriteLine("");
                }
            }

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(location, true))
            {
                sw.WriteLine(s);
            }
        }

        #endregion

    }


    /*
     * Change the design mode behavior of the DropDownControl
     * in the designer
     */
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    internal class DropdownControlDesigner : ControlDesigner
    {

        #region Constructors

        private DropdownControlDesigner()
        {
            base.AutoResizeHandles = true;
        }

        #endregion Constructors

        #region Properties

        public override SelectionRules SelectionRules
        {
            get
            {
                return SelectionRules.LeftSizeable | SelectionRules.RightSizeable | SelectionRules.Moveable;
            }
        }

        #endregion Properties

    }
}