using System;
using System.Drawing;

namespace Ekstrand.Windows.Forms
{
    /// <summary>
    /// Provides data for the Paint Text Area event.
    /// </summary>
    public class DrawTextAreaEventArgs : EventArgs, IDisposable
    {
        #region Fields

        private readonly Rectangle _textRec;
        private Graphics _graphics = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DrawTextAreaEventArgs class with the specified graphics and clipping rectangle.
        /// </summary>
        /// <param name="graphics">The Graphics used to paint the item.</param>
        /// <param name="clipRect">The Rectangle that represents the rectangle in which to paint.</param>
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

        #endregion Constructors

        #region Destructors

        /// <summary>
        /// Destructor for class
        /// </summary>
        ~DrawTextAreaEventArgs()
        {
            Dispose(false);
        }

        #endregion Destructors

        #region Properties

        /// <summary>
        /// Gets the graphics used to paint.
        /// </summary>
        public Graphics Graphics
        {
            get { return _graphics; }
        }

        /// <summary>
        /// Gets the rectangle in which to paint.
        /// </summary>
        public Rectangle TextArea
        {
            get { return _textRec; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Releases all resources used by the DrawTextAreaEventArgs.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the DrawTextAreaEventArgs and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">Boolean true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
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

        #endregion Methods
    }
}
