using System;
using System.Drawing;

namespace Ekstrand.Windows.Forms
{
    public class DrawTextAreaEventArgs : EventArgs, IDisposable
    {
        private Graphics _graphics = null;
        private readonly Rectangle _textRec;

        public DrawTextAreaEventArgs(Graphics graphics, Rectangle clipRect)
        {
            if (graphics == null)
            {
                throw new ArgumentNullException("Graphics");
            }

            if(clipRect == null)
            {
                throw new ArgumentNullException("Rectangle");
            }

            this._graphics = graphics;
            this._textRec = clipRect;
        }

        ~DrawTextAreaEventArgs()
        {
            Dispose(false);
        }

        public Rectangle TextArea
        {
            get { return _textRec; }
        }

        public Graphics Graphics
        {
            get { return _graphics; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //only dispose the graphics object if we created it via the dc.
                if (_graphics != null)
                {
                    _graphics.Dispose();
                }
            }
        }
    }
}
