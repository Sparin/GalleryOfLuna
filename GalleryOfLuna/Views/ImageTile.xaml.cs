using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GalleryOfLuna.Views
{
    /// <summary>
    /// Логика взаимодействия для ImageTile.xaml
    /// </summary>
    public partial class ImageTile : UserControl
    {
        public string CachePath { get; set; }
        public Uri DownloadUri { get; set; }
        private WebClient imageDownloader = new WebClient();

        public ImageTile()
        {
            InitializeComponent();
        }

        private void extMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            extMenu.Visibility = System.Windows.Visibility.Visible;
            Image.StartAnimate();
        }

        private void extMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            extMenu.Visibility = System.Windows.Visibility.Hidden;
            Image.StopAnimate();
        }

        private void ImageTile_Loaded(object sender, RoutedEventArgs e)
        {
            if (System.IO.Path.GetExtension(CachePath) != ".gif")
                gifMark.Visibility = Visibility.Collapsed;

            if (CachePath != null)
            {
                imageDownloader.DownloadFileCompleted += (obj, args) =>
                {
                    if (args.Error != null || args.Cancelled)
                    {
                        if (File.Exists(CachePath))
                        {
                            File.Delete(CachePath);
                            Console.WriteLine("WebClient's callback delete a broken image: {0}", CachePath);
                        }
                        Console.WriteLine(args.Error.Message);
                    }
                    else
                        Image.ChangeImage(CachePath);
                };
                if (!File.Exists(CachePath))
                {
                    Image.ChangeImageToText("Loading...",12);
                    App.ClearCacheFolder();
                    imageDownloader.DownloadFileAsync(DownloadUri, CachePath);                    
                }
                else
                    Image.ChangeImage(CachePath);
            }
        }

        private void ImageTile_Unloaded(object sender, RoutedEventArgs e)
        {
            Image.StopAnimate();
            imageDownloader.CancelAsync();
            imageDownloader.Dispose();
        }
    }
}
