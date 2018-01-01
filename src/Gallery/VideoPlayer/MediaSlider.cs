using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace Gallery
{

    public class MediaSlider : Slider
    {
        public Thumb horizontalThumb;
        private FrameworkElement horizontalLeftTrack;
        private FrameworkElement horizontalRightTrack;
        private double oldValue = 0, newValue = 0, prevNewValue = 0;
        public event RoutedPropertyChangedEventHandler<double> MyValueChanged;
        public event RoutedPropertyChangedEventHandler<double> MyValueChangedInDrag;
        private DispatcherTimer dragtimer = new DispatcherTimer();
        private double dragTimeElapsed = 0;
        private const short DragWaitThreshold = 200, DragWaitInterval = 100;
        public Rectangle progressRect = null;
        private bool dragSeekJustFired = false;

        public MediaSlider()
        {

            this.ValueChanged += new RoutedPropertyChangedEventHandler<double>(CustomSlider_ValueChanged);
            dragtimer.Interval = new TimeSpan(0, 0, 0, 0, DragWaitInterval);
            dragtimer.Tick += new EventHandler(dragtimer_Tick);
        }

        void dragtimer_Tick(object sender, EventArgs e)
        {
            dragTimeElapsed += DragWaitInterval;

            if (dragTimeElapsed >= DragWaitThreshold)
            {
                RoutedPropertyChangedEventHandler<double> handler = MyValueChangedInDrag;
                
                if ((handler != null) && (newValue != prevNewValue))
                {
                    handler(this, new RoutedPropertyChangedEventArgs<double>(oldValue, newValue));
                    dragSeekJustFired = true;
                    prevNewValue = newValue;
                }

                dragTimeElapsed = 0;
            }
        }

        void CustomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            oldValue = e.OldValue;
            newValue = e.NewValue;

            if (horizontalThumb.IsDragging)
            {
                dragTimeElapsed = 0;
                dragtimer.Stop();
                dragtimer.Start();
                dragSeekJustFired = false;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            horizontalThumb = GetTemplateChild("HorizontalThumb") as Thumb;
            horizontalLeftTrack = GetTemplateChild("LeftTrack") as FrameworkElement;
            horizontalRightTrack = GetTemplateChild("RightTrack") as FrameworkElement;
            progressRect = GetTemplateChild("Progress") as Rectangle;

            if (horizontalLeftTrack != null) horizontalLeftTrack.MouseLeftButtonDown += new MouseButtonEventHandler(OnMoveThumbToMouse);

            if (horizontalRightTrack != null) horizontalRightTrack.MouseLeftButtonDown += new MouseButtonEventHandler(OnMoveThumbToMouse);

            horizontalThumb.DragCompleted += new DragCompletedEventHandler(DragCompleted);

            progressRect.Width = this.Width;
        }

        public Storyboard ProgressStoryboard { get { return (GetTemplateChild("ProgressStoryboard") as Storyboard); } }

        public Rectangle ProgressBar { get { return (GetTemplateChild("Progress") as Rectangle); } }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Size s = base.ArrangeOverride(finalSize);

            if (double.IsNaN(horizontalThumb.Width) && (horizontalThumb.ActualWidth != 0))
            {
                horizontalThumb.Width = horizontalThumb.ActualWidth;
            }

            if (double.IsNaN(horizontalThumb.Height) && (horizontalThumb.ActualHeight != 0))
            {
                horizontalThumb.Height = horizontalThumb.ActualHeight;
            }

            if (double.IsNaN(horizontalThumb.Width)) horizontalThumb.Width = horizontalThumb.Height;
            if (double.IsNaN(horizontalThumb.Height)) horizontalThumb.Height = horizontalThumb.Width;

            return (s);
        }
        
        private void OnMoveThumbToMouse(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point p = e.GetPosition(this);

            if (this.Orientation == Orientation.Horizontal)
            {
                Value = (p.X - (horizontalThumb.ActualWidth / 2)) / (ActualWidth - horizontalThumb.ActualWidth) * Maximum;
            }

            RoutedPropertyChangedEventHandler<double> handler = MyValueChanged;

            if (handler != null)
            {
                handler(this, new RoutedPropertyChangedEventArgs<double>(oldValue, Value));
            }
        }

        private void DragCompleted(object sender, DragCompletedEventArgs e)
        {
            dragtimer.Stop();
            dragTimeElapsed = 0;

            RoutedPropertyChangedEventHandler<double> handler = MyValueChanged;

            if ((handler != null) && (!dragSeekJustFired))
            {
                handler(this, new RoutedPropertyChangedEventArgs<double>(oldValue, this.Value));
            }
        }
    }
}