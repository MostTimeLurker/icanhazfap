using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace Engine
{
    // https://github.com/SirCmpwn/RedditSharp/blob/master/RedditSharp/AuthProvider.cs
    public class RedditLoader
    {
        // List of saved Items
        // username -> List of id-values (from saved)
        public Dictionary<String, List<String>> savedItems = new Dictionary<String, List<String>>();

        // username -> links
        public Dictionary<String, List<String>> userUrls = new Dictionary<String, List<String>>();
        // username -> list of reddit id ("seen" items)
        // public Dictionary<String, List<Dictionary<String, String>>> userHistory = new Dictionary<String, List<Dictionary<String, String>>>();
        public Dictionary<String, SortedList<String, String>> userHistory = new Dictionary<String, SortedList<String, String>>();

        public List<String> urlList = new List<String>();
        public List<String> userList = new List<String>();
        
        private String urlPrefixUser = "http://www.reddit.com/user/";
        private String urlSuffixSaved = "/saved.json?count=100&limit=100";

        private String urlSuffixOverview = "/overview.json?count=100&limit=100";

        private String patternRegularLink = "(?<complete><a href=\"(?<url>[^\"]+?)\">\\[link\\]</a>){1,}";
        private String patternImgurLink = @"(?<url>http://(i.|m.|www.|)imgur.com/(.*?))( |<|$|\)|\s){1,}";

        private RedditApi redditApi;
        private Logger logger;

        private LOGLEVEL logLevel = LOGLEVEL.DEBUG;

        private void WriteToLog(String text, LOGLEVEL level = LOGLEVEL.VERBOSE)
        {
            if (this.logger != null)
            {
                String logText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + text;

                if (level <= this.logLevel)
                {
                    // this.logger.WriteLine(text);
                    Console.WriteLine(logText);
                }

                this.logger.WriteLine(text);
            }
        }

        public RedditLoader(RedditApi api, Logger log)
        {
            this.redditApi = api;
            this.logger = log;
        }

        public void doSaveItemList(String file)
        {
            String json = JsonConvert.SerializeObject(this.savedItems, Formatting.Indented);

            StreamWriter sw = new StreamWriter(file, false);
            sw.AutoFlush = true;

            sw.WriteLine(json);
            
            sw.Close();
        }

        public void doLoadItemList(String file)
        {
            if (File.Exists(file) == false)
            {
                return;
            }

            StreamReader sr = new StreamReader(file);
            String json = sr.ReadToEnd();
            sr.Close();

            this.savedItems = JsonConvert.DeserializeObject<Dictionary<String, List<String>>>(json);

            this.WriteToLog("Loaded " + this.savedItems.Count + " Users/Items");
        }

        public void doSaveUserUrlList(String file)
        {
            String json = JsonConvert.SerializeObject(this.userUrls, Formatting.Indented);

            StreamWriter sw = new StreamWriter(file, false);
            sw.AutoFlush = true;

            sw.WriteLine(json);

            sw.Close();
        }

        public void doLoadUserUrlList(String file)
        {
            if (File.Exists(file) == false)
            {
                return;
            }

            StreamReader sr = new StreamReader(file);
            String json = sr.ReadToEnd();
            sr.Close();

            this.userUrls = JsonConvert.DeserializeObject<Dictionary<String, List<String>>>(json);

            this.WriteToLog("Loaded " + this.userUrls.Count + " Users with their data");
        }

        public void doSaveUserHistoryList(String file)
        {
            String json = JsonConvert.SerializeObject(this.userHistory, Formatting.Indented);

            StreamWriter sw = new StreamWriter(file, false);
            sw.AutoFlush = true;

            sw.WriteLine(json);

            sw.Close();
        }

        public void doLoadUserHistoryList(String file)
        {
            if (File.Exists(file) == false)
            {
                return;
            }

            StreamReader sr = new StreamReader(file);
            String json = sr.ReadToEnd();
            sr.Close();

            this.userHistory = JsonConvert.DeserializeObject<Dictionary<String, SortedList<String, String>>>(json);

            this.WriteToLog("Loaded " + this.userHistory.Count + " Users with their history");
        }


        public void doGetSavedItems(Boolean onlyOnePage = false, String after = "xx")
        {
            int iPage = 0;
            String nextPage = after;

            if (this.redditApi.LoggedIn == false)
            {
                this.WriteToLog("not logged in, aborting!");
                return;
            }

            this.WriteToLog("doGetSavedItems BEGIN");
            this.WriteToLog("onlyOnePage: " + onlyOnePage);

            while (nextPage != "")
            {
                try
                {
                    this.WriteToLog("Loading Page " + (iPage++));
                    nextPage = this.doGetSavedItemPage(this.redditApi.Login, nextPage);

                    if (onlyOnePage)
                        nextPage = "";
                }
                catch (Exception ex)
                {
                    this.WriteToLog("### ERROR doGetSavedItems(" + onlyOnePage + ", " + after + "): " + ex.Message, LOGLEVEL.DEBUG);
                }
            }

            this.WriteToLog("Found " + this.savedItems.Count + " unique Users/Items");

            this.WriteToLog("doGetSavedItems END");
        }

        // listNumber = dicNumber.Keys.ToList();
        public void doGetUserFromList(List<String> users)
        {
            this.WriteToLog("doGetUserFromList BEGIN");
            this.WriteToLog("List with " + users.Count + " entries", LOGLEVEL.DEBUG);

            for (int i = 0; i < users.Count; i++)
            {
                String local_user = users[i];
                this.doGetUser(local_user);

                if ((i%10) == 0)
                {
                    this.WriteToLog("user="+ local_user + ", i=" + i + "/" + users.Count + " -> saving file", LOGLEVEL.DEBUG);
                    this.doSaveUserUrlList(@"d:\userurl-" + i.ToString("000") + ".txt");
                    this.doSaveUserHistoryList(@"d:\userhistory-" + i.ToString("000") + ".txt");
                }
            }


            this.WriteToLog("doGetUserFromList END");
        }

        // Every Item in overview from a user
        public void doGetUser(String username, Boolean onlyOnePage = false, String after = "xx")
        {
            int iPage = 0;
            String nextPage = after;

            this.WriteToLog("doGetUser BEGIN");
            this.WriteToLog("Getting User '" + username + "', onlyOnePage: " + onlyOnePage);

            while (nextPage != "")
            {
                try
                {
                    this.WriteToLog("Loading Page " + (iPage++));
                    nextPage = this.doGetUserOverviewPage(username, nextPage);

                    if (onlyOnePage)
                        nextPage = "";
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof (WebException))
                    {
                        WebException wex = ex as WebException;

                        if (wex.Status == WebExceptionStatus.ProtocolError)
                        {
                            if (((HttpWebResponse) wex.Response).StatusCode == HttpStatusCode.NotFound)
                            {
                                // handle the 404 here
                                this.WriteToLog("### ERROR doGetUser(" + username + ", " + onlyOnePage + ", " + after + "): 404 - User not found", LOGLEVEL.DEBUG);
                                nextPage = ""; // nothing to do anymore
                            }
                        }
                        else if (wex.Status == WebExceptionStatus.NameResolutionFailure)
                        {
                            // handle name resolution failure
                            this.WriteToLog("### ERROR doGetUser(" + username + ", " + onlyOnePage + ", " + after + "): Host/Site/URL error DNS " + wex.Message, LOGLEVEL.DEBUG);
                        }
                        else
                        {
                            this.WriteToLog("### ERROR doGetUser(" + username + ", " + onlyOnePage + ", " + after + "): Unknown WebException " + wex.Message, LOGLEVEL.DEBUG);
                        }
                    }
                    else
                    {
                        this.WriteToLog(
                            "### ERROR doGetUser(" + username + ", " + onlyOnePage + ", " + after + "): " + ex.Message,
                            LOGLEVEL.DEBUG);
                    }
                }
            }

            this.WriteToLog("Found " + this.userUrls[username].Count + " unique URLs on " + (iPage -1) + " pages for user '" + username+ "'", LOGLEVEL.DEBUG);

            this.WriteToLog("doGetUser END");
        }


        private String doGetSavedItemPage(String username, String after = "xx")
        {
            String nextPage = "";
            String url = this.urlPrefixUser + username + this.urlSuffixSaved;
            url += (after == "xx" ? "" : "&after=" + after);
            
            this.WriteToLog("doGetSavedItemPage BEGIN");
            this.WriteToLog("Loading for User '" + username + "', after='" + after + "'");

            WebClient wc = new WebClient();
            if (this.redditApi != null)
            {
                wc.Headers.Add(HttpRequestHeader.Cookie, "reddit_session=" + this.redditApi.Cookie);
            }

            this.WriteToLog("url='" + url + "'");
            String json = wc.DownloadString(url);
            redditSavedList onePage = JsonConvert.DeserializeObject<redditSavedList>(json);
            List<redditListingEntry> savedList = onePage.data.children;

            this.WriteToLog("Got List with " + savedList.Count + " Items");
            for (int i = 0; i < savedList.Count; i++)
            {
                redditListingEntry entry = savedList[i];

                // kind: t1_ comment, t3_ link
                this.WriteToLog("Item '" + i + "'");
                this.WriteToLog(" From: " + entry.data.author);
                this.WriteToLog(" Type: " + entry.kind);
                this.WriteToLog(" ID:   " + entry.data.id);

                nextPage = entry.kind + "_" + entry.data.id;

                if (this.savedItems.ContainsKey(entry.data.author) == false)
                {
                    this.WriteToLog(" New User");
                    this.savedItems.Add(entry.data.author, new List<String>());
                }

                this.savedItems[entry.data.author].Add(entry.data.id);

            }


            this.WriteToLog("doGetSavedItemPage END");

            return nextPage;
        }

        public String doGetUserOverviewPage(String username, String after = "xx")
        {
            if (this.userUrls.ContainsKey(username) == false)
            {
                this.userUrls.Add(username, new List<String>());
            }

            if (this.userHistory.ContainsKey(username) == false)
            {
                this.userHistory.Add(username, new SortedList<String,String>());
            }

            String nextPage = "";
            String url = this.urlPrefixUser + username + this.urlSuffixOverview;
            url += (after == "xx" ? "" : "&after=" + after);

            this.WriteToLog("doGetUserOverviewPage BEGIN");
            this.WriteToLog("Loading for User '" + username + "', after='" + after + "'");

            WebClient wc = new WebClient();
            if (this.redditApi != null)
            {
                wc.Headers.Add(HttpRequestHeader.Cookie, "reddit_session=" + this.redditApi.Cookie);
            }

            this.WriteToLog("url='" + url + "'");
            String json = wc.DownloadString(url);
            redditOverviewList onePage = JsonConvert.DeserializeObject<redditOverviewList>(json);
            List<redditOverviewListingEntry> overviewList = onePage.data.children;

            this.WriteToLog("Got List with " + overviewList.Count + " Items");
            for (int i = 0; i < overviewList.Count; i++)
            {
                redditOverviewListingEntry entry = overviewList[i];

                this.WriteToLog("Item '" + i + "'");
                this.WriteToLog(" From: " + entry.data.author);
                this.WriteToLog(" Type: " + entry.kind);
                this.WriteToLog(" ID:   " + entry.data.id);

                nextPage = entry.kind + "_" + entry.data.id;

                // "edited":false,
                // "created":1423816371.0,
                if (this.userHistory[username].ContainsKey(nextPage) && (this.userHistory[username][nextPage] == entry.data.created) && (entry.data.edited == false))
                {
                    // Seite bekannt
                        // Zeitstempel ist gleich und nicht editiert
                        // nixmachen
                    this.WriteToLog(" Entry '" + nextPage + "' for user '" + username + "' already know (edited=" + entry.data.edited + ",created=" +entry.data.created + "), skipped", LOGLEVEL.INFO);
                }
                else
                {
                    // Seite ist neu -> laden
                    this.doExtractLinksFromUserEntry(username, entry.data, entry.kind);
                    // this.userHistory[username].Add(nextPage, entry.data.created);
                    if (this.userHistory[username].ContainsKey(nextPage))
                    {
                        //Console.WriteLine("nextPage bekannt");
                        //Console.WriteLine("historie " + this.userHistory[username][nextPage]);
                        //Console.WriteLine("data     " + entry.data.created);
                    }
                    else
                    {
                        this.userHistory[username].Add(nextPage, entry.data.created);
                    }
                }

/*
                if (this.userHistory[username](nextPage) == false)
                {
                    this.doExtractLinksFromUserEntry(username, entry.data, entry.kind);
                    this.userHistory[username].Add(nextPage);
                }
                else
                {
                    this.WriteToLog(" Entry '" + nextPage + "' for user '" + username + "' already know, skipped", LOGLEVEL.INFO);
                }
 * */
            }

            this.WriteToLog("doGetUserOverviewPage END");

            return nextPage;
        }

        private void doExtractLinksFromUserEntry(String username, redditOverviewEntry entry, String kind)
        {
            // body,link,_url, url
            // t1 link_url, t3 url

            String firstUrl = "";
            if (kind == "t1")
            {
                firstUrl = entry.link_url;
            }
            else if (kind == "t3")
            {
                firstUrl = entry.url;
            }

            if (firstUrl != "")
            {
                if (this.userUrls[username].Contains(firstUrl) == false)
                    this.userUrls[username].Add(firstUrl);
            }

            if ((entry.body == null) || (entry.body.Trim() == ""))
                return;

            String content = entry.body;

            Regex regexRegularLink = new Regex(this.patternRegularLink, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Regex regexImgurLink = new Regex(this.patternImgurLink, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            MatchCollection mc = regexRegularLink.Matches(content);
            foreach (Match match in mc)
            {
                String found = match.Groups["url"].ToString();

                //Console.WriteLine(" r ||" + found + "||");

                //if (urlHistorie.Contains(found))
                //{
                //    //Console.WriteLine("bereits bekannter Link: " + found);
                //}
                //else
                {
                    if (this.userUrls[username].Contains(found) == false)
                    {
                        //Console.WriteLine("AddRegex");
                        this.userUrls[username].Add(found);
                    }
                }


                content = content.Replace(match.Groups["complete"].ToString(), "");
            }

            mc = regexImgurLink.Matches(content);
            foreach (Match match in mc)
            {
                //Console.WriteLine("   rege match: " + match.Groups["url"]);
                String found = match.Groups["url"].ToString();

                //Console.WriteLine(" i ||" + found + "||");

                //if (urlHistorie.Contains(found))
                //{
                //    //Console.WriteLine("bereits bekannter Link");
                //}
                //else
                {
                    if (this.userUrls[username].Contains(found) == false)
                    {
                        //Console.WriteLine("AddImgur");
                        this.userUrls[username].Add(found);
                    }
                }
            }
        }
    }
}
