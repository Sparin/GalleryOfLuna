using GalleryOfLuna.ViewModel;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Input;

namespace GalleryOfLuna.Commands
{
    class LoadConfigurationCommand : ICommand
    {
        RegistryKey RegKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Gallery-of-Luna\");

        private wndMainViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the LoadConfigurationCommand class.
        /// </summary>
        public LoadConfigurationCommand(wndMainViewModel viewModel)
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
            try
            {
                viewModel.SearchPage = Convert.ToInt32(RegKey.GetValue("SearchPage", 1));
                viewModel.SearchQuery = Convert.ToString(RegKey.GetValue("SearchQuery", string.Empty));
                viewModel.LastSearchQuery = Convert.ToString(RegKey.GetValue("LastSearchQuery", string.Empty));
                viewModel.APIKey = Convert.ToString(RegKey.GetValue("APIKey", string.Empty));
                viewModel.HighResolutionSize = Convert.ToInt32(RegKey.GetValue("HighResolutionSize", 7));
                viewModel.ThumbnailSize = Convert.ToInt32(RegKey.GetValue("ThumbnailSize", 2));

                string sf_pictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + @"\Luna's Gallery Images";
                viewModel.PathForSavingImages = Convert.ToString(RegKey.GetValue("PathForSavingImages", sf_pictures));
                if (viewModel.PathForSavingImages == string.Empty)
                    viewModel.PathForSavingImages = sf_pictures;
                if (!Directory.Exists(viewModel.PathForSavingImages))
                    Directory.CreateDirectory(viewModel.PathForSavingImages);

                viewModel.InformationLabel = App.CurrectLocalizationDictionary["locale_LoadConfigurationSuccesful"] as string;
            }
            catch(Exception ex)
            {
                App.WriteMessage(ex, true);
                viewModel.InformationLabel = App.CurrectLocalizationDictionary["locale_LoadConfigurationError"] as string;
            }            
        }
    }
}
