using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using WindowsUXDemo;

namespace Utility
{
    public sealed class XGridViewPanel : Panel
    {
        #region Properties

        private App App { get { return App.Current as App; } }

        private XGridView Host = null;

        private List<XGridViewItem> _sortedChildren = new List<XGridViewItem>();
        private List<XGridViewItem> SortedChildren
        {
            get
            {
                return _sortedChildren;
            }
        }

        private bool _Registered = false;
        private bool PleaseReMeasureAll = false;

        public bool IsBusy
        {
            get
            {
                return Drawing;
            }
        }

        public DataTemplateSelector ItemTemplateSelector { get; set; }

        #endregion

        #region Constructor

        public XGridViewPanel()
        {
            this.Loaded += Panel_Loaded;
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        void Panel_Loaded(object sender, RoutedEventArgs e)
        {
            XGridView parent = UIHelper.FindAncestorOfType<XGridView>(this) as XGridView;
            if (parent != null && !_Registered)
            {
                parent.RegisterPanel(this);
                Host = parent;
                this.ItemTemplateSelector = parent.ItemTemplateSelector;
                _Registered = true;
                Init();
            }
        }

        private bool UseAnimationOnInit = false;
        public void Init(bool useAnimation = false)
        {
            SetRowCount();
            UseAnimationOnInit = useAnimation;
            this.MinWidth = ItemWidth;
            PleaseReMeasureAll = true;
            this.InvalidateMeasure();
        }

        #endregion

        #region Duration

        public TimeSpan Duration
        {
            get
            {
                return (TimeSpan)base.GetValue(DurationProperty);
            }
            set
            {
                base.SetValue(DurationProperty, value);
            }
        }

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan),
            typeof(XGridViewPanel), new PropertyMetadata(TimeSpan.FromMilliseconds(200.0)));

        #endregion

        #region ItemWidth / ItemHeight

        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register("ItemWidth", typeof(double), typeof(XGridViewPanel), new PropertyMetadata(220d));

        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeight", typeof(double), typeof(XGridViewPanel), new PropertyMetadata(220d));

        #endregion

        #region Timer: CompositionTarget_Rendering

        void CompositionTarget_Rendering(object sender, object e)
        {
            if (Drawing)
            {
                CompositionTarget_RenderingForDraw(sender, e);
            }
        }

        #endregion

        #region Override

        protected override Size MeasureOverride(Size availableSize)
        {
            if (PleaseReMeasureAll)
            {
                foreach (XGridViewItem child in this.Children)
                {
                    child.Measure(availableSize);
                    if (!this.SortedChildren.Contains(child))
                    {
                        this.SortedChildren.Add(child);
                    }
                }

                SetWhereTheyShouldBe();

                PleaseReMeasureAll = false;
            }

            ColumnCount = this.Children.Count / RowCount;
            Size size = new Size(ItemWidth * ColumnCount, ItemHeight * RowCount);
            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (lastDrawTime < drawUntil)
            {
                Drawing = true;
            }
            else
            {
                Drawing = false;
                if (UseAnimationOnInit)
                {
                    UseAnimationOnInit = false;
                    TryInvokeAnimation();
                }
                else
                {
                    foreach (DrawableComponent element in this.Children)
                    {
                        element.Arrange(element.WhereItShouldBe);
                        element.CurrentPosition = element.WhereItShouldBe;
                    }
                }
            }
            Size size = base.ArrangeOverride(finalSize);
            return size;
        }

        #endregion

        #region Position

        public int RowCount = 1;
        private int ColumnCount = 0;

        private void SetRowCount()
        {
            RowCount = (int)(Host.ActualHeight / ItemHeight);
        }

        private int GetColumn(int index)
        {
            return index / RowCount;
        }

        private int GetRow(int index)
        {
            return index % RowCount;
        }

