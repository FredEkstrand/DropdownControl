using System;
using System.Text;
using System.Windows.Forms.Design;

namespace Ekstrand.Windows.Forms
{
    /*
     * Change the design mode behavior of the DropDownControl
     * in the designer
     */
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
