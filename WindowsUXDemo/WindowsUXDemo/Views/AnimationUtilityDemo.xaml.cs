using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Utility.Animations;
using System;

namespace WindowsUXDemo.Views
{
    public sealed partial class AnimationUtilityDemo : Page
    {
        public AnimationUtilityDemo()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ShowPageTitle.Begin();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void moveFromTo_Click(object sender, RoutedEventArgs e)
        {
            MoveAnimation.MoveFromTo(ellipse, 0, 0, 300, 300, TimeSpan.FromSeconds(1), null);
        }

        private void moveTo_Click(object sender, RoutedEventArgs e)
        {
            MoveAnimation.MoveTo(ellipse, -300, -300, TimeSpan.FromSeconds(0.5), null);
        }

        private void moveBy_Click(object sender, RoutedEventArgs e)
        {
            MoveAnimation.MoveBy(ellipse, 100, 100, TimeSpan.FromSeconds(0.3), null);
        }

        private void moveCombo_Click(object sender, RoutedEventArgs e)
        {
            MoveAnimation.MoveFromTo(ellipse, -200, 200, -200, -200, TimeSpan.FromSeconds(0.5),
                fe1 =>
                {
                    MoveAnimation.MoveTo(ellipse, 200, -200, TimeSpan.FromSeconds(0.5),
                        fe2 =>
                        {
                            MoveAnimation.MoveTo(ellipse, 200, 200, TimeSpan.FromSeconds(0.5),
                                fe3 =>
                                {
                                    MoveAnimation.MoveTo(ellipse, -200, 200, TimeSpan.FromSeconds(0.5), null);
                                });
                        });
                });

            MoveAnimation.MoveFromTo(ellipse1, -310, -800, -310, 800, TimeSpan.FromSeconds(1), null);
            MoveAnimation.MoveFromTo(ellipse2, 0, 800, 0, -800, TimeSpan.FromSeconds(1), null);
            MoveAnimation.MoveFromTo(ellipse3, 310, -800, 310, 800, TimeSpan.FromSeconds(1), null);
        }

        private void scaleFromTo_Click(object sender, RoutedEventArgs e)
        {
            ScaleAnimation animation = new ScaleAnimation();
            animation.InstanceScaleFromTo(ellipse, 0, 0, 1, 1, TimeSpan.FromSeconds(0.5), null);
        }

        private void scaleTo_Click(object sender, RoutedEventArgs e)
        {
            ScaleAnimation.ScaleTo(ellipse, 2.5, 1.5, TimeSpan.FromSeconds(0.3), null);
        }

        private void fade_Click(object sender, RoutedEventArgs e)
        {
            FadeAnimation.Fade(ellipse, 0, 1, TimeSpan.FromSeconds(1), null);
        }

        private void vibrateButton_Click(object sender, RoutedEventArgs e)
        {
            VibrateAnimation.Vibrate(ellipse, null);
        }


    }
}
