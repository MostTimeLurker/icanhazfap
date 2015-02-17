using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// Fehlt
//  Gallery = /r/polandball
// CustomGallery = ka

namespace Engine
{
    /*
    public class NullableValueProvider : IValueProvider
    {
        private readonly object _defaultValue;
        private readonly IValueProvider _underlyingValueProvider;

        public NullableValueProvider(MemberInfo memberInfo, Type underlyingType)
        {
            _underlyingValueProvider = new DynamicValueProvider(memberInfo);
            _defaultValue = Activator.CreateInstance(underlyingType);
        }

        public void SetValue(object target, object value)
        {
            _underlyingValueProvider.SetValue(target, value);
        }

        public object GetValue(object target)
        {
            return _underlyingValueProvider.GetValue(target) ?? _defaultValue;
        }
    }

    public class SpecialContractResolver : DefaultContractResolver
    {
        protected override IValueProvider CreateMemberValueProvider(MemberInfo member)
        {
            if(member.MemberType == MemberTypes.Property)
            {
                var pi = (PropertyInfo) member;
                if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof (Nullable<>))
                {
                    return new NullableValueProvider(member, pi.PropertyType.GetGenericArguments().First());
                }
            }
            else if(member.MemberType == MemberTypes.Field)
            {
                var fi = (FieldInfo) member;
                if(fi.FieldType.IsGenericType && fi.FieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    return new NullableValueProvider(member, fi.FieldType.GetGenericArguments().First());
            }

            return base.CreateMemberValueProvider(member);
        }
    }
    */

    #region API Image
    internal class ImgurImage
    {
        public ImgurImageData data { get; set; }
        public bool success { get; set; }
        public int status { get; set; }
    }

