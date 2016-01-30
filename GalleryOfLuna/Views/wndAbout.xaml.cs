using System.Windows.Navigation;
using MahApps.Metro.Controls;
using System.Diagnostics;

namespace GalleryOfLuna.Views
{
    /// <summary>
    /// Interaction logic for wndAbout.xaml
    /// </summary>
    public partial class wndAbout : MetroWindow
    {
        public wndAbout()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
