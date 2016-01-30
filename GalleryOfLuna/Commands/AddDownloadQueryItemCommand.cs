using GalleryOfLuna.Model;
using GalleryOfLuna.ViewModel;
using System;
using System.IO;
using System.Windows.Input;

namespace GalleryOfLuna.Commands
{
    class AddDownloadQueryItemCommand : ICommand
    {
        private ImageTileViewModel viewModel;
        private wndMainViewModel collectionHandler;

        /// <summary>
        /// Initializes a new instance of the AddDownloadQueryItemCommand class.
        /// </summary>
        public AddDownloadQueryItemCommand(ImageTileViewModel viewModel, wndMainViewModel collectionHandler)
        {
            this.viewModel = viewModel;
            this.collectionHandler = collectionHandler;
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
            try
            {
                string tempPath = string.Format(@"{0}\{1}{2}", collectionHandler.PathForSavingImages, viewModel.ID, Path.GetExtension(viewModel.thumbnails.full.AbsoluteUri));
                bool canToAdd = true;
                if (!File.Exists(tempPath))
                {
                    foreach (DownloadQueryItem item in collectionHandler.DownloadsCollection)
                        if (item.Destination == tempPath)
                            canToAdd = false;
                }
                else
                    canToAdd = false;
                if (canToAdd && collectionHandler.PathForSavingImages != string.Empty)
                {
                    if (!Directory.Exists(collectionHandler.PathForSavingImages))
                        Directory.CreateDirectory(collectionHandler.PathForSavingImages);
                    collectionHandler.DownloadsCollection.Add(new DownloadQueryItem(tempPath, viewModel.Tags, viewModel.thumbnails.full));
                    collectionHandler.InformationLabel = String.Format((App.CurrectLocalizationDictionary["locale_AddDownloadQueryItem"] as string), viewModel.ID);
                }
                else if (collectionHandler.PathForSavingImages == string.Empty)
                    throw new ArgumentException("Path can't be null");
            }
            catch(Exception ex)
            {
                App.WriteMessage(ex, true);
            }
        }
    }
}
