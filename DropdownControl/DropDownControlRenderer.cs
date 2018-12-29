using System.Drawing;
using System.Windows.Forms;

namespace Ekstrand.Windows.Forms
{
    public sealed class DropDownControlRenderer
    {
        #region Fields

        private Color _BoarderColor;
        private Color _DropDownButtonHotColor;
        private Color _DropDownButtonPressedColor;
        private Color _TextAreaBackColor;
        private const TextFormatFlags _Flags = TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.GlyphOverhangPadding | TextFormatFlags.LeftAndRightPadding;
        private DropDownButtonSide m_DropDownButton;
        private readonly Color _BorderColor = Color.FromArgb(122, 122, 122);
        private readonly Color _DisabledBoarderColor = Color.FromArgb(166, 166, 166);
        private readonly Color _FocusBackGroundColor;
        private readonly Color _FocusBorderColor = SystemColors.HotTrack;
        private readonly Color _FocusColor = SystemColors.Highlight;
        private readonly Color _FocusPressedColor = Color.FromArgb(204, 228, 247);
        private readonly Color _TextAreaColor = SystemColors.HighlightText;
        private readonly Image _Chevron1;
        private readonly Image _Chevron2;
        private readonly Image _Chevron3;
        private readonly Image _Chevron4;

        #endregion Fields

        #region Constructors

        public DropDownControlRenderer()
        {
            _FocusBackGroundColor = Color.FromArgb(20, _FocusColor.R, _FocusColor.G, _FocusColor.B);
            m_DropDownButton = DropDownButtonSide.Right;
            TextBounds = new Rectangle();
            DropDownButtonHotColor = _FocusBackGroundColor;
            DropDownButtonPressedColor = _FocusPressedColor;
            BoarderColor = _BorderColor;
            TextAreaBackColor = _TextAreaColor;

            _Chevron1 = new Bitmap(Ekstrand.Windows.Forms.Properties.Resources.Chevron1); // normal
            _Chevron2 = new Bitmap(Ekstrand.Windows.Forms.Properties.Resources.Chevron2); // pressed
            _Chevron3 = new Bitmap(Ekstrand.Windows.Forms.Properties.Resources.Chevron3); // disabled
            _Chevron4 = new Bitmap(Ekstrand.Windows.Forms.Properties.Resources.Chevron4); // hot

        }

        #endregion Constructors

        #region Properties

        public Color BoarderColor
        {
            get
            {
                if (_BoarderColor == null)
                {
                    return _BoarderColor = _BorderColor;
                }

                return _BoarderColor;
            }
            set
            {
                if (_BoarderColor.Equals(value) != true)
                {
                    if (value == Color.Empty)
                    {
                        _BoarderColor = _BorderColor;
                        return;
                    }
                    _BoarderColor = value;
                }
            }
        }

        public Color DropDownButtonHotColor
        {
            get
            {
                if (_DropDownButtonHotColor == null)
                {
                    return _DropDownButtonHotColor = _FocusBackGroundColor;
                }
                return _DropDownButtonHotColor;
            }
            set
            {
                if (_DropDownButtonHotColor.Equals(value) != true)
                {
                    if (value == Color.Empty)
                    {
                        _DropDownButtonHotColor = _FocusBackGroundColor;
                        return;
                    }
                    _DropDownButtonHotColor = value;
                }
            }
        }

        public Color DropDownButtonPressedColor
        {
            get
            {
                if (_DropDownButtonPressedColor == null)
                {
                    return _DropDownButtonPressedColor = _FocusPressedColor;
                }
                return _DropDownButtonPressedColor;
            }
            set
            {
                if (_DropDownButtonPressedColor.Equals(value) != true)
                {
                    if (value == Color.Empty)
                    {
                        _DropDownButtonPressedColor = _FocusPressedColor;
                        return;
                    }

                    _DropDownButtonPressedColor = value;
                }
            }
        }

        public DropDownButtonSide DropDownButtonSide
        {
            get { return m_DropDownButton; }
            set
            {
                if (value != m_DropDownButton)
                {
                    m_DropDownButton = value;
                }
            }
        }

        public Color TextAreaBackColor
        {
            get
            {
                if (_TextAreaBackColor == null)
                {
                    return _TextAreaBackColor = _TextAreaColor;
                }
                return _TextAreaBackColor;
            }
            set
            {
                if (_TextAreaBackColor.Equals(value) != true)
                {
                    if (value == Color.Empty)
                    {
                        _TextAreaBackColor = _TextAreaColor;
                        return;
                    }
                    _TextAreaBackColor = value;
                }
            }
        }

        private Rectangle TextBounds
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        public Rectangle TextBoxBounds(Rectangle bounds)
        {
            Rectangle tb = new Rectangle();
            Rectangle r = Rectangle.Inflate(bounds, -3, -3);

            switch (DropDownButtonSide)
            {
                case DropDownButtonSide.Right:
                r.Width = r.Width - 18;
                tb = r;
                break;

                case DropDownButtonSide.Left:
                r.X = r.X + 18;
                r.Width = r.Width - 18;
                tb = r;
                break;
            }

            return tb;
        }

        public Rectangle ButtonBounds(Rectangle rec)
        {
            Rectangle r = new Rectangle(0, 0, rec.Width, rec.Height);

            if (DropDownButtonSide == DropDownButtonSide.Right)
            {
                return new Rectangle(r.Width - 18, r.Y, 18, r.Height);
            }
            else
            {
                return new Rectangle(0, 0, 18, r.Height);
            }
        }

