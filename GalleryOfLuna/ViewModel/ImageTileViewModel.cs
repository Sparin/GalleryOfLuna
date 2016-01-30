using Apex.MVVM;
using GalleryOfLuna.Commands;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace GalleryOfLuna.ViewModel
{
    internal class ImageTileViewModel
    {
        public struct Thumbnails
        {
            public Uri thumb_tiny { get; set; }
            public Uri thumb_small { get; set; }
            public Uri thumb { get; set; }
            public Uri small { get; set; }
            public Uri medium { get; set; }
            public Uri large { get; set; }
            public Uri tall { get; set; }
            public Uri full { get; set; }
        }

        public Uri ThumbnailUri { get { return GetUriToImage(true); } }
        public string ThumbnailCachePath { get { return GetCachePathToImage(true); } }
        public string OriginalCachePath { get { return GetCachePathToImage(false); } }
        public Uri OriginalUri { get { return GetUriToImage(false); } }
        public string DownloadPath { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public int Score { get; set; }
        public int ID { get; set; }
        public string Uploader { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public Uri uriSource { get; set; }
        public Thumbnails thumbnails;

        public Command GetUriToClipboardCommand { get; private set; }
        public ICommand AddDownloadQueryItemCommand { get; private set; }
        public ICommand CallFullsizeModeCommand { get; private set; }

        private wndMainViewModel viewModel;

        public ImageTileViewModel()
        {
            GetUriToClipboardCommand = new Command(GetUriToClipboard);
        }

        public ImageTileViewModel(wndMainViewModel viewModel, int index)
        {
            this.viewModel = viewModel;
            GetUriToClipboardCommand = new Command(GetUriToClipboard);
            this.AddDownloadQueryItemCommand = new AddDownloadQueryItemCommand(this, viewModel);
            this.CallFullsizeModeCommand = new CallFullsizeModeCommand(viewModel, index);
        }

        private string GetCachePathToImage(bool Thumbnail)
        {
            int size = 0;
            if (Thumbnail)
                size = viewModel.ThumbnailSize;
            else
                size = viewModel.HighResolutionSize;

            Uri uri = GetUriToImage(Thumbnail);

            return string.Format("{0}GalleryOfLuna\\cache\\Derpibooru\\{1}_{2}{3}", Path.GetTempPath(), ID, ((wndMainViewModel.SizeOfImages)size).ToString("F"), System.IO.Path.GetExtension(uri.AbsolutePath));
        }

        private Uri GetUriToImage(bool Thumbnail)
        {
            int size = 0;
            if (Thumbnail)
                size = viewModel.ThumbnailSize;
            else
                size = viewModel.HighResolutionSize;

            switch (size)
            {
                case 0: return thumbnails.thumb_tiny;
                case 1: return thumbnails.thumb_small;
                case 2: return thumbnails.thumb;
                case 3: return thumbnails.small;
                case 4: return thumbnails.medium;
                case 5: return thumbnails.large;
                case 6: return thumbnails.tall;
                case 7: return thumbnails.full;
                default: throw new Exception("Cannot recognize size of image by collection from wndViewModel");
            }
        }

        private void GetUriToClipboard()
        {
            try
            {
                Clipboard.SetText(thumbnails.full.AbsoluteUri);
            }
            catch (Exception ex)
            {
                App.WriteMessage(ex, false);
            }
        }
    }
}
