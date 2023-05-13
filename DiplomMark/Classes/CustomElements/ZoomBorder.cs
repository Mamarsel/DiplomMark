using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace DiplomMark.Classes.CustomElements
{
    public class ZoomBorder : Border
    {
        /// <summary>
        /// Собственный элемент для зумирования и панаромирования элемента. Работает с помощью колесика мыши и движение мыши с зажатым ctrl
        /// </summary>
        private UIElement child = null;
        private Point origin;
        private Point start;

        private TranslateTransform _getTranslateTransform(UIElement element)
        {
            return (TranslateTransform)((TransformGroup)element.RenderTransform)
              .Children.First(tr => tr is TranslateTransform);
        }

        private ScaleTransform _getScaleTransform(UIElement element)
        {
            return (ScaleTransform)((TransformGroup)element.RenderTransform)
              .Children.First(tr => tr is ScaleTransform);
        }

        public override UIElement Child
        {
            get { return base.Child; }
            set
            {
                if (value != null && value != Child)
                    Initialize(value);
                base.Child = value;
            }
        }

        public void Initialize(UIElement element)
        {
            child = element;
            if (child != null)
            {
                TransformGroup group = new TransformGroup();
                ScaleTransform st = new ScaleTransform();
                group.Children.Add(st);
                TranslateTransform tt = new TranslateTransform();
                group.Children.Add(tt);
                child.RenderTransform = group;
                child.RenderTransformOrigin = new Point(0.0, 0.0);
                MouseWheel += child_MouseWheel;
                MouseLeftButtonDown += child_MouseLeftButtonDown;
                MouseLeftButtonUp += child_MouseLeftButtonUp;
                MouseMove += child_MouseMove;
                PreviewMouseRightButtonDown += new MouseButtonEventHandler(
              child_PreviewMouseRightButtonDown);
            }
        }

        public void Reset()
        {
            if (child != null)
            {
                // reset zoom
                var st = _getScaleTransform(child);
                st.ScaleX = 1.0;
                st.ScaleY = 1.0;

                // reset pan
                var tt = _getTranslateTransform(child);
                tt.X = 0.0;
                tt.Y = 0.0;
            }
        }

        #region Child Events
        double zoomCorrected;
        private void child_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (child != null)
            {
                var st = _getScaleTransform(child);
                var tt = _getTranslateTransform(child);

                double zoom = e.Delta > 0 ? .2 : -.2;
                if (!(e.Delta > 0) && (st.ScaleX < .2 || st.ScaleY < .2))
                    return;

                Point relative = e.GetPosition(child);
                double absoluteX;
                double absoluteY;

                absoluteX = relative.X * st.ScaleX + tt.X;
                absoluteY = relative.Y * st.ScaleY + tt.Y;
                zoomCorrected = zoom * st.ScaleX;
                st.ScaleX += zoomCorrected;
                st.ScaleY += zoomCorrected;


                tt.X = absoluteX - relative.X * st.ScaleX;
                tt.Y = absoluteY - relative.Y * st.ScaleY;
            }
        }
        private void child_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (child != null)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) && CustomCanvas.selectedRectangle == null)
                {
                    var tt = _getTranslateTransform(child);
                    start = e.GetPosition(this);
                    origin = new Point(tt.X, tt.Y);
                    Cursor = Cursors.Hand;
                    child.CaptureMouse();
                }
            }
        }

        private void child_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (child != null)
            {
                child.ReleaseMouseCapture();
                Cursor = Cursors.Arrow;
            }
        }

        void child_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Reset();
        }

        private void child_MouseMove(object sender, MouseEventArgs e)
        {
            if (child != null)
            {
                if (child.IsMouseCaptured)
                {
                    var tt = _getTranslateTransform(child);
                    Vector v = start - e.GetPosition(this);
                    tt.X = origin.X - v.X;
                    tt.Y = origin.Y - v.Y;
                }
            }
        }

        #endregion
    }
}
