using System.Drawing;
using System.Windows.Forms;

namespace Ekstrand.Windows.Forms
{
    
    public sealed class DropdownRenderer
    {
        private const TextFormatFlags Flags = TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.GlyphOverhangPadding | TextFormatFlags.LeftAndRightPadding;
        private static DropdownControl _dropdownControl;
        private static Color _BorderColor = Color.FromArgb(122, 122, 122);
        private static Color _DisabledBoarderColor = Color.FromArgb(166, 166, 166);
        private static Color _FocusBackGroundColor;
        private static Color _FocusBorderColor = SystemColors.HotTrack;
        private static Color _FocusColor = SystemColors.Highlight;
        private static Color _FocusPressedColor = Color.FromArgb(204, 228, 247);
        private static Color _TextAreaColor = SystemColors.HighlightText;
        private static  Image _Chevron1;
        private static  Image _Chevron2;
        private static  Image _Chevron3;
        private static  Image _Chevron4;
        private static Rectangle _textBounds;

        private DropdownRenderer() { }
        
        private static void GetChevrons()
        {
            _FocusBackGroundColor = Color.FromArgb(20, _FocusColor.R, _FocusColor.G, _FocusColor.B);
            _Chevron1 = new Bitmap(Ekstrand.Windows.Forms.Properties.Resources.Chevron1); // normal
            _Chevron2 = new Bitmap(Ekstrand.Windows.Forms.Properties.Resources.Chevron2); // pressed
            _Chevron3 = new Bitmap(Ekstrand.Windows.Forms.Properties.Resources.Chevron3); // disabled
            _Chevron4 = new Bitmap(Ekstrand.Windows.Forms.Properties.Resources.Chevron4); // hot
        }
        public static Rectangle TextBoxBounds(Rectangle bounds, DropdownButtonSide side)
        {
            Rectangle tb = new Rectangle();
            Rectangle r = Rectangle.Inflate(bounds, -3, -3);

            switch (side)
            {
                case DropdownButtonSide.Right:
                    r.Width = r.Width - 18;
                    tb = r;
                    break;

                case DropdownButtonSide.Left:
                    r.X = r.X + 18;
                    r.Width = r.Width - 18;
                    tb = r;
                    break;
            }

            return tb;
        }

        public static Rectangle ButtonBounds(Rectangle rec, DropdownButtonSide side)
        {
            Rectangle r = new Rectangle(0, 0, rec.Width, rec.Height);

            if (side == DropdownButtonSide.Right)
            {
                return new Rectangle(r.Width - 18, r.Y, 18, r.Height);
            }
            else
            {
                return new Rectangle(0, 0, 18, r.Height);
            }
        }

        public static void DrawDropDownControl(DropdownControl c, Graphics g, DropdownState state)
        {
            DrawDropDownControl(c, g, Flags, state);
        }

        // (Graphics g, Rectangle bounds, string text, Font font, Color foreColor, Color background, TextFormatFlags flags, DropDownState state)
        public static void DrawDropDownControl(DropdownControl c, Graphics g, TextFormatFlags flags, DropdownState state)
        {
            Rectangle bounds = c.ClientRectangle;

            if (_Chevron1 == null)
            {
                GetChevrons();
            }

            Rectangle r = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
            DrawBackground(g, r, state, c.BackColor);

            if (state != DropdownState.Disabled)
            {
                if (state == DropdownState.Default || state == DropdownState.Pressed)
                {
                    g.DrawRectangle(new Pen(_FocusColor), r);
                }
                else
                {
                    g.DrawRectangle(new Pen(_BorderColor), r);
                }
            }
            else
            {
                g.DrawRectangle(new Pen(_DisabledBoarderColor), r);
            }

            DrawDropDownButton(c, g, state);
            DrawTextArea(g, r, c.ButtonSide);
            DrawText(g, _textBounds, c.Text, c.Font, c.ForeColor);
        }

        private static void DrawBackground(Graphics g, Rectangle bounds, DropdownState state, Color background)
        {
            Color c = background;
            if (state != DropdownState.Disabled)
            {
                g.FillRectangle(new SolidBrush(c), bounds);
            }
            else
            {
                g.FillRectangle(SystemBrushes.ControlLight, bounds);
            }
        }

        public static void DrawDropDownButton(DropdownControl c, Graphics g, DropdownState state)
        {
            Rectangle r = ButtonBounds(c.ClientRectangle, c.ButtonSide);
            r = new Rectangle(r.X, r.Y, r.Width-1, r.Height-1);            
            DrawHighlight(g, r, state);

            // x,y offset to center image chevron on split button
            int XPos = ((r.Width / 2) - (_Chevron1.Width / 2));
            int YPos = (r.Height / 2) - (_Chevron1.Height / 2);
            switch (state)
            {
                case DropdownState.Normal:
                case DropdownState.Default:
                    g.DrawImage(_Chevron1, r.X + XPos, YPos);
                    break;

                case DropdownState.Disabled:
                    g.DrawImage(_Chevron3, r.X + XPos, YPos);
                    break;

                case DropdownState.Pressed:
                    g.DrawImage(_Chevron2, r.X + XPos, YPos);
                    g.DrawRectangle(new Pen(_FocusBorderColor), r);
                    break;

                case DropdownState.Hot:
                    g.DrawImage(_Chevron4, r.X + XPos, YPos);
                    using (Pen p = new Pen(_FocusColor))
                    {
                        g.DrawRectangle(p, r);
                    }
                    break;
            }
        }

        private static void DrawHighlight(Graphics g, Rectangle bounds, DropdownState state)
        {
            switch (state)
            {
                case DropdownState.Hot:
                    g.FillRectangle(new SolidBrush(_FocusBackGroundColor), bounds);
                    break;

                case DropdownState.Pressed:
                    g.FillRectangle(new SolidBrush(_FocusPressedColor), bounds);
                    break;
            }
        }

        private static void DrawText(Graphics g, Rectangle bounds, string text, Font font, Color foreColor)
        {
           DrawText(g, bounds, text, font, foreColor, Flags);
        }
        
        private static void DrawText(Graphics g, Rectangle bounds, string text, Font font, Color foreColor, TextFormatFlags flags)
        {
            TextRenderer.DrawText(g, text, font, bounds, foreColor, flags);
        }

        private static void DrawTextArea(Graphics g, Rectangle bounds, DropdownButtonSide side)
        {
            Color c = _TextAreaColor;
            Rectangle r = Rectangle.Inflate(bounds, -3, -3);

            switch (side)
            {
                case DropdownButtonSide.Right:
                    using (SolidBrush b = new SolidBrush(c))
                    {
                        r.Width = r.Width - 18;
                        _textBounds = r;
                        g.FillRectangle(b, r);
                    }
                    break;

                case DropdownButtonSide.Left:
                    using (SolidBrush b = new SolidBrush(c))
                    {
                        r.X = r.X + 18;
                        r.Width = r.Width - 18;
                        _textBounds = r;
                        g.FillRectangle(b, r);
                    }
                    break;
            }
        }
    }

    public enum DropdownState
    {
        Normal = 0,
        Hot = 1,
        Pressed = 2,
        Disabled = 3,
        Default = 4
    }
}
