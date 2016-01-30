using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using GalleryOfLuna.Views;
using System.IO;

namespace GalleryOfLuna
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static List<CultureInfo> _languages = new List<CultureInfo>();
        public static ResourceDictionary CurrectLocalizationDictionary;
        public static bool IgnoreAvailable = false;
        public static bool ForceAnonymizer = false;

        public static void ClearCacheFolder()
        {
            string path = Path.GetTempPath()+ "GalleryOfLuna\\cache";
            foreach (string endCacheFolder in Directory.GetDirectories(path))
            {
                Console.WriteLine(new DirectoryInfo(endCacheFolder).EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length)/1024/1024);
                if (new DirectoryInfo(endCacheFolder).EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length) / 1024 / 1024 > 300)
                    foreach (string file in Directory.GetFiles(endCacheFolder))
                        try
                        {
                            File.Delete(file);
                        }
                        catch(Exception)
                        { }
            }
        }

        public static List<CultureInfo> Languages
        {
            get
            {
                return _languages;
            }
        }

        public App()
        {
            _languages.Clear();
            _languages.Add(new CultureInfo("en-US")); //Нейтральная культура для этого проекта
            _languages.Add(new CultureInfo("ru-RU"));
        }

        //http://habrahabr.ru/post/256193/
        public static CultureInfo Language
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;

                //1. Меняем язык приложения:
                System.Threading.Thread.CurrentThread.CurrentUICulture = value;

                //2. Создаём ResourceDictionary для новой культуры
                ResourceDictionary dict = new ResourceDictionary();
                switch (value.Name)
                {
                    case "ru-RU":
                        dict.Source = new Uri(String.Format("Resources/Localization.{0}.xaml", value.Name), UriKind.Relative);
                        break;
                    default:
                        dict.Source = new Uri("Resources/Localization.xaml", UriKind.Relative);
                        break;
                }

                //3. Находим старую ResourceDictionary и удаляем его и добавляем новую ResourceDictionary
                ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                              where d.Source != null && d.Source.OriginalString.StartsWith("Resources/Localization")
                                              select d).First();
                if (oldDict != null)
                {
                    int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                    Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(dict);                    
                }
                CurrectLocalizationDictionary = dict;
            }
        }

        public static void WriteMessage(Exception ex, bool critical)
        {
            string msg = string.Format("Type: {0}\r\nMessage: {1}\r\nStackTrace: {2}", ex.GetType(), ex.Message, ex.StackTrace);
            if (critical)
                MessageBox.Show(msg, "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Console.WriteLine("{0}: {1}",DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),msg);
        }
        public static void WriteMessage(string msg,bool critical)
        {
            if (critical)
                MessageBox.Show(msg, "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Console.WriteLine("{0}: {1}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"), msg);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            foreach (string arg in e.Args)
            {
                switch(arg)
                {
                    case "forceanonymizer":
                        ForceAnonymizer = true;
                        break;
                    case "ignoreavailable":
                        IgnoreAvailable = true;
                        break;
                }
            }
            base.OnStartup(e);
        }
    }
}
