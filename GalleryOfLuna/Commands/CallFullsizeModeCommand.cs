using GalleryOfLuna.ViewModel;
using GalleryOfLuna.Views;
using System;
using System.Windows.Input;

namespace GalleryOfLuna.Commands
{
    class CallFullsizeModeCommand : ICommand
    {
        private wndMainViewModel handler;
        private int index;

        /// <summary>
        /// Initializes a new instance of the CallFullsizeModeCommand class.
        /// </summary>
        public CallFullsizeModeCommand(wndMainViewModel handler, int index)
        {
            this.handler = handler;
            this.index = index;
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
            return (handler.ImageViewModelCollection.Count != 0);
        }

        public void Execute(object parameter)
        {
            try
            {
                new wndFullsize { DataContext = new wndFullsizeViewModel(handler, index) }.ShowDialog();
            }
            catch (Exception ex)
            {
                App.WriteMessage(ex, true);
            }
        }
    }
}
