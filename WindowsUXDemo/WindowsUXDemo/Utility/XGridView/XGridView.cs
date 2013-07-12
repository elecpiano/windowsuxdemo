using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Utility
{
    public class XGridView : ItemsControl
    {
        #region Properties

        private Point? DraggingPointOffset;
        private Grid DragAgent;
        private XGridViewItem DragAgentContentPresenter;
        public ScrollViewer ScrollViewer;

        private bool _IsInSettingMode;
        public bool IsInSettingMode
        {
            get
            {
                return _IsInSettingMode;
            }
            set
            {
                _IsInSettingMode = value;
            }
        }

        #endregion

        #region ItemWidth / ItemHeight

        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register("ItemWidth", typeof(double), typeof(XGridView), new PropertyMetadata(120d));

        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeight", typeof(double), typeof(XGridView), new PropertyMetadata(120d));

        #endregion

        #region Constructor

        public XGridView()
        {
            this.AddHandler(UIElement.PointerPressedEvent, new PointerEventHandler(XGridView_PointerPressed), true);
            this.AddHandler(UIElement.PointerMovedEvent, new PointerEventHandler(XGridView_PointerMoved), true);
            this.AddHandler(UIElement.PointerReleasedEvent, new PointerEventHandler(XGridView_PointerReleased), true);

            this.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            this.ManipulationStarted += XGridView_ManipulationStarted;
            this.ManipulationDelta += XGridView_ManipulationDelta;
            this.ManipulationCompleted += XGridView_ManipulationCompleted;
        }

        #endregion

        #region Manipulation

        void XGridView_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {

        }

        void XGridView_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (_Panel == null)
            {
                return;
            }

            if (!this.IsInSettingMode)
            {
                return;
            }

            if (_Panel.IsDragging)
            {
                var transform = (CompositeTransform)DragAgent.RenderTransform;
                transform.TranslateX += e.Delta.Translation.X;
                transform.TranslateY += e.Delta.Translation.Y;
            }
        }

        void XGridView_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            
        }

        #endregion

        #region Pointer

        void XGridView_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (_Panel == null)
            {
                return;
            }

            if (!this.IsInSettingMode)
            {
                return;
            }

            try
            {
                Frame frame = Window.Current.Content as Frame;
                Point position = e.GetCurrentPoint(frame).Position;
                foreach (UIElement element in VisualTreeHelper.FindElementsInHostCoordinates(position, frame))
                {
                    if (element is XGridViewItem)
                    {
                        XGridViewItem item = element as XGridViewItem;
                        DraggingPointOffset = new Point?(e.GetCurrentPoint(element).Position);
                        StartDrag(item, e.GetCurrentPoint(this).Position);
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        void XGridView_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (_Panel != null && _Panel.IsDragging)
            {
                _Panel.UpdateDrag(e, DraggingPointOffset);
            }
        }

        void XGridView_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_Panel == null)
            {
                return;
            }

            if (_Panel.IsDragging)
            {
                StopDrag();
            }
        }

        #endregion

        #region Drag

        private int ItemIndexOnDragStart = 0;

        public void StartDrag(XGridViewItem item, Point point)
        {
            if (!IsInSettingMode)
            {
                return;
            }

            item.Opacity = 0;
            DragAgentContentPresenter.ContentTemplate = item.ContentTemplate;
            DragAgentContentPresenter.DataContext = item.DataContext;
            DragAgentContentPresenter.Width = item.ActualWidth;
            DragAgentContentPresenter.Height = item.ActualHeight;

            var transform = (CompositeTransform)DragAgent.RenderTransform;
            transform.TranslateX = point.X -DraggingPointOffset.Value.X;
            transform.TranslateY = point.Y -DraggingPointOffset.Value.Y;

            DragAgent.Visibility = Visibility.Visible;

            ItemIndexOnDragStart = _Panel.StartDragging(item);
        }

        void StopDrag()
        {
            int newIndex = _Panel.StopDragging();
            if (ItemIndexOnDragStart != newIndex && Reordered != null)
            {
                Reordered(this, DragAgentContentPresenter.DataContext, ItemIndexOnDragStart, newIndex);
            }

            DragAgentContentPresenter.DataContext = null;
            DragAgent.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Override

        protected override void OnApplyTemplate()
        {
            DragAgent = (Grid)GetTemplateChild("dragAgent");
            DragAgentContentPresenter = (XGridViewItem)GetTemplateChild("dragAgentContentPresenter");

            ScrollViewer = (ScrollViewer)GetTemplateChild("ScrollViewer");

            base.OnApplyTemplate();
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            XGridViewItem item = new XGridViewItem();
            RegisterItemEventHandlers(item);
            return item;
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            XGridViewItem visualItem = element as XGridViewItem;
            if (visualItem != null)
            {
                UnRegisterItemEventHandlers(visualItem);
                _Panel.RemoveItem(visualItem);
            }
        }

        protected override void OnItemsChanged(object e)
        {
            base.OnItemsChanged(e);
            if (_Panel != null)
            {
                _Panel.Init(true);
            }
        }

        #endregion

        #region Panel

        XGridViewPanel _Panel = null;
        public void RegisterPanel(XGridViewPanel panel)
        {
            _Panel = panel;
            //_Panel.AllowDrop = true;
        }

        #endregion

        #region Item Manipulation

        void RegisterItemEventHandlers(XGridViewItem item)
        {
            item.Tapped += item_Tapped;
        }

        void UnRegisterItemEventHandlers(XGridViewItem item)
        {
            item.Tapped -= item_Tapped;
        }

        void item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            XGridViewItem item = sender as XGridViewItem;
            if (item != null && ItemTapped != null)
            {
                ItemTapped(this, item, item.DataContext);
            }
        }

        #endregion

        #region Public Events

        public event XGridViewItemTappedEventHandler ItemTapped;
        public event XGridViewReorderEventHandler Reordered;

        #endregion

    }

    public delegate void XGridViewItemTappedEventHandler(object sender, object itemVisual, object itemData);
    public delegate void XGridViewReorderEventHandler(object sender, object item, int oldIndex, int newIndex);

}
