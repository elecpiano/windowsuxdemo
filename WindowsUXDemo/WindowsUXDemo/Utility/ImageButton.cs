using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Utility
{
    public class ImageButton : Button
    {
        #region Properties

        public ImageSource BackgroundNormal
        {
            get { return (ImageSource)GetValue(BackgroundNormalProperty); }
            set
            {
                SetValue(BackgroundNormalProperty, value);
            }
        }

        public static readonly DependencyProperty BackgroundNormalProperty =
            DependencyProperty.Register("BackgroundNormal", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));

        public ImageSource BackgroundPressed
        {
            get { return (ImageSource)GetValue(BackgroundPressedProperty); }
            set
            {
                SetValue(BackgroundPressedProperty, value);
            }
        }

        public static readonly DependencyProperty BackgroundPressedProperty =
            DependencyProperty.Register("BackgroundPressed", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));

        public ImageSource BackgroundHover
        {
            get { return (ImageSource)GetValue(BackgroundHoverProperty); }
            set
            {
                SetValue(BackgroundHoverProperty, value);
            }
        }

        public static readonly DependencyProperty BackgroundHoverProperty =
            DependencyProperty.Register("BackgroundHover", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));

        public ImageSource BackgroundDisabled
        {
            get { return (ImageSource)GetValue(BackgroundDisabledProperty); }
            set
            {
                SetValue(BackgroundDisabledProperty, value);
            }
        }

        public static readonly DependencyProperty BackgroundDisabledProperty =
            DependencyProperty.Register("BackgroundDisabled", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));

        private Storyboard storyPressed;

        #endregion

        #region Lifecycle

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var _layoutRoot = GetTemplateChild("root") as Grid;
            if (_layoutRoot!=null && _layoutRoot.Resources.ContainsKey("storyPressed"))
            {
                storyPressed = _layoutRoot.Resources["storyPressed"] as Storyboard;
            }
        }

        protected override void OnPointerPressed(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (storyPressed!=null)
            {
                storyPressed.Begin();
            }
            base.OnPointerPressed(e);
        }

        #endregion

    }
}
