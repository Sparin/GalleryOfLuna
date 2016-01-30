using Apex.MVVM;
using GalleryOfLuna.Commands;
using GalleryOfLuna.Model;
using GalleryOfLuna.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace GalleryOfLuna.ViewModel
{
    internal class wndMainViewModel : INotifyPropertyChanged
    {
        public enum SizeOfImages { thumb_tiny, thumb_small, thumb, small, medium, large, tall, full };

        public ObservableCollection<DownloadQueryItem> DownloadsCollection { get; private set; }
        public ObservableCollection<UIElement> ImageCollection { get; private set; }
        public List<ImageTileViewModel> ImageViewModelCollection { get; private set; }
        public ObservableCollection<string> ThumbnailSizeCollection { get; private set; }
        public ObservableCollection<string> HighResolutionSizeCollection { get; private set; }

        private int _thumbnailSize = 2;
        public int ThumbnailSize
        {
            get
            {
                return _thumbnailSize;
            }
            set
            {
                _thumbnailSize = value;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs("ThumbnailSize"));
            }
        }

        private int _languageCollectionIndex = 0;
        public int LanguageCollectionIndex
        {
            get
            {
                return _languageCollectionIndex;
            }
            set
            {
                _languageCollectionIndex = value;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs("LanguageCollectionIndex"));
                }
            }
        }

        private int _highResolutionSize = 7;
        public int HighResolutionSize
        {
            get
            {
                return _highResolutionSize;
            }
            set
            {
                _highResolutionSize = value;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs("HighResolutionSize"));
            }
        }

        private string _informationLabel = "System Information Here";
        public string InformationLabel
        {
            get
            {
                return _informationLabel;
            }
            set
            {
                _informationLabel = value;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs("InformationLabel"));
            }
        }

        private string _searchQuery = string.Empty;
        public string SearchQuery
        {
            get
            {
                return _searchQuery;
            }
            set
            {
                _searchQuery = value;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs("SearchQuery"));
            }
        }

        private string _lastSearchQuery = string.Empty;
        public string LastSearchQuery
        {
            get
            {
                return _lastSearchQuery;
            }
            set
            {
                _lastSearchQuery = value;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs("LastSearchQuery"));
            }
        }

        public string[] LanguageCollection
        {
            get
            {
                List<string> collection = new List<string>();
                foreach (CultureInfo info in App.Languages)
                    collection.Add(info.EnglishName);
                return collection.ToArray();
            }
        }

        private string _apiKey = string.Empty;
        public string APIKey
        {
            get
            {
                return _apiKey;
            }
            set
            {
                _apiKey = value;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs("APIKey"));
            }
        }

        private string _pathForSavingImages = "";
        public string PathForSavingImages
        {
            get
            {
                return _pathForSavingImages;
            }
            set
            {
                _pathForSavingImages = value;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs("PathForSavingImages"));
            }
        }

        private int _searchPage = 1;
        public int SearchPage
        {
            get
            {
                return _searchPage;
            }
            set
            {
                _searchPage = value;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs("SearchPage"));
            }
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs("IsBusy"));
            }
        }

        private FolderBrowserDialog _fbDialog = new FolderBrowserDialog();
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand msgCommand { get; private set; }
        public ICommand UpdateImageCollectionCommand { get; private set; }
        public Command GetPathForSavingImagesCommand { get; private set; }
        public Command AddCurrectPageAsDQItemsCommand { get; private set; }
        public ICommand LoadConfigurationCommand { get; private set; }
        public ICommand SaveConfigurationCommand { get; private set; }
        public Command ChangeLocalizationCommand { get; private set; }
        public Command IncreasePageCommand { get; private set; }
        public Command DecreasePageCommand { get; private set; }
        public ICommand CallFullsizeModeCommand { get; private set; }

        public wndMainViewModel()
        {
            DownloadsCollection = new ObservableCollection<DownloadQueryItem>();
            ImageCollection = new ObservableCollection<UIElement>();
            HighResolutionSizeCollection = new ObservableCollection<string>();
            ThumbnailSizeCollection = new ObservableCollection<string>();
            ImageViewModelCollection = new List<ImageTileViewModel>();

            this.GetPathForSavingImagesCommand = new Command(GetPathForSavingImages);
            this.UpdateImageCollectionCommand = new GetImageCollectionCommand(this);
            this.AddCurrectPageAsDQItemsCommand = new Command(AddCurrectPageAsDQItems);
            this.LoadConfigurationCommand = new LoadConfigurationCommand(this);
            this.SaveConfigurationCommand = new SaveConfigurationCommand(this);
            this.ChangeLocalizationCommand = new Command(ChangeLocalization);
            this.IncreasePageCommand = new Command(IncreasePage);
            this.DecreasePageCommand = new Command(DecreasePage);
            this.CallFullsizeModeCommand = new CallFullsizeModeCommand(this, 0);

            if (PathForSavingImages == string.Empty && PathForSavingImages == null)
                _fbDialog.RootFolder = Environment.SpecialFolder.MyPictures;

            for (int i = 0; i < 8; i++)
            {
                HighResolutionSizeCollection.Add(((wndMainViewModel.SizeOfImages)i).ToString("F"));
                ThumbnailSizeCollection.Add(((wndMainViewModel.SizeOfImages)i).ToString("F"));
            }

            App.Language = GalleryOfLuna.Properties.Settings.Default.DefaultLanguage;
            _fbDialog.Description = App.CurrectLocalizationDictionary["locale_GetPathForSaving"] as string;

            for (int i = 0; i < App.Languages.Count; i++)
                if (App.Languages[i].DisplayName == App.Language.DisplayName)
                {
                    LanguageCollectionIndex = i;
                    break;
                }

            string cache_folder = Path.GetTempPath() + @"GalleryOfLuna\Cache\Derpibooru";
            if (!Directory.Exists(cache_folder))
                Directory.CreateDirectory(cache_folder);
        }

        private void ChangeLocalization()
        {
            App.Language = App.Languages[this.LanguageCollectionIndex];
            Properties.Settings.Default.DefaultLanguage = App.Languages[this.LanguageCollectionIndex];
            Properties.Settings.Default.Save();
        }

        private void AddCurrectPageAsDQItems()
        {
            foreach (UIElement element in ImageCollection)
                ((ImageTile)element).btnSave.Command.Execute(null);
        }

        private void DecreasePage()
        {
            if (SearchPage > 1)
                SearchPage--;
            this.InformationLabel = String.Format((App.CurrectLocalizationDictionary["locale_SelectPage"] as string), this.SearchPage);
        }

        private void IncreasePage()
        {
            SearchPage++;
            this.InformationLabel = String.Format((App.CurrectLocalizationDictionary["locale_SelectPage"] as string), this.SearchPage);
        }

        private void GetPathForSavingImages()
        {
            _fbDialog.ShowDialog();
            if (_fbDialog.SelectedPath != string.Empty)
                PathForSavingImages = _fbDialog.SelectedPath;
        }
    }
}