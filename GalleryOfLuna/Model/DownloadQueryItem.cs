using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Media.Imaging;

namespace GalleryOfLuna.Model
{
    //WIP
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class DownloadQueryItem : INotifyPropertyChanged
    {
        private string _destination = string.Empty;
        public string Destination
        {
            get
            {
                return _destination;
            }
            set
            {
                _destination = value;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs("Destination"));
            }
        }

        public string _status = string.Empty;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs("Status"));
            }
        }

        public string _tags = string.Empty;
        public string Tags
        {
            get
            {
                return _tags;
            }
            set
            {
                _tags = value;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs("Tags"));
            }
        }


        private WebClient _wbClient = new WebClient();
        private Uri _DownloadUri;
        public event PropertyChangedEventHandler PropertyChanged;


        public DownloadQueryItem(string path, string tags, Uri DownloadUri)
        {
            Destination = path;
            Tags = tags;
            _DownloadUri = DownloadUri;
            Status = "0%";

            _wbClient.DownloadProgressChanged += _wbClient_DownloadProgressChanged;
            _wbClient.DownloadFileCompleted += _wbClient_DownloadFileCompleted;
            _wbClient.DownloadFileAsync(_DownloadUri, Destination);
        }

        public void Repeat()
        {
            try
            {
                Cancel();
                _wbClient.DownloadFileAsync(_DownloadUri, Destination);
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " " + ex.Message);
                Status = ex.Message;
            }
        }

        public void Cancel()
        {
            try
            {
                if (_wbClient.IsBusy)
                    _wbClient.CancelAsync();
                if (File.Exists(Destination))
                    File.Delete(Destination);
                Status = "Удален";
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " " + ex.Message);
                Status = ex.Message;
            }
        }

        void _wbClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Status = String.Format("{0}%", e.ProgressPercentage);
        }

        //WIP Set metadata (Tags)
        void _wbClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            string[] tags = Tags.Split(',');
            if (Path.GetExtension(Destination) == ".jpeg" || Path.GetExtension(Destination) == ".jpg")
                SetUpMetadataOnImage(Destination, tags);
        }

        //WIP
        private static void SetUpMetadataOnImage(string filename, string[] tags)
        {
            try
            {
                // padding amount, using 2Kb.  don't need much here; metadata is rather small
                uint paddingAmount = 2048;
                BitmapDecoder original;
                BitmapEncoder output = new JpegBitmapEncoder();
                using (Stream file = File.Open(filename, FileMode.Open, FileAccess.ReadWrite))
                {
                    original = BitmapDecoder.Create(file, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnDemand);

                    if (original.Frames[0] != null && original.Frames[0].Metadata != null)
                    {
                        BitmapFrame frameCopy = (BitmapFrame)original.Frames[0].Clone();
                        BitmapMetadata metadata = original.Frames[0].Metadata.Clone() as BitmapMetadata;

                        metadata.SetQuery("/app1/ifd/PaddingSchema:Padding", paddingAmount);
                        metadata.SetQuery("/app1/ifd/exif/PaddingSchema:Padding", paddingAmount);
                        metadata.SetQuery("/xmp/PaddingSchema:Padding", paddingAmount);
                        metadata.SetQuery("System.Keywords", tags);

                        output.Frames.Add(BitmapFrame.Create(frameCopy, frameCopy.Thumbnail, metadata, frameCopy.ColorContexts));
                    }
                    using (FileStream outputFile = File.Open(filename + ".temp", FileMode.Create, FileAccess.Write))
                    {
                        output.Save(outputFile);
                    }
                }
                File.Delete(filename);
                File.Move(filename + ".temp", filename);
            }
            catch(Exception ex)
            {
                App.WriteMessage(ex, false);
            }
        }
    }
}