        public void DrawDropDownButton(Graphics g, Rectangle bounds, DropDownState state)
        {
            Rectangle r = ButtonBounds(bounds);

            DrawHighlight(g, r, state);

            // x,y offset to center image chevron on split button
            int XPos = ((r.Width / 2) - (_Chevron1.Width / 2));
            int YPos = (r.Height / 2) - (_Chevron1.Height / 2);
            switch (state)
            {
                case DropDownState.Normal:
                case DropDownState.Default:
                g.DrawImage(_Chevron1, r.X + XPos, YPos);
                break;

                case DropDownState.Disabled:
                g.DrawImage(_Chevron3, r.X + XPos, YPos);
                break;

                case DropDownState.Pressed:
                g.DrawImage(_Chevron2, r.X + XPos, YPos);
                g.DrawRectangle(new Pen(_FocusBorderColor), r);
                break;

                case DropDownState.Hot:
                g.DrawImage(_Chevron4, r.X + XPos, YPos);
                using (Pen p = new Pen(_FocusColor))
                {
                    g.DrawRectangle(p, r);
                }
                break;
            }
        }

        public void DrawDropDownControl(Graphics g, Rectangle bounds, DropDownState state)
        {
            Rectangle r = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
            DrawBackground(g, r, state);

            if (state != DropDownState.Disabled)
            {
                if (state == DropDownState.Default)
                {
                    g.DrawRectangle(new Pen(_FocusColor), r);
                }
                else
                {
                    g.DrawRectangle(new Pen(_BoarderColor), r);
                }
            }
            else
            {
                g.DrawRectangle(new Pen(_DisabledBoarderColor), r);
            }

            DrawDropDownButton(g, r, state);
            DrawTextArea(g, bounds);
        }

        public void DrawDropDownControl(Graphics g, Rectangle bounds, string text, Font font, DropDownState state)
        {
            DrawDropDownControl(g, bounds, text, font, SystemColors.WindowText, SystemColors.Control, _Flags, state);
        }

        public void DrawDropDownControl(Graphics g, Rectangle bounds, string text, Font font, Color foreColor, DropDownState state)
        {
            DrawDropDownControl(g, bounds, text, font, foreColor, SystemColors.Control, _Flags, state);
        }

        public void DrawDropDownControl(Graphics g, Rectangle bounds, string text, Font font, Color foreColor, TextFormatFlags flags, DropDownState state)
        {
            DrawDropDownControl(g, bounds, text, font, foreColor, SystemColors.Control, flags, state);
        }

        public void DrawDropDownControl(Graphics g, Rectangle bounds, string text, Font font, Color foreColor, Color background, DropDownState state)
        {
            DrawDropDownControl(g, bounds, text, font, foreColor, background, _Flags, state);
        }

        public void DrawDropDownControl(Graphics g, Rectangle bounds, string text, Font font, Color foreColor, Color background, TextFormatFlags flags, DropDownState state)
        {
            Rectangle r = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
            DrawBackground(g, r, state, background);

            if (state != DropDownState.Disabled)
            {
                if (state == DropDownState.Default || state == DropDownState.Pressed)
                {
                    g.DrawRectangle(new Pen(_FocusColor), r);
                }
                else
                {
                    g.DrawRectangle(new Pen(_BoarderColor), r);
                }
            }
            else
            {
                g.DrawRectangle(new Pen(_DisabledBoarderColor), r);
            }

            DrawDropDownButton(g, r, state);
            DrawTextArea(g, bounds);
            DrawText(g, TextBounds, text, font, foreColor);
        }

        public void DrawText(Graphics g, string text, Font font)
        {
            DrawText(g, TextBounds, text, font, SystemColors.WindowText);
        }

        public void DrawText(Graphics g, Rectangle bounds, string text, Font font, Color foreColor)
        {
            TextFormatFlags flags = TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.GlyphOverhangPadding | TextFormatFlags.LeftAndRightPadding;
            DrawText(g, bounds, text, font, foreColor, flags);
        }

        public void DrawText(Graphics g, Rectangle bounds, string text, Font font, Color foreColor, TextFormatFlags flags)
        {
            TextRenderer.DrawText(g, text, font, bounds, foreColor, flags);
        }

        private void DrawBackground(Graphics g, Rectangle bounds, DropDownState state, Color? background = null)
        {
            Color c = background.GetValueOrDefault(SystemColors.Control);
            if (state != DropDownState.Disabled)
            {
                g.FillRectangle(new SolidBrush(c), bounds);
            }
            else
            {
                g.FillRectangle(SystemBrushes.ControlLight, bounds);
            }
        }

        private void DrawHighlight(Graphics g, Rectangle bounds, DropDownState state)
        {
            switch (state)
            {
                case DropDownState.Hot:
                g.FillRectangle(new SolidBrush(DropDownButtonHotColor), bounds);
                break;

                case DropDownState.Pressed:
                g.FillRectangle(new SolidBrush(DropDownButtonPressedColor), bounds);
                break;
            }
        }

        private void DrawTextArea(Graphics g, Rectangle bounds)
        {
            Color c = TextAreaBackColor;
            Rectangle r = Rectangle.Inflate(bounds, -3, -3);

            switch (DropDownButtonSide)
            {
                case DropDownButtonSide.Right:
                using (SolidBrush b = new SolidBrush(c))
                {
                    r.Width = r.Width - 18;
                    TextBounds = r;
                    g.FillRectangle(b, r);
                }
                break;

                case DropDownButtonSide.Left:
                using (SolidBrush b = new SolidBrush(c))
                {
                    r.X = r.X + 18;
                    r.Width = r.Width - 18;
                    TextBounds = r;
                    g.FillRectangle(b, r);
                }
                break;
            }
        }

        #endregion Methods
    }

    public enum DropDownState
    {
        Normal = 0,
        Hot = 1,
        Pressed = 2,
        Disabled = 3,
        Default = 4
    }
}