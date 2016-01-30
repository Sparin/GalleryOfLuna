using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Input;

namespace GalleryOfLuna.Views
{
    /// <summary>
    /// Логика взаимодействия для wndFullsize.xaml
    /// </summary>
    public partial class wndFullsize : MetroWindow
    {
        public wndFullsize()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MetroWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
                flInformation.IsOpen = !flInformation.IsOpen;
        }

        private void MetroWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