        private void SetWhereTheyShouldBe()
        {
            foreach (var item in this.SortedChildren)
            {
                int index = this.SortedChildren.IndexOf(item);

                item.Row = GetRow(index);
                item.Column = GetColumn(index); 

                Point LT = new Point(ItemWidth * item.Column, ItemHeight * item.Row);
                Point RB = new Point(LT.X + ItemWidth, LT.Y + ItemHeight);
                Rect rect = new Rect(LT, RB);
                item.WhereItShouldBe = rect;
            }
        }

        private void CompositionTarget_RenderingForDraw(object sender, object e)
        {
            Draw();
            base.InvalidateArrange();
        }

        private void RearrangeByHoverring(XGridViewItem draggingItem, Point draggingCenterPoint)
        {
            int newRow = (int)(draggingCenterPoint.Y / ItemHeight);
            int newColumn = (int)(draggingCenterPoint.X / ItemWidth);

            newRow = newRow < 0 ? 0 : newRow;
            newColumn = newColumn < 0 ? 0 : newColumn;

            if (newRow != draggingItem.Row || newColumn != draggingItem.Column)
            {
                this.SortedChildren.Remove(draggingItem);
                int newIndex = RowCount * newColumn + newRow;
                newIndex = newIndex < (this.Children.Count-1) ? newIndex : (this.Children.Count - 1);
                this.SortedChildren.Insert(newIndex, draggingItem);
                TryInvokeAnimation();
            }
        }

        private void TryInvokeAnimation()
        {
            SetWhereTheyShouldBe();
            StartDraw();
        }

        private void StartDraw()
        {
            foreach (DrawableComponent item in this.SortedChildren)
            {
                item.OriginalPosition = item.CurrentPosition;
            }

            lastDrawTime = DateTime.Now;
            drawUntil = lastDrawTime + Duration;
            Drawing = true;
        }

        DateTime lastDrawTime = DateTime.MinValue;
        DateTime drawUntil = DateTime.MinValue;
        bool Drawing = false;
        private void Draw()
        {
            DateTime now = DateTime.Now;
            var interval = now - lastDrawTime;
            var factor = interval.TotalMilliseconds / Duration.TotalMilliseconds;

            foreach (DrawableComponent element in this.Children)
            {
                element.CurrentPosition.X += (element.WhereItShouldBe.X - element.OriginalPosition.X) * factor;
                element.CurrentPosition.Y += (element.WhereItShouldBe.Y - element.OriginalPosition.Y) * factor;
                element.Arrange(element.CurrentPosition);
            }

            lastDrawTime += interval;
        }

        #endregion

        #region Drag

        private XGridViewItem _DraggingItem;
        private Point DraggingCenterPoint;
        public bool IsDragging = false;

        public int StartDragging(XGridViewItem draggingItem)
        {
            _DraggingItem = draggingItem;
            _DraggingItem.SetValue(Canvas.ZIndexProperty, 99);
            IsDragging = true;

            return this.SortedChildren.IndexOf(draggingItem);
        }

        public void UpdateDrag(PointerRoutedEventArgs e, Point? draggingPointOffset)
        {
            if (_DraggingItem == null)
            {
                return;
            }

            Point position = e.GetCurrentPoint(this).Position;
            DraggingCenterPoint = new Point(position.X - draggingPointOffset.Value.X + ItemWidth / 2, position.Y - draggingPointOffset.Value.Y + ItemHeight / 2);
            RearrangeByHoverring(_DraggingItem, DraggingCenterPoint);
        }

        public int StopDragging()
        {
            this.ManipulationMode = ManipulationModes.System;

            if (_DraggingItem != null)
            {
                _DraggingItem.Opacity = 1;
            }

            int newIndex = this.SortedChildren.IndexOf(_DraggingItem);
            _DraggingItem = null;
            IsDragging = false;
            return newIndex;
        }

        #endregion

        #region Add / Remove

        public void RemoveItem(XGridViewItem item)
        {
            if (this.Children.Contains(item))
            {
                this.Children.Remove(item);
            }
            if (this.SortedChildren.Contains(item))
            {
                this.SortedChildren.Remove(item);
            }
        }

        #endregion

    }
}
