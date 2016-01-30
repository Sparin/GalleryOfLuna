using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GalleryOfLuna.Behaviors
{
    class TransformationLogic
    {
        Border MagicalNumberOfLogic;

        public TransformationLogic(Border borderOfControl)
        {
            if (borderOfControl == null)
                throw new NullReferenceException();
            else
                MagicalNumberOfLogic = borderOfControl;
        }

        private const double StepSize = .2;

        private static double StepToOriginal(double currentOffset, double currentScale, double stepSize)
        {
            var steps = Math.Round((currentScale - 1) / stepSize);
            return currentOffset / (steps < 1 ? 1 : steps);
        }

        public void controlMouseWheel(object sender, MouseWheelEventArgs args)
        {
            var img = sender as Image;
            if (img == null) return;
            var transform = (TransformGroup)img.RenderTransform;
            foreach (var child in transform.Children.OfType<ScaleTransform>())
            {
                var delta = args.Delta > 0 ? StepSize : -StepSize;
                var newScale = child.ScaleX + delta;
                if (newScale >= 1)
                {
                    child.ScaleX = newScale;
                    child.ScaleY = newScale;
                    ((ImageParams)img.Tag).Scale = newScale;
                }
            }
            foreach (var translate in transform.Children.OfType<TranslateTransform>())
            {
                var scale = ((ImageParams)img.Tag).Scale;
                var relY = (img.ActualHeight * scale - img.ActualHeight) / 2;
                var relX = (img.ActualWidth * scale - MagicalNumberOfLogic.ActualWidth) / 2;
                if (translate.Y < -relY || translate.Y > relY)
                {
                    var step = StepToOriginal(translate.Y, scale, StepSize);
                    translate.Y -= step;
                }
                if (translate.X < -relX || translate.X > relX)
                {
                    var step = StepToOriginal(translate.X, scale, StepSize);
                    translate.X -= step;
                }
            }
        }

        public void controlMouseLeftButtonDown(object sender, MouseButtonEventArgs args)
        {
            var img = sender as Image;
            if (img == null) return;
            ((ImageParams)img.Tag).MousePoint = args.GetPosition(null);
            var transform = (TransformGroup)img.RenderTransform;
            foreach (var translate in transform.Children.OfType<TranslateTransform>())
            {
                ((ImageParams)img.Tag).InitialPoint.Y = translate.Y;
                ((ImageParams)img.Tag).InitialPoint.X = translate.X;
            }
            img.CaptureMouse();
        }

        public void controlMouseMove(object sender, MouseEventArgs args)
        {
            var img = sender as Image;
            if (img == null || !img.IsMouseCaptured) return;
            var transform = (TransformGroup)img.RenderTransform;
            foreach (var translate in transform.Children.OfType<TranslateTransform>())
            {
                var y = ((ImageParams)img.Tag).InitialPoint.Y + (args.GetPosition(null).Y - ((ImageParams)img.Tag).MousePoint.Y);
                var x = ((ImageParams)img.Tag).InitialPoint.X + (args.GetPosition(null).X - ((ImageParams)img.Tag).MousePoint.X);
                var scale = ((ImageParams)img.Tag).Scale;
                var relY = img.ActualHeight * scale - img.ActualHeight;
                var relX = img.ActualWidth * scale - MagicalNumberOfLogic.ActualWidth;
                if (y > -relY / 2 && y < relY / 2)
                {
                    translate.Y = y;
                }
                if (x > -relX / 2 && x < relX / 2)
                {
                    translate.X = x;
                }
            }
        }

        public void controlMouseLeftButtonUp(object sender, MouseButtonEventArgs args)
        {
            (sender as Image)?.ReleaseMouseCapture();
        }
    }

    public class ImageParams
    {
        public Point MousePoint;
        public Point InitialPoint;
        public double Scale;

        public ImageParams(double scale, Point initialPoint, Point mousePoint)
        {
            Scale = scale;
            InitialPoint = initialPoint;
            MousePoint = mousePoint;
        }

    }
}
