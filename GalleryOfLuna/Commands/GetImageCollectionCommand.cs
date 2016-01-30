using BooruonrailsAPI;
using GalleryOfLuna.ViewModel;
using GalleryOfLuna.Views;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace GalleryOfLuna.Commands
{
    class GetImageCollectionCommand : ICommand
    {
        private wndMainViewModel viewModel;

        private BooruonrailsClient b_clinet = new BooruonrailsClient("https://derpibooru.org/","");
        public Task TaskClientDownload { get; private set; }
        private CancellationTokenSource _ctsClientDownload = new CancellationTokenSource();

        /// <summary>
        /// Initializes a new instance of the GetImageCollectionCommand class.
        /// </summary>
        public GetImageCollectionCommand(wndMainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event System.EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (TaskClientDownload != null)
                if (!TaskClientDownload.IsCompleted)
                {
                    _ctsClientDownload.Cancel();
                    TaskClientDownload.Wait();
                }
            _ctsClientDownload = new CancellationTokenSource();
            TaskClientDownload = Task.Factory.StartNew(() =>
            {
                try
                {
                    viewModel.IsBusy = true;                    

                    if (viewModel.LastSearchQuery != viewModel.SearchQuery)
                    {
                        viewModel.LastSearchQuery = viewModel.SearchQuery;
                        viewModel.SearchPage = 1;
                    }
                    App.Current.Dispatcher.Invoke((Action)delegate()
                    {
                        foreach (UIElement ui in viewModel.ImageCollection)
                            ui.Visibility = System.Windows.Visibility.Collapsed;
                    });

                    b_clinet.KeyAPI = viewModel.APIKey;

                    //viewModel.InformationLabel = string.Format("Получение коллекции по поисковому запросу \"{0}\" на {1} странице", viewModel.SearchQuery, viewModel.SearchPage);
                    viewModel.InformationLabel = string.Format((App.CurrectLocalizationDictionary["locale_GetImageCollection"] as string), viewModel.SearchQuery, viewModel.SearchPage);
                    BooruonrailsImage[] imgs = b_clinet.GetImagesTaskAsync(viewModel.SearchQuery, viewModel.SearchPage, _ctsClientDownload.Token).Result;
                    //viewModel.SearchQuery, viewModel.SearchPage, _ctsClientDownload.Token).Result;
                    viewModel.InformationLabel = string.Format((App.CurrectLocalizationDictionary["locale_GotImageCollection"] as string), imgs.Length);

                    App.Current.Dispatcher.BeginInvoke((Action)delegate
                    {
                        viewModel.ImageCollection.Clear();
                        viewModel.ImageViewModelCollection.Clear();
                        foreach (BooruonrailsImage img in imgs)
                        {
                            ImageTileViewModel imgViewModel = new ImageTileViewModel(viewModel,viewModel.ImageViewModelCollection.Count);

                            imgViewModel.Upvotes = img.upvotes;
                            imgViewModel.Downvotes = img.downvotes;
                            imgViewModel.Description = img.description;
                            imgViewModel.Tags = img.tags;
                            imgViewModel.Score = img.score;
                            imgViewModel.Uploader = img.uploader;
                            imgViewModel.ID = img.id_number;

                            //imgViewModel.DownloadPath_Source = string.Format("{0}\\cache\\{1}\\{2}_{3}", AppDomain.CurrentDomain.BaseDirectory, cmbBoard.SelectedItem, img.id_number, System.IO.Path.GetExtension(img.representations.full));

                            imgViewModel.thumbnails.thumb_tiny = new Uri("http:" + img.representations.thumb_tiny);
                            imgViewModel.thumbnails.thumb_small = new Uri("http:" + img.representations.thumb_small);
                            imgViewModel.thumbnails.thumb = new Uri("http:" + img.representations.thumb);
                            imgViewModel.thumbnails.small = new Uri("http:" + img.representations.small);
                            imgViewModel.thumbnails.medium = new Uri("http:" + img.representations.medium);
                            imgViewModel.thumbnails.large = new Uri("http:" + img.representations.large);
                            imgViewModel.thumbnails.tall = new Uri("http:" + img.representations.thumb_small);
                            imgViewModel.thumbnails.full = new Uri("http:" + img.representations.full);

                            viewModel.ImageCollection.Add(new ImageTile
                            {
                                DataContext = imgViewModel,
                                CachePath = imgViewModel.ThumbnailCachePath,
                                DownloadUri = imgViewModel.ThumbnailUri
                            });
                            viewModel.ImageViewModelCollection.Add(imgViewModel);
                        }
                    });

                }
                catch (Exception ex)
                {
                    App.WriteMessage(ex, false);
                }
                finally
                {
                    viewModel.IsBusy = false;
                }
            });
        }
    }
}
