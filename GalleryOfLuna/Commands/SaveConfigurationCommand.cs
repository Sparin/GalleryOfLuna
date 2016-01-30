using GalleryOfLuna.ViewModel;
using Microsoft.Win32;
using System;
using System.Windows.Input;

namespace GalleryOfLuna.Commands
{
    class SaveConfigurationCommand : ICommand
    {
        RegistryKey RegKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Gallery-of-Luna\");

        private wndMainViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the SaveConfigurationCommand class.
        /// </summary>
        public SaveConfigurationCommand(wndMainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event System.EventHandler CanExecuteChanged {
            add {
                CommandManager.RequerySuggested += value;
            }
            remove {
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
                RegKey.SetValue("LanguageCollectionIndex", viewModel.LanguageCollectionIndex);
                RegKey.SetValue("ThumbnailSize", viewModel.ThumbnailSize);
                RegKey.SetValue("HighResolutionSize", viewModel.HighResolutionSize);
                RegKey.SetValue("PathForSavingImages", viewModel.PathForSavingImages);
                RegKey.SetValue("SearchPage", viewModel.SearchPage);
                RegKey.SetValue("SearchQuery", viewModel.SearchQuery);
                RegKey.SetValue("LastSearchQuery", viewModel.LastSearchQuery);
                RegKey.SetValue("APIKey", viewModel.APIKey);
            }
            catch(Exception ex)
            {
                App.WriteMessage(ex, true);
            }
        }
    }
}
