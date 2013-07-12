using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Utility
{
    public class DrawableComponent : ContentControl
    {
        #region Properties

        public Rect WhereItShouldBe;
        public Rect CurrentPosition;
        public Rect OriginalPosition;

        #endregion
    }

}
