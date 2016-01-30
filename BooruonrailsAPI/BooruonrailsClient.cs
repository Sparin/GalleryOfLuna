using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace BooruonrailsAPI //derpiboo.ru Booruonrails API v1.0 + Noblockme
{
    public class BooruonrailsClient
    {
        public WebClientExtended webClient = new WebClientExtended();
        public string OriginalURL { get; private set; }
        public string Site { get; set; }
        public string KeyAPI { get; set; }
        private bool ForceAnonymizer = false;
        private bool FireCloudLogicalIssue = false;

        public BooruonrailsClient(string URL) : this(URL, "", false, false)
        { }
        public BooruonrailsClient(string URL, string KeyAPI) : this(URL, KeyAPI, false, false)
        { }
        public BooruonrailsClient(string URL, string KeyAPI, bool IgnoreAvailable, bool ForceAnonymizer)
        {
            this.ForceAnonymizer = ForceAnonymizer;
            if (URL != null)
            {
                OriginalURL = URL;
                try
                {
                    if (ForceAnonymizer)
                        this.Site = GetAnonymizerURL(URL);
                    else
                    {
                        string text = webClient.DownloadString(URL);
                        this.Site = URL;
                    }
                }
                catch (Exception)
                {
                    if (!IgnoreAvailable)
                    {
                        this.Site = GetAnonymizerURL(URL);
                    }
                }

                this.KeyAPI = KeyAPI;
            }
            else
                throw new ArgumentNullException(URL);
            FixFireCloudLogicalIssue();
        }

        private void FixFireCloudLogicalIssue()
        {
            if (new Uri(OriginalURL).Host.Contains("derpiboo"))
            {
                try
                {
                    webClient.DownloadString("https://derpicdn.net");
                    FireCloudLogicalIssue = false;
                }
                catch (Exception)
                {
                    FireCloudLogicalIssue = true;
                }
            }
        }

        public string GetAnonymizerURL(string URL)
        {
            string result = string.Empty;
            string resultJson = webClient.DownloadString("http://noblockme.ru/api/anonymize?url=" + URL);
            JObject obj = JObject.Parse(resultJson);
            foreach (JProperty w in obj.Children())
            {
                if (w.Name == "result")
                    result = w.Value.ToString();
                if (w.Name == "status" && Convert.ToInt32(w.Value) != 0)
                    throw new Exception("Anonymaizer request has been failed");
            }
            return result;
        }

        public string GetRestructedURL(string URL)
        {
            if (!ForceAnonymizer || (FireCloudLogicalIssue && new Uri(URL).Host.Contains("derpicdn.net")))
                return URL;
            Uri tempUri = new Uri(URL);
            string anonymizerHost = GetAnonymizerURL(tempUri.Host);
            string result = anonymizerHost;
            foreach (string segment in tempUri.Segments)
                result += segment;
            return result;
        }

        public Task<BooruonrailsImage[]> GetImagesTaskAsync(string SearchTag, int Page, CancellationToken ct)
        {
            TaskCompletionSource<BooruonrailsImage[]> tskResult = new TaskCompletionSource<BooruonrailsImage[]>();
            List<BooruonrailsImage> Result = new List<BooruonrailsImage>();

            //if (SearchTag != string.Empty)
            //    SearchTag += ",safe";
            //else
            //    SearchTag = "safe";

            try
            {
                Uri URL;
                switch (SearchTag)
                {
                    case "alltop":
                        URL = new Uri(this.Site + "/lists/all_time_top_scoring.json?page=" + Page + "&key=" + this.KeyAPI);
                        break;
                    case "top":
                        URL = new Uri(this.Site + "/lists/top_scoring.json?page=" + Page + "&key=" + this.KeyAPI);
                        break;
                    case "watchlist":
                        URL = new Uri(this.Site + "/images/watched.json?page=" + Page + "&key=" + this.KeyAPI);
                        break;
                    case "favourites":
                        URL = new Uri(this.Site + "/images/favourites.json?page=" + Page + "&key=" + this.KeyAPI);
                        break;
                    case "":
                        URL = new Uri(this.Site + "images/page/" + Page + ".json" + "?key=" + this.KeyAPI);
                        break;
                    default:
                        URL = new Uri(this.Site + "search.json?sbq=" + SearchTag.Replace(" ", "+") + "&page=" + Page + "&key=" + this.KeyAPI);
                        break;
                }

                StreamReader sr = new StreamReader(webClient.OpenRead(URL));
                string str;
                string json = string.Empty;
                while ((str = sr.ReadLine()) != null && !ct.IsCancellationRequested)
                    json += str;
                ct.ThrowIfCancellationRequested();

                switch (SearchTag)
                {
                    default:
                        JObject js = JObject.Parse(json);
                        foreach (JProperty w in js.Children())
                        {
                            if (w.Name == "search" || w.Name == "images")
                                foreach (JArray x in w.Children())
                                {
                                    foreach (JObject m in x.Children<JObject>())
                                    {
                                        ct.ThrowIfCancellationRequested();
                                        BooruonrailsImage temp = new BooruonrailsImage();
                                        temp = JsonConvert.DeserializeObject<BooruonrailsImage>(m.ToString());
                                        if (new Uri(OriginalURL).Host != URL.Host)
                                        {
                                            temp.representations.full = GetRestructedURL("http:" + temp.representations.full).Remove(0, 5);
                                            temp.representations.large = GetRestructedURL("http:" + temp.representations.large).Remove(0, 5);
                                            temp.representations.medium = GetRestructedURL("http:" + temp.representations.medium).Remove(0, 5);
                                            temp.representations.small = GetRestructedURL("http:" + temp.representations.small).Remove(0, 5);
                                            temp.representations.tall = GetRestructedURL("http:" + temp.representations.tall).Remove(0, 5);
                                            temp.representations.thumb = GetRestructedURL("http:" + temp.representations.thumb).Remove(0, 5);
                                            temp.representations.thumb_small = GetRestructedURL("http:" + temp.representations.thumb_small).Remove(0, 5);
                                            temp.representations.thumb_tiny = GetRestructedURL("http:" + temp.representations.thumb_tiny).Remove(0, 5);
                                        }
                                        Result.Add(temp);
                                    }
                                }
                        }
                        break;
                }
                tskResult.TrySetResult(Result.ToArray());
            }
            catch (Exception ex)
            {
                tskResult.TrySetException(ex);
            }

            return tskResult.Task;
        }
    }
    public class BooruonrailsImage
    {
        [JsonObject(MemberSerialization.OptIn)]
        public struct Representaitions
        {
            [JsonProperty("thumb_tiny")]
            public string thumb_tiny { get; set; }

            [JsonProperty("thumb_small")]
            public string thumb_small { get; set; }

            [JsonProperty("thumb")]
            public string thumb { get; set; }

            [JsonProperty("small")]
            public string small { get; set; }

            [JsonProperty("medium")]
            public string medium { get; set; }

            [JsonProperty("large")]
            public string large { get; set; }

            [JsonProperty("tall")]
            public string tall { get; set; }

            [JsonProperty("full")]
            public string full { get; set; }
        }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("id_number")]
        public int id_number { get; set; }

        [JsonProperty("created_at")]
        public string created_at { get; set; }

        [JsonProperty("updated_at")]
        public string updated_at { get; set; }

        [JsonProperty("file_name")]
        public string file_name { get; set; }

        [JsonProperty("description")]
        public string description { get; set; }

        [JsonProperty("uploader")]
        public string uploader { get; set; }

        [JsonProperty("image")]
        public string image { get; set; }

        [JsonProperty("score")]
        public int score { get; set; }

        [JsonProperty("upvotes")]
        public int upvotes { get; set; }

        [JsonProperty("downvotes")]
        public int downvotes { get; set; }

        [JsonProperty("faves")]
        public int faves { get; set; }

        [JsonProperty("comment_count")]
        public int comment_count { get; set; }

        [JsonProperty("tags")]
        public string tags { get; set; }

        [JsonProperty("tag_ids")]
        public string[] tag_ids { get; set; }

        [JsonProperty("width")]
        public int width { get; set; }

        [JsonProperty("height")]
        public int height { get; set; }

        [JsonProperty("aspect_ratio")]
        public double aspect_ratio { get; set; }

        [JsonProperty("original_format")]
        public string original_format { get; set; }

        [JsonProperty("mime_type")]
        public string mime_type { get; set; }

        [JsonProperty("sha512_hash")]
        public string sha512_hash { get; set; }

        [JsonProperty("original_sha512_hash")]
        public string original_sha512_hash { get; set; }

        [JsonProperty("source_url")]
        public string source_url { get; set; }

        [JsonProperty("license")]
        public string license { get; set; }

        [JsonProperty("representations")]
        public Representaitions representations;

        [JsonProperty("is_rendered")]
        public bool is_rendered { get; set; }

        [JsonProperty("is_optimized")]
        public bool is_optimized { get; set; }
    }

    public class WebClientExtended : WebClient
    {
        public int Timeout = 1 * 5 * 1000;
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest wr = base.GetWebRequest(address);
            wr.Timeout = Timeout;
            return wr;
        }
    }
}

