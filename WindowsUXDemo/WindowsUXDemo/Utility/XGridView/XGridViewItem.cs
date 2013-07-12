using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Utility
{
    public class XGridViewItem : DrawableComponent
    {
        #region Properties

        public int Row { get; set; }
        public int Column { get; set; }

        #endregion

        #region Override Methods

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #endregion

    }

}
