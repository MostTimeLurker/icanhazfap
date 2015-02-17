using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Engine
{
        public class MediaEmbed
        {
            public string content { get; set; }
            public int width { get; set; }
            public bool scrolling { get; set; }
            public int height { get; set; }
        }

        public class SecureMediaEmbed
        {
        }

        public class Oembed
        {
            public string provider_url { get; set; }
            public string title { get; set; }
            public string type { get; set; }
            public string html { get; set; }
            public int thumbnail_width { get; set; }
            public int height { get; set; }
            public int width { get; set; }
            public string version { get; set; }
            public string provider_name { get; set; }
            public string thumbnail_url { get; set; }
            public int thumbnail_height { get; set; }
        }

        public class Media
        {
            public Oembed oembed { get; set; }
            public string type { get; set; }
        }

        public class redditOverviewEntry
        {
                public string subreddit_id { get; set; }
                public bool edited { get; set; }
                public object banned_by { get; set; }
                public string link_id { get; set; }
                public string link_author { get; set; }
                public object likes { get; set; }
                public object replies { get; set; }
                public List<object> user_reports { get; set; }
                public bool saved { get; set; }
                public string id { get; set; }
                public int gilded { get; set; }
                public bool archived { get; set; }
                public object report_reasons { get; set; }
                public string author { get; set; }
                public string parent_id { get; set; }
                public int score { get; set; }
                public object approved_by { get; set; }
                public int controversiality { get; set; }
                public string body { get; set; }
                public string link_title { get; set; }
                public object author_flair_css_class { get; set; }
                public int downs { get; set; }
                public string body_html { get; set; }
                public string subreddit { get; set; }
                public bool score_hidden { get; set; }
                public string name { get; set; }
                public string created { get; set; }
                public object author_flair_text { get; set; }
                public string link_url { get; set; }
                public string created_utc { get; set; }
                public object distinguished { get; set; }
                public List<object> mod_reports { get; set; }
                public object num_reports { get; set; }
                public int ups { get; set; }

            [JsonProperty(Required = Newtonsoft.Json.Required.Default)]
                public string url { get; set; }
        }

        public class redditInnerListingEntry
        {
            public string domain { get; set; }
            public object banned_by { get; set; }
            public MediaEmbed media_embed { get; set; }
            public string subreddit { get; set; }
            public object selftext_html { get; set; }
            public string selftext { get; set; }
            public object likes { get; set; }
            public List<object> user_reports { get; set; }
            public object secure_media { get; set; }
            public string link_flair_text { get; set; }
            public string id { get; set; }
            public int gilded { get; set; }
            public bool archived { get; set; }
            public bool clicked { get; set; }
            public object report_reasons { get; set; }
            public string author { get; set; }
            public int num_comments { get; set; }
            public int score { get; set; }
            public object approved_by { get; set; }
            public bool over_18 { get; set; }
            public bool hidden { get; set; }
            public string thumbnail { get; set; }
            public string subreddit_id { get; set; }
            public bool edited { get; set; }
            public string link_flair_css_class { get; set; }
            public object author_flair_css_class { get; set; }
            public int downs { get; set; }
            public SecureMediaEmbed secure_media_embed { get; set; }
            public bool saved { get; set; }
            public bool stickied { get; set; }
            public bool is_self { get; set; }
            public string permalink { get; set; }
            public string name { get; set; }
            public string created { get; set; }
            public string url { get; set; }
            public object author_flair_text { get; set; }
            public string title { get; set; }
            public string created_utc { get; set; }
            public object distinguished { get; set; }
            public Media media { get; set; }
            public List<object> mod_reports { get; set; }
            public bool visited { get; set; }
            public object num_reports { get; set; }
            public int ups { get; set; }
        }

        public class  redditListingEntry
        {
            public String kind { get; set; }
            public redditInnerListingEntry data { get; set; }
        }

        public class redditOverviewListingEntry
        {
            public String kind { get; set; }
            public redditOverviewEntry data { get; set; }
        }

        public class redditSavedListData
        {
            public string modhash { get; set; }
            public List<redditListingEntry> children { get; set; }

            [JsonProperty(Required = Newtonsoft.Json.Required.Default)]
            public string after { get; set; }

            [JsonProperty(Required = Newtonsoft.Json.Required.Default)]
            public string before { get; set; }
        }

        public class redditOverviewListData
        {
            public string modhash { get; set; }
            public List<redditOverviewListingEntry> children { get; set; }

            [JsonProperty(Required = Newtonsoft.Json.Required.Default)]
            public string after { get; set; }

            [JsonProperty(Required = Newtonsoft.Json.Required.Default)]
            public string before { get; set; }
        }


        public class redditSavedList
        {
            public string kind { get; set; }
            public redditSavedListData data { get; set; }
        }

        public class redditOverviewList
        {
            public string kind { get; set; }
            public redditOverviewListData data { get; set; }
        }

        public class redditUserData
        {
            public bool need_https { get; set; }
            public string modhash { get; set; }
            public string cookie { get; set; }
        }

        public class redditLogin
        {
            public List<object> errors { get; set; }
            public redditUserData data { get; set; }
        }

    }
