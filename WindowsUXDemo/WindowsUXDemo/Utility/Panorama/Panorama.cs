using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Utility
{
    public class Panorama : ItemsControl
    {
        #region Property

        private FrameworkElement _AttachTo;
        public FrameworkElement AttachTo
        {
            get { return _AttachTo; }
            set
            {
                if (_AttachTo!=null)
                {
                    _AttachTo.Loaded -= _AttachTo_Loaded;
                }

                _AttachTo = value;

                if (_AttachTo!=null)
                {
                    _AttachTo.Loaded += _AttachTo_Loaded;
                }
            }
        }

        void _AttachTo_Loaded(object sender, RoutedEventArgs e)
        {
            if (AttachTo.GetType() == typeof(ScrollViewer))
            {
                _ScrollViewer = (ScrollViewer)AttachTo;
            }
            else
            {
                _ScrollViewer = AttachTo.GetFirstDescendantOfType<ScrollViewer>();
            }
            _ScrollViewer.ViewChanged += scrollViewer_ViewChanged;
        }

        #endregion

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            foreach (var item in this.Items)
            {
                EnsureTranform(item);
            }
        }

        ScrollViewer _ScrollViewer;
        private void scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var delta = (_ScrollViewer.HorizontalOffset / _ScrollViewer.ScrollableWidth) * 1200;
            UpdateOffset(delta);
        }

        private void EnsureTranform(object item)
        {
            FrameworkElement fe = item as FrameworkElement;
            if (fe==null)
            {
                return;
            }

            fe.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            fe.Margin = new Thickness(0, 0, -99999, 0);

            CompositeTransform transform = fe.RenderTransform as CompositeTransform;
            if (transform == null)
            {
                fe.RenderTransform = transform = new CompositeTransform();
                fe.RenderTransformOrigin = new Point(0.5d, 0.5d);
            }
        }

        private void UpdateOffset(double offset)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                FrameworkElement fe = this.Items[i] as FrameworkElement;
                CompositeTransform transform = fe.RenderTransform as CompositeTransform;
                transform.TranslateX = 0 - offset/Math.Pow(2,(this.Items.Count - i));
            }
        }
    }
}
