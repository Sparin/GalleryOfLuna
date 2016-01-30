using Apex.MVVM;
using GalleryOfLuna.Behaviors;
using GalleryOfLuna.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace GalleryOfLuna.ViewModel
{
    class wndFullsizeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ListBoxItem> _tags = new ObservableCollection<ListBoxItem>();
        public ObservableCollection<ListBoxItem> Tags
        {
            get { return _tags; }
            set
            {
                _tags = value;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs("Tags"));
            }
        }

        public ObservableCollection<UIElement> FlipViewCollection { get; set; }

        private WebClient _webClient = new WebClient();
        public bool IsBusy { get { return _webClient.IsBusy; } }
        public string StatusBar { get { return String.Format("{0}/{1} Loading...", _imageIndex + 1, FlipViewCollection.Count); } }

        public int HandlerCollectionCount { get { return viewModel.ImageViewModelCollection.Count; } }

        public string SearchQuery
        {
            get
            {
                List<String> temp = new List<string>();
                temp.AddRange(Regex.Replace(viewModel.SearchQuery, @"[^\w:. ]", " ").Replace("  ", " ").Split(' ').ToArray());
                string selected = string.Empty;
                foreach (ListBoxItem lsi in Tags)
                    if (lsi.IsSelected && !temp.Contains(lsi.Content))
                        if (viewModel.SearchQuery.Replace(" ", "").Length + selected.Length != 0)
                            selected += ", " + lsi.Content;
                        else
                            selected += lsi.Content;
                return viewModel.SearchQuery + selected;
            }
            set
            {
                viewModel.SearchQuery = value;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs("SearchQuery"));
            }
        }

        private int countLap = 0;
        private int _imageIndex = -1;
        public int ImageIndex
        {
            get
            {
                return _imageIndex;
            }
            set
            {
                if (countLap < 5) //Fuck you MahApps and forgive me for being idiot
                    countLap++;
                if (countLap != 2 && countLap != 3)
                {
                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("ImageIndex"));
                        handler(this, new PropertyChangedEventArgs("Tags"));
                    }

                    if (_imageIndex != value && viewModel.IsBusy != true && value < HandlerCollectionCount)
                    {
                        if (_imageIndex != -1)
                            ((AnimatedImage)((Border)FlipViewCollection[_imageIndex]).Child).StopAnimate();                            
                        Console.WriteLine("Fullsize Mode changed index to {0}", value);
                        _imageIndex = value;
                        if (value - 1 >= 0)
                            ((AnimatedImage)((Border)FlipViewCollection[value-1]).Child).ChangeImageToText("Loading...", 12);
                        if(value +1<FlipViewCollection.Count)
                            ((AnimatedImage)((Border)FlipViewCollection[value+1]).Child).ChangeImageToText("Loading...", 12);

                        ObservableCollection<ListBoxItem> temp = new ObservableCollection<ListBoxItem>();
                        foreach (string tag in viewModel.ImageViewModelCollection[_imageIndex].Tags.Split(','))
                        {
                            string s = tag;
                            if (s[0] == ' ')
                                s = tag.Remove(0, 1);
                            temp.Add(new ListBoxItem
                            {
                                Content = s
                            });
                        }

                        Tags = SetTagFlags(temp);

                        _webClient.CancelAsync();
                        _webClient = new WebClient();
                        try
                        {
                            string CachePath = viewModel.ImageViewModelCollection[value].OriginalCachePath;
                            if (CachePath != null)
                            {
                                _webClient.DownloadFileCompleted += (obj, args) =>
                                {
                                    if (args.Error != null || args.Cancelled)
                                    {
                                        if (File.Exists(CachePath))
                                        {
                                            File.Delete(CachePath);
                                            Console.WriteLine("WebClient's callback delete a broken image: {0}", CachePath);
                                        }
                                        Console.WriteLine(args.Error.Message);
                                    }
                                    else
                                        ((AnimatedImage)((Border)FlipViewCollection[value]).Child).ChangeImage(CachePath);
                                    PropertyChangedEventHandler h = PropertyChanged;
                                    if (h != null)
                                        h(this, new PropertyChangedEventArgs("IsBusy"));
                                };
                                if (!File.Exists(CachePath))
                                {
                                    App.ClearCacheFolder();
                                    _webClient.DownloadFileAsync(viewModel.ImageViewModelCollection[value].OriginalUri, CachePath);
                                }
                                else
                                    ((AnimatedImage)((Border)FlipViewCollection[value]).Child).ChangeImage(CachePath);

                                var transform = (TransformGroup)((AnimatedImage)((Border)FlipViewCollection[value]).Child).RenderTransform;

                                foreach (var child in transform.Children.OfType<ScaleTransform>())
                                {
                                    child.ScaleX = 1;
                                    child.ScaleY = 1;
                                }
                                foreach (var translate in transform.Children.OfType<TranslateTransform>())
                                    translate.X = 1;

                                if (handler != null)
                                    handler(this, new PropertyChangedEventArgs("IsBusy"));
                                if (handler != null)
                                    handler(this, new PropertyChangedEventArgs("StatusBar"));
                            }
                        }
                        catch (OperationCanceledException) { }
                        catch (Exception ex)
                        {
                            App.WriteMessage(ex, true);
                        }
                    }
                }
            }
        }
        private wndMainViewModel viewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand UpdateImageCollectionCommand
        {
            get
            {
                return viewModel.UpdateImageCollectionCommand;
            }
        }
        public Command UpdateImageCollectionFullsizeCommand { get; private set; }
        public Command UpdateTagsCommand { get; private set; }
        public ICommand SaveImageCommand { get { return viewModel.ImageViewModelCollection[ImageIndex].AddDownloadQueryItemCommand; } }

        public wndFullsizeViewModel(wndMainViewModel viewModel, int index)
        {
            this.viewModel = viewModel;
            UpdateImageCollectionFullsizeCommand = new Command(GetImageCollectionFullsize);
            UpdateTagsCommand = new Command(UpdateTags);
            FlipViewCollection = new ObservableCollection<UIElement>();
            foreach (ImageTileViewModel vm in this.viewModel.ImageViewModelCollection)
                FlipViewCollection.Add(SetTransformationLogic());

            ImageIndex = index;
        }

        private void GetImageCollectionFullsize()
        {
            viewModel.SearchQuery = SearchQuery;
            FlipViewCollection.Select((ui) => ((Border)ui).Child).ToList().ForEach((img) => ((AnimatedImage)img).StopAnimate());
            UpdateImageCollectionCommand.Execute(null);
        }

        private Border SetTransformationLogic()
        {
            AnimatedImage img = new AnimatedImage
            {
                Stretch = System.Windows.Media.Stretch.Uniform,
                AutoStartAnimation = true
            };
            img.ChangeImageToText("Loading...", 12);
            Border border = new Border();
            img.RenderTransformOrigin = new Point(0.5, 0.5);

            TransformGroup tFormGroup = new TransformGroup();
            tFormGroup.Children.Add(new ScaleTransform());
            tFormGroup.Children.Add(new TranslateTransform());

            img.RenderTransform = tFormGroup;
            img.Tag = new ImageParams(1, new Point(), new Point());
            TransformationLogic tLogic = new TransformationLogic(border);

            img.MouseWheel += tLogic.controlMouseWheel;
            img.MouseLeftButtonDown += tLogic.controlMouseLeftButtonDown;
            img.MouseLeftButtonUp += tLogic.controlMouseLeftButtonUp;
            img.MouseMove += tLogic.controlMouseMove;

            border.Child = img;
            return border;
        }

        private ObservableCollection<ListBoxItem> SetTagFlags(ObservableCollection<ListBoxItem> tagsCollection)
        {
            string temp = Regex.Replace(SearchQuery, @"[^\w:. ]", " ").Replace("  ", " ");
            string[] tags = temp.Split(' ');
            foreach (string tag in tags)
                foreach (ListBoxItem s in tagsCollection)
                    if (s.Content.ToString() == tag)
                        s.IsSelected = true;

            return tagsCollection;
        }

        private void UpdateTags()
        {
            PropertyChangedEventHandler x = PropertyChanged;
            if (x != null)
                x(this, new PropertyChangedEventArgs("SearchQuery"));
        }

    }
}
