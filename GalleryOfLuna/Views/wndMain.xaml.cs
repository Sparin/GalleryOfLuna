using System.Windows;
using GalleryOfLuna.ViewModel;
using MahApps.Metro.Controls;
using GalleryOfLuna.Model;

namespace GalleryOfLuna.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class wndMain : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public wndMain()
        {
            InitializeComponent();
            DataContext = new wndMainViewModel();
        }

        private void miOpenExplorer_Click(object sender, RoutedEventArgs e)
        {
            foreach (object item in dtrGrid.SelectedItems)
                System.Diagnostics.Process.Start("explorer", "/select," + ((DownloadQueryItem)item).Destination);
        }

        private void miRepeat_Click(object sender, RoutedEventArgs e)
        {
            foreach (object item in dtrGrid.SelectedItems)
                ((DownloadQueryItem)item).Repeat();
        }

        private void miCancel_Click(object sender, RoutedEventArgs e)
        {
            foreach (object item in dtrGrid.SelectedItems)
                ((DownloadQueryItem)item).Cancel();
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            scrWorkspace.ScrollToTop();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            new wndAbout().ShowDialog();
        }
    }
}