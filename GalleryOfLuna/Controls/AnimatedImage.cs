using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GalleryOfLuna.Controls
{
    public class AnimatedImage : System.Windows.Controls.Image
    {

        private Bitmap _bitmap;// Local bitmap member to cache image resource
        public bool AutoStartAnimation = false;
        public delegate void FrameUpdatedEventHandler();

        /// <summary>
        /// Override the OnInitialized method
        /// </summary>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.Unloaded += extImage_Unloaded;
        }

        public void ChangeImageToText(string text, int size)
        {
            Stretch = System.Windows.Media.Stretch.None;
            Bitmap bmp = new Bitmap(100, 40);
            RectangleF rectf = new RectangleF(15, 15, 100, 40);
            Graphics g = Graphics.FromImage(bmp);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString(text, new Font("Fixedsys", size), Brushes.DarkGreen, rectf);
            g.Flush();

            _bitmap = bmp;
            Source = GetBitmapSource();
        }

        public void ChangeImage(string path)
        {
            StopAnimate();
            Image img = null;
            using (Stream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                img = Image.FromStream(stream, false, false);

                if (img.Size.Width > System.Windows.SystemParameters.PrimaryScreenWidth || img.Size.Height > System.Windows.SystemParameters.PrimaryScreenHeight)
                    Stretch = System.Windows.Media.Stretch.Uniform;
                else
                    Stretch = System.Windows.Media.Stretch.None;

                if (System.IO.Path.GetExtension(path) != ".gif")
                    _bitmap = new Bitmap(img);
                else
                    _bitmap = new Bitmap(path);

                Source = GetBitmapSource();
                if (AutoStartAnimation)
                {
                    StartAnimate();
                }
            }
        }

        /// <summary>
        /// Close the FileStream to unlock the GIF file
        /// </summary>
        private void extImage_Unloaded(object sender, RoutedEventArgs e)
        {
            StopAnimate();
            if (_bitmap != null)
                _bitmap.Dispose();
        }

        /// <summary>
        /// Start animation
        /// </summary>
        public void StartAnimate()
        {
            ImageAnimator.Animate(_bitmap, OnFrameChanged);
        }

        /// <summary>
        /// Stop animation
        /// </summary>
        public void StopAnimate()
        {
            ImageAnimator.StopAnimate(_bitmap, OnFrameChanged);
        }

        /// <summary>
        /// Event handler for the frame changed
        /// </summary>
        private void OnFrameChanged(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                   new FrameUpdatedEventHandler(FrameUpdatedCallback));
        }

        private void FrameUpdatedCallback()
        {
            ImageAnimator.UpdateFrames();
            Source?.Freeze();
            // Convert the bitmap to BitmapSource that can be display in WPF Visual Tree
            Source = GetBitmapSource();
            InvalidateVisual();
        }

        private BitmapSource GetBitmapSource()
        {
            using (MemoryStream memory = new MemoryStream())
            {
                _bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }

    }
    internal static class SafeNativeMethods
    {
        /// <summary>
        /// Delete local bitmap resource
        /// Reference: http://msdn.microsoft.com/en-us/library/dd183539(VS.85).aspx
        /// </summary>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool DeleteObject(IntPtr hObject);
    }
}
