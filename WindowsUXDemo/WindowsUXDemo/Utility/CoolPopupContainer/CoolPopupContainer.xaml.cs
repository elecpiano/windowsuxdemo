using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utility.Animations;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace WindowsUXDemo.Utility
{
    public sealed partial class CoolPopupContainer : UserControl
    {
        public CoolPopupContainer()
        {
            this.InitializeComponent();
            this.IsHitTestVisible = false;
            this.Opacity = 0;
        }

        #region Expand / Hide

        FrameworkElement CurrentlyShownTarget = null;

        double previousScaleX = 0d;
        double previousScaleY = 0d;
        double currentScaleX = 1.0d;
        double currentScaleY = 1.0d;

        double previousPositionX = 0d;
        double previousPositionY = 0d;
        double currentPositionX = 0d;
        double currentPositionY = 0d;
        TimeSpan showDuration = TimeSpan.FromSeconds(0.5);
        TimeSpan hideDuration = TimeSpan.FromSeconds(0.5);

        private bool isHidding = false;

        public void Show(FrameworkElement content, object from = null)
        {
            contentArea.Children.Clear();
            contentArea.Children.Add(content);

            FrameworkElement feFrom = from as FrameworkElement;
            if (feFrom == null)
            {
                Show();
            }
            else
            {
                previousScaleX = feFrom.Width / content.Width;
                previousScaleY = feFrom.Height / content.Height;

                ShowFrom(feFrom);
            }
        }

        private void ShowFrom(FrameworkElement targetElement)
        {
            CurrentlyShownTarget = targetElement;
            CurrentlyShownTarget.Opacity = 0;

            var transform = targetElement.TransformToVisual(null);
            var point = transform.TransformPoint(new Point(0, 0));

            previousPositionX = point.X + targetElement.ActualWidth / 2 - Window.Current.Bounds.Width / 2;
            previousPositionY = point.Y + targetElement.ActualHeight / 2 - Window.Current.Bounds.Height / 2;

            PlayExpandAnimation();
        }

        private void Show()
        {
            previousPositionX = 0;
            previousPositionY = 0 - Window.Current.Bounds.Height / 2;

            PlayExpandAnimation();
        }

        private void PlayExpandAnimation()
        {
            this.Opacity = 1;
            this.IsHitTestVisible = true;
            currentPositionX = 0;
            currentPositionY = 0;
            MoveAnimation.MoveFromTo(contentArea, previousPositionX, previousPositionY, currentPositionX, currentPositionY, showDuration, null);
            ScaleAnimation.ScaleFromTo(contentArea, previousScaleX, previousScaleY, currentScaleX, currentScaleY, showDuration,null);
            //FadeAnimation.Fade(contentArea, 0, 1, showDuration, null);
            FadeAnimation.Fade(mask, 0, 1, showDuration, ExpandStory_Completed);
        }

        private void ExpandStory_Completed(FrameworkElement fe)
        {
            RegisterKeyboardEvents();
            isHidding = false;
        }

        public void Hide()
        {
            PlayHideAnimation();
        }

        public void HideByDropping()
        {
            previousPositionX = 0;
            previousPositionY = Window.Current.Bounds.Height / 2;
            CurrentlyShownTarget = null;
            PlayHideAnimation();
        }

        public void HideToCenter()
        {
            previousPositionX = 0;
            previousPositionY = 0;
            CurrentlyShownTarget = null;
            PlayHideAnimation();
        }

        private void PlayHideAnimation()
        {
            if (isHidding)
            {
                return;
            }
            isHidding = true;

            this.IsHitTestVisible = false;
            MoveAnimation.MoveFromTo(contentArea, currentPositionX, currentPositionY, previousPositionX, previousPositionY, hideDuration, null);
            ScaleAnimation.ScaleFromTo(contentArea, currentScaleX, currentScaleY, previousScaleX, previousScaleY, hideDuration,
              fe =>
              {
                  this.Opacity = 0;
                  if (CurrentlyShownTarget != null)
                  {
                      CurrentlyShownTarget.Opacity = 1;
                      CurrentlyShownTarget = null;
                  }
              });
            //FadeAnimation.Fade(contentArea, 1, 0, hideDuration, null);
            FadeAnimation.Fade(mask, 1, 0, hideDuration, HideStory_Completed);
        }

        private void HideStory_Completed(FrameworkElement fe)
        {
            
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void mask_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Hide();
        }

        private void RootGrid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (e.Cumulative.Scale < 0.8f)
            {
                Hide();
            }
        }

        CoreDispatcher dispatcher
        {
            get
            {
                return Window.Current.CoreWindow.Dispatcher;
            }
        }

        private void RegisterKeyboardEvents()
        {
            dispatcher.AcceleratorKeyActivated += dispatcher_AcceleratorKeyActivated;
        }

        void dispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            VirtualKey key = args.VirtualKey;
            if ((args.EventType == CoreAcceleratorKeyEventType.KeyDown) && (args.VirtualKey == VirtualKey.Escape))
            {
                Hide();
                UnRegisterKeyboardEvents();
                args.Handled = true;
            }
        }

        private void UnRegisterKeyboardEvents()
        {
            dispatcher.AcceleratorKeyActivated -= dispatcher_AcceleratorKeyActivated;
        }

        #endregion

    }
}
