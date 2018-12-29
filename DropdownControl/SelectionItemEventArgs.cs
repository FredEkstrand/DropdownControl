using System;
using System.Drawing;
using System.Text;

namespace Ekstrand.Windows.Forms
{
    public class SelectionDrawAreaEventArgs : EventArgs
    {
        private readonly System.Drawing.Graphics _graphics;
        private readonly Rectangle _bounds;
        private readonly DropDownState _state;

        public SelectionDrawAreaEventArgs(Graphics graphics, Rectangle rect)
        {
            _graphics = graphics;
            _bounds = rect;
        }

        public SelectionDrawAreaEventArgs(Graphics graphics, Rectangle rect, DropDownState state)
        {
            _graphics = graphics;
            _bounds = rect;
            _state = state;
        }

        public Rectangle Bounds
        {
            get
            {
                return _bounds;
            }
        }

        public Graphics Graphics
        {
            get
            {
                return _graphics;
            }
        }

        public DropDownState State
        {
            get
            {
                return _state;
            }
        }
    }
}