    internal class ImgurImageData
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public string id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public string title { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public string description { get; set; }
        public string datetime { get; set; }
        public string type { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool animated { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string size { get; set; }
        public string views { get; set; }
        public string bandwidth { get; set; }
        public string vote { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool favorite { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool nsfw { get; set; }
        public string section { get; set; }
        public string account_url { get; set; }
        public string account_id { get; set; }
        public string link { get; set; }
    }
    #endregion

    #region API Album
    internal class ImgurAlbum
    {
        public ImgurAlbumData data { get; set; }
        public bool success { get; set; }
        public int status { get; set; }
    }

    internal class ImgurAlbumData
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Include,  DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public string id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public String title { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public object description { get; set; }
        public int datetime { get; set; }
        public string cover { get; set; }
        public string cover_width { get; set; }
        public string cover_height { get; set; }
        public string account_url { get; set; }
        public string account_id { get; set; }
        public string privacy { get; set; }
        public string layout { get; set; }
        public int views { get; set; }
        public string link { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool favorite { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool nsfw { get; set; }
        public object section { get; set; }
        public int images_count { get; set; }
        public List<ImgurImageData> images { get; set; }
    }
    #endregion

    #region vidme
    internal class vidmeAuth
    {
        public bool status { get; set; }
        public vidmeAuthData auth { get; set; }
        public vidmeUser user { get; set; }
    }

    internal class vidmeUser
    {
        public string user_id { get; set; }
        public string username { get; set; }
        public string full_url { get; set; }
        public object avatar { get; set; }
        public string avatar_url { get; set; }
        public object cover { get; set; }
        public object cover_url { get; set; }
        public int follower_count { get; set; }
        public string likes_count { get; set; }
        public int video_count { get; set; }
        public string video_views { get; set; }
        public int videos_scores { get; set; }
        public object bio { get; set; }
    }

    internal class vidmeAuthData
    {
        public string token { get; set; }
        public string expires { get; set; }
        public string user_id { get; set; }
    }

    internal class vidmeVideoJson
    {
        public bool status { get; set; }
        public vidmeVideoData video { get; set; }
        public vidmeProgress progress { get; set; }
        public vidmeWatchers watchers { get; set; }
    }

    internal class vidmeWatchers
    {
        public int total { get; set; }
        public List<string> countries { get; set; }
    }

    internal class vidmeProgress
    {
        public int progress { get; set; }
    }
    internal class vidmeVideoFormat
    {
        public string type { get; set; }
        public string uri { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int version { get; set; }
    }

    internal class vidmeVideoData
    {
            public string video_id { get; set; }
            public string url { get; set; }
            public string full_url { get; set; }
            public string embed_url { get; set; }
            public string user_id { get; set; }
            public string complete { get; set; }
            public string complete_url { get; set; }
            public string state { get; set; }
            public string title { get; set; }
            public object description { get; set; }
            public double duration { get; set; }
            public int height { get; set; }
            public int width { get; set; }
            public string date_created { get; set; }
            public string date_stored { get; set; }
            public string date_completed { get; set; }
            public int comment_count { get; set; }
            public int view_count { get; set; }
            public int version { get; set; }
            public bool nsfw { get; set; }
            public string thumbnail { get; set; }
            public string thumbnail_url { get; set; }
            public string thumbnail_gif { get; set; }
            public string thumbnail_gif_url { get; set; }
            public string storyboard { get; set; }
            public int score { get; set; }
            public int likes_count { get; set; }
            public object channel_id { get; set; }
            public string source { get; set; }
            public bool @private { get; set; }
            public int latitude { get; set; }
            public int longitude { get; set; }
            public object place_id { get; set; }
            public object place_name { get; set; }
            public string colors { get; set; }
            public vidmeUser user { get; set; }
            public List<vidmeVideoFormat> formats { get; set; }
    }
    
    #endregion

    public class ImgurLoader
    {
        private Logger logger;
        private String clientId;

        private String pathBase = "";

        private String urlPrefixImage = "https://api.imgur.com/3/image/";
        private String urlPrefixAlbum = "https://api.imgur.com/3/album/";
        private String urlPrefixGfycat = "http://zippy.gfycat.com/";
        private String urlPrefixVidble = "http://www.vidble.com/";

        private LOGLEVEL logLevel = LOGLEVEL.DEBUG;

        private void WriteToLog(String text, LOGLEVEL level = LOGLEVEL.VERBOSE)
        {
            if (this.logger != null)
            {
                if (level <= this.logLevel)
                {
                    this.logger.WriteLine(text);
                }
            }
        }

        public ImgurLoader(Logger log, String id, String targetPath)
        {
            this.logger = log;
            this.clientId = id;
            this.pathBase = targetPath;

            if (this.pathBase == "")
                this.pathBase = Directory.GetCurrentDirectory() + @"\IMAGES\";
        }

        private String doCleanId(String text)
        {
            String result = text;

            if (result.Contains("#"))
                result = result.Substring(0, result.IndexOf("#"));

            if (result.Contains("?"))
                result = result.Substring(0, result.IndexOf("?"));

            if (result.Contains("."))
                result = result.Substring(0, result.IndexOf("."));

            return result;
        }

        private String doCleanName(String text)
        {
            String result = (text == null ? "" : text);

            Regex regex = new Regex(@"[^a-zA-Z0-9\-\._ ]");
            result = regex.Replace(result, "");

            return result;
        }

        private String getExtensionByType(String fileType)
        {
            String result = "";

            switch (fileType)
            {
                case "image/jpeg":
                    result = ".jpg";
                    break;
                case "image/png":
                    result = ".png";
                    break;

                case "image/gif":
                    result = ".gif";
                    break;

                default:
                    this.WriteToLog("### UNBEKANNT " + fileType, LOGLEVEL.DEBUG);
                    result = ""; // ".txt";
                    break;
            }

            return result;
        }

        public void CheckCredits()
        {
            // https://api.imgur.com/3/credits
            WebClient wc = new WebClient();
            if (this.clientId != "")
                wc.Headers.Add("Authorization", "Client-ID " + this.clientId);


            String credits = wc.DownloadString("https://api.imgur.com/3/credits");
            Console.WriteLine(credits);
        }

        public void doGetInfo_Album(String url, String username = "", Boolean displayOnly = true)
        {
            WebClient wc = new WebClient();
            if (this.clientId != "")
                wc.Headers.Add("Authorization", "Client-ID " + this.clientId);

            String local_url =  this.urlPrefixAlbum + this.doCleanId(Path.GetFileName(url));
            String local_directory = this.pathBase + username + @"\";

            this.WriteToLog("doGetInfo_Album BEGIN");
            this.WriteToLog("Loading '" + local_url + "'");
            
            // 401 ohne ID
            // 403 bei falscher ID
            String json = wc.DownloadString(local_url);
            //var settings = new JsonSerializerSettings() { ContractResolver = new SpecialContractResolver() };
            //ImgurAlbum oneAlbum = JsonConvert.DeserializeObject<ImgurAlbum>(json, settings);

            ImgurAlbum oneAlbum = JsonConvert.DeserializeObject<ImgurAlbum>(json);

            if (oneAlbum.success)
            {
                String albumTitle = this.doCleanName(oneAlbum.data.title);
                String albumId = this.doCleanName(oneAlbum.data.id);
                String albumName = albumTitle == "" ? albumId : albumTitle;

                local_directory += albumName + @"\";
                this.WriteToLog(" download to " + local_directory);

                if (displayOnly)
                {
                    this.WriteToLog(" id:           " + oneAlbum.data.id);
                    this.WriteToLog(" images_count: " + oneAlbum.data.images_count);
                    this.WriteToLog(" title:        '" + oneAlbum.data.title + "'");
                    this.WriteToLog(" description:   '" + oneAlbum.data.description + "'");
                }

                foreach (ImgurImageData image in oneAlbum.data.images)
                {
                    if (displayOnly)
                    {
                        this.WriteToLog("   image id:          " + image.id);
                        this.WriteToLog("   image type:        " + image.type);
                        this.WriteToLog("   image link:        " + image.link);
                        this.WriteToLog("   image title:       " + image.title);
                        this.WriteToLog("   image description: " + image.description);
                        this.WriteToLog("-------------------------------------------");
                    }
                    else
                    {
                        // download file to local_directory + albumName 
                        String imgTitle = this.doCleanName(image.title);
                        String imgId = this.doCleanName(image.id);
                        String filename = imgTitle == "" ? imgId : imgTitle;
                        filename += this.getExtensionByType(image.type);
                        this.WriteToLog("download " + filename);
                        this.doRealDownload_Image(image.link, filename, local_directory);
                    }

                }
            }

            this.WriteToLog("doGetInfo_Album END");
        }

        public void doGetInfo_Image(String url, String username = "", Boolean displayOnly = true)
        {
            WebClient wc = new WebClient();
            if (this.clientId != "")
                wc.Headers.Add("Authorization", "Client-ID " + this.clientId);

            String local_url = this.urlPrefixImage + this.doCleanId(Path.GetFileName(url));
            String local_directory = this.pathBase + username + @"\";
            
            this.WriteToLog("doGetInfo_Image BEGIN");
            this.WriteToLog("Loading '" + local_url + "'");

            // 401 ohne ID
            // 403 bei falscher ID
            String json = wc.DownloadString(local_url);
            ImgurImage oneImage = JsonConvert.DeserializeObject<ImgurImage>(json);

            if (oneImage.success)
            {
                if (displayOnly)
                {
                    this.WriteToLog(" id:          " + oneImage.data.id);
                    this.WriteToLog(" type:        " + oneImage.data.type);
                    this.WriteToLog(" link:        " + oneImage.data.link);
                    this.WriteToLog(" title:       '" + oneImage.data.title + "'");
                    this.WriteToLog(" description: '" + oneImage.data.description + "'");
                }
                else
                {
                    String imgTitle = this.doCleanName(oneImage.data.title);
                    String imgId = this.doCleanName(oneImage.data.id);
                    String filename = imgTitle == "" ? imgId : imgTitle;
                    filename += this.getExtensionByType(oneImage.data.type);

                    this.doRealDownload_Image(oneImage.data.link, filename, local_directory);
                }
            }

            this.WriteToLog("doGetInfo_Image END");
        }

        public void doDownload_Album(String url, String username)
        {
            this.WriteToLog("doDownload_Album BEGIN");

            this.doGetInfo_Album(url, username, false);

            this.WriteToLog("doDownload_Album END");
        }

        public void doDownload_Image(String url, String username)
        {
            this.WriteToLog("doDownload_Album BEGIN");

            this.doGetInfo_Image(url, username, false);

            this.WriteToLog("doDownload_Album END");
        }

        private void doRealDownload_Image(String url, String filename, String directory)
        {
            WebClient wc = new WebClient();
            if (this.clientId != "")
                wc.Headers.Add("Authoriz" +
                               "ation", "Client-ID " + this.clientId);

            this.WriteToLog("doLoad_Image BEGIN");
            this.WriteToLog(" Downloading '" + url + "' as '" + filename + "' to '" + directory + "'");

            Directory.CreateDirectory(directory);
            if (File.Exists(directory + @"\" + filename))
            {
                this.WriteToLog("Ueberspringe " + filename + ", da bereits vorhanden");
            }
            else
            {
                wc.DownloadFile(url, directory + @"\" + filename);
            }

            this.WriteToLog("doLoad_Image END");
        }

        // https://docs.vid.me/
        // https://docs.vid.me/#api-Video-Detail
        // curl -H "AccessToken: abcdef0123456789"

        // https://api.vid.me/auth/create
        // curl -X POST -d "username=foo&password=bar" https://api.vid.me/auth/create
        // <li class="video_watching js-video-watchers" data-video="834596">
        public void doDownload_vidme(String url, String username)
        {
            String local_directory = this.pathBase + username + @"\";
            Directory.CreateDirectory(local_directory);

            WebClient wc = new WebClient();
            String html = wc.DownloadString(url);
            Regex regex_id =new Regex("(data-video=\"(?<videoid>.*?)\" ){1,}");
            Match match = regex_id.Match(html);
            String video_id = match.Groups["videoid"].ToString();

            String json = wc.DownloadString("https://api.vid.me/video/" + video_id);
            vidmeVideoJson video = JsonConvert.DeserializeObject<vidmeVideoJson>(json);
            // video.video.complete_url
            wc.DownloadFile(video.video.complete_url, local_directory + video_id + ".mp4");
        }

        /*
http://zippy.gfycat.com/BestThatDormouse.webm
http://gfycat.com/BestThatDormouse

http://zippy.gfycat.com/VariableObeseBilby.webm
http://gfycat.com/VariableObeseBilby
/*         */
        public void doDownload_gfycat(String url, String username)
        {
            String filename = this.doCleanName(Path.GetFileName(url)) + ".webm";
            String local_url = this.urlPrefixGfycat + filename;
            String local_directory = this.pathBase + username + @"\";

            WebClient wc = new WebClient();

            this.WriteToLog("doDownload_gfycat BEGIN");
            this.WriteToLog(" Downloading '" + local_url + "' as '" + filename + "' to '" + local_directory + "'");

            Directory.CreateDirectory(local_directory);
            wc.DownloadFile(local_url, local_directory + @"\" + filename);

            this.WriteToLog("doDownload_gfycat END");
        }

        public void doDownload_vidble(String url, String username)
        {
            // http://www.vidble.com/album/RBNPNdqI
            // http://www.vidble.com/XWqDQXigrd_med.jpg -> http://www.vidble.com/XWqDQXigrd.jpg
            // <a name='pic_3'></a><img data-original='/XWqDQXigrd_med.jpg' style='max-width:100%;' class='img2 img2_80 lazy' height='600' width='800'/><noscript><img src='/XWqDQXigrd_med.jpg' style='max-width:100%;' class='img2 img2_80' />
            // <img src='/XWqDQXigrd_med.jpg' style='max-width:100%;' class='img2 img2_80' />
            // img1 img2 img3

            String local_directory = this.pathBase + username + @"\";


            this.WriteToLog("doDownload_vidbleAlbum BEGIN");

            if (url.ToLower().Contains("/album/"))
            {
                local_directory += Path.GetFileName(url) + @"\";
                this.WriteToLog("Album found, new direcetory: " + local_directory);
            }

            Directory.CreateDirectory(local_directory);
            String local_url = url;

            String pattern = "(<img src='(?<filename1>.*?[^ '/])' (|id='idMain' )style='.*?' class='.*?'){1,}";
            Regex regex_Images = new Regex(pattern);

            WebClient wc = new WebClient();
            String html = wc.DownloadString(local_url);

            MatchCollection mc = regex_Images.Matches(html);
            for (int i = 0; i < mc.Count; i++)
            {
                String downloadFilename = mc[i].Groups["filename1"].ToString();
                downloadFilename = downloadFilename.Replace("_med", "");
                downloadFilename = this.urlPrefixVidble + downloadFilename;
                String local_filename = this.doCleanName(downloadFilename);
                local_filename = local_directory + local_filename;

                this.WriteToLog("downloading '" + downloadFilename + "' to " + local_filename );

                wc.DownloadFile(downloadFilename, local_filename);
            }

            this.WriteToLog("doDownload_vidbleAlbum END");
        }

        public void doDownload_imgur(String url, String username, ref int iAlbums, ref int iImages)
        {
            String local_url = url;
            String local_url_lower = url.ToLower();


            if (local_url_lower.Contains("/a/"))
            {
                this.WriteToLog(" Found Album");
                this.doDownload_Album(local_url, username);
                iAlbums++;
            }
            else
            {
                this.WriteToLog(" Found Image");
                this.doDownload_Image(local_url, username);
                iImages++;
            }

        }


        public void doDownload_List(String username, List<String> urls)
        {
            String local_directory = this.pathBase + username + @"\";

            this.WriteToLog("Downloading " + urls.Count + " URLs to " + local_directory);
            int iImages = 0, iAlbums = 0, iVidble = 0, iGfycat = 0, iVidme = 0, iOther = 0, iFailed=0;

            for (int i = 0; i < urls.Count; i++)
            {
                String local_url = urls[i];
                String local_url_lower = urls[i].ToLower();

                //if (local_url_lower.Contains("imgur") == false)
                //{
                //    this.WriteToLog("Ueberspringe '" + local_url + "'", LOGLEVEL.INFO);
                //    iOther++;
                //    continue;
                //}

                try
                {
                    if (local_url_lower.Contains("imgur"))
                    {
                        this.doDownload_imgur(local_url, username, ref iAlbums, ref iImages);
                    }
                    else if (local_url_lower.Contains("vidble"))
                    {
                        this.doDownload_vidble(local_url, username);
                        iVidble++;
                    }
                    else if (local_url_lower.Contains("gfycat"))
                    {
                        this.doDownload_gfycat(local_url, username);
                        iGfycat++;
                    }
                    else if (local_url_lower.Contains("vid.me"))
                    {
                        this.doDownload_vidme(local_url, username);
                        iVidme++;
                    }
                    else
                    {
                        this.WriteToLog("Unbekannte URL '" + local_url_lower + "'", LOGLEVEL.INFO);
                        iOther++;
                    }

                }
                catch (Exception ex)
                {
                    this.WriteToLog("Fehler bei " + local_url + ": " + ex.Message, LOGLEVEL.DEBUG);
                    iFailed++;
                }

                if ((i%20) == 0)
                {
                    Console.WriteLine("bin bei  "+ i + " von " + urls.Count);
                }
            }

            this.WriteToLog("Stats: iImage="+iImages + ", iAlbums=" +iAlbums +", iGfycat=" +iGfycat +", iVidble=" + iVidble+ ", iVidme=" + iVidme + ", iOther=" +  iOther + ", iFailed=" + iFailed, LOGLEVEL.DEBUG);
        }
       /* 
        public void doLoad(String username, List<String> urls, String directory = @"D:\Downloads\_xgur\__xperiment3\")
        {
            String targetDirectory = directory + username;
            this.WriteToLog("Downloading " + urls.Count + " URLs to " + targetDirectory);

            for (int i = 0; i < urls.Count; i++)
            {
                String url = urls[i];

                try
                {
                    this.doLoadFromUrl(url, targetDirectory);

                }
                catch (Exception ex)
                {
                    this.WriteToLog(" ERROR username="+ username + ", url="+url +": " +ex.Message);
                }
            }
        }

        public void doLoadFromUrl(String url, String directory)
        {
            String dateiName = Path.GetFileName(url);
            String local_url = url;

            if (local_url.Contains("imgur") == false)
                return;

            Directory.CreateDirectory(directory);
            WebClient wc = new WebClient();

            if (directory.EndsWith(@"\") == false)
                directory += @"\";

            if (dateiName.Contains("?"))
            {
                dateiName = dateiName.Substring(0, dateiName.IndexOf("?"));

                Console.WriteLine("Dateiname mit ? - dateiName '" + dateiName + "', url " +local_url);
            }

            if (local_url.Contains("#"))
            {
                local_url = url.Substring(0, url.IndexOf("#"));
                dateiName = dateiName.Substring(0, dateiName.IndexOf("#"));

                Console.WriteLine("url mit # - dateiName '" + dateiName + "', url " + local_url);
            }


            this.WriteToLog(" " + local_url + " -> " + directory + dateiName);

            if (local_url.Contains("/a/"))
            {
                // album
                wc.DownloadFile(local_url + "/zip", directory + dateiName + ".zip");
            }
            else
            {
                // picture
                try
                {
                    if (dateiName.Contains("."))
                    {
                        wc.DownloadFile(local_url.Replace(dateiName, "/download/" + dateiName),
                                        directory + dateiName);
                    }
                    else
                    {
                        wc.DownloadFile(local_url.Replace(dateiName, "/download/" + dateiName),
                                        directory + dateiName + ".jpg");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fehler beim Download von " + local_url + ":");
                    Console.WriteLine(ex.Message);

                    wc.DownloadFile(local_url, directory + dateiName);
                }

            }

        }
*/
        /*
        public void doGetImagesFromList(List<String> urlList, String username, String target, StreamWriter swHistorie)
        {
            WebClient wc =  new WebClient();

            String targetDirectory = target + username + @"\";

            // Erstelle Ziel
            Directory.CreateDirectory(targetDirectory);

            this.sw = new StreamWriter(targetDirectory + logFile, false);
            sw.AutoFlush = true;

            Console.WriteLine("Ziel " + targetDirectory);
            Console.WriteLine("Anzahl: " + urlList.Count);

            int i = 0;

            foreach (string url in urlList)
            {
                String dateiName = Path.GetFileName(url);
                String local_url = url;

                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm ") + url);


                if (url.Contains("imgur") ==false)
                    continue;

                //Console.WriteLine("lade " + url);

                try
                {
                    if (dateiName.Contains("?"))
                    {
                        dateiName = dateiName.Substring(0, dateiName.IndexOf("?"));
                        Console.WriteLine("dateiname " + dateiName);
                        Console.WriteLine("url " + local_url);
                    }

                    if (url.Contains("#"))
                    {
                        local_url = url.Substring(0, url.IndexOf("#"));
                        dateiName = dateiName.Substring(0, dateiName.IndexOf("#"));
                        Console.WriteLine("dateiname " + dateiName);
                        Console.WriteLine("url " + url);
                    }

                    if (local_url.Contains("/a/"))
                    {
                        //Console.WriteLine("Ein Album");
                        //dateigroesse!
                        wc.DownloadFile(local_url + "/zip", targetDirectory + dateiName + ".zip");
                    }
                    else
                    {
                        //Console.WriteLine("Normales Bild");
                        try
                        {
                            if (dateiName.Contains("."))
                            {
                                wc.DownloadFile(local_url.Replace(dateiName, "/download/" + dateiName),
                                                targetDirectory + dateiName);
                            }
                            else
                            {
                                wc.DownloadFile(local_url.Replace(dateiName, "/download/" + dateiName),
                                                targetDirectory + dateiName + ".jpg");
                            }

                        }
                        catch (Exception)
                        {
                            wc.DownloadFile(local_url, targetDirectory + dateiName);

                        }
                    }

                    swHistorie.WriteLine(url);
                    i++;

                    if ((i%50) == 0)
                    {
                        Console.WriteLine("  " + i + " / " + urlList.Count);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fehler beim Download von " + url);
                    Console.WriteLine(ex.Message);
                    this.sw.WriteLine("Fehler DL " + url + " : " + ex.Message);

                    this.bFehler = true;
                }
            }
        }*/

    }
}
