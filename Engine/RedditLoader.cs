using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Engine
{
    // https://github.com/SirCmpwn/RedditSharp/blob/master/RedditSharp/AuthProvider.cs
    public class RedditLoader
    {
        public List<String> urlList = new List<String>();

        
        private String urlUser = "http://www.reddit.com/user/";
        //private String suffixUser = "/overview.xml?count=50&limit=50";
        private String suffixUser = "/overview.xml?count=100&limit=100";

        private String patternRegularLink = "(?<complete><a href=\"(?<url>[^\"]+?)\">\\[link\\]</a>){1,}";
        private String patternImgurLink = @"(?<url>http://(i.|m.|www.|)imgur.com/(.*?))( |<|$\)){1,}";


        public void run(String user, String target, List<String> urlHistorie, StreamWriter swHistorie)
        {
            int i = 0;
            String nextPage = this.doGetLinksFromUser(user, "", urlHistorie,  swHistorie);

            if (nextPage != "")
            {
                while (nextPage != "")
                {
                    Console.WriteLine("Seite " + (i++));
                    nextPage = this.doGetLinksFromUser(user, nextPage, urlHistorie, swHistorie);
                }
            }
        }

        public String doGetLinksFromUser(String user, String after, List<String> urlHistorie, StreamWriter swHistorie )
        {
            String nextPage = "";

            String url = this.urlUser + user + suffixUser;
            if (after != "")
            {
                url += "&after=" + after;
            }

            //Console.WriteLine("Lade " + url);

            WebClient wc = new WebClient();
            String xml = wc.DownloadString(url);

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);
            // /rss/channel/item/description

            // XmlNodeList xItems = xdoc.SelectNodes("//channel/item/description");
            XmlNodeList xItems = xdoc.SelectNodes("//channel/item");
            //Console.WriteLine("gefunden: " + xItems.Count + " Items");


            Regex regexRegularLink = new Regex(this.patternRegularLink, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Regex regexImgurLink = new Regex(this.patternImgurLink, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            MatchCollection mc;

            foreach (XmlNode xmlNode in xItems)
            {
                String content = xmlNode["description"].InnerText;
                // String content = xmlNode.InnerText;
                String guid = xmlNode["guid"].InnerText;
                String title = xmlNode["title"].InnerText;

                //Console.WriteLine("---- " + guid);

                if (guid.EndsWith("/"))
                {
                    // http://www.reddit.com/r/gonewild/comments/2uocif/fulfilling_a_request/
                    // t3
                    String[] uriPath = guid.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

                    nextPage = "t3_" + uriPath[uriPath.Length - 2];
                }
                else
                {
                    // http://www.reddit.com/r/gonewildcurvy/comments/2tss6x/have_i_been_posting_in_the_wrong_place_any_love/co2a36b
                    // t1
                    nextPage = "t1_" + Path.GetFileName(guid);
                }

                //Console.WriteLine(" " + nextPage);

                if (content.Contains("[link]"))
                {
                    //Console.WriteLine("content enthaelt link");
                    //Console.WriteLine(content);

                    mc = regexRegularLink.Matches(content);

                    //Console.WriteLine(" * gefundene regulaere links: " + mc.Count);
                    foreach (Match match in mc)
                    {
                        Console.WriteLine("   link match: " + match.Groups["url"]);
                        String found = match.Groups["url"].ToString();

                        if (urlHistorie.Contains(found))
                        {
                            //Console.WriteLine("bereits bekannter Link: " + found);
                        }
                        else
                        {
                            if (this.urlList.Contains(found) == false)
                            {
                                this.urlList.Add(found);
                                //swHistorie.WriteLine(found);
                            }
                        }


                        content = content.Replace(match.Groups["complete"].ToString(), "");
                    }
                }


                mc = regexImgurLink.Matches(content);
                //Console.WriteLine(" * gefundene imgur links: " + mc.Count);
                foreach (Match match in mc)
                {
                    Console.WriteLine("   rege match: " + match.Groups["url"]);
                    String found = match.Groups["url"].ToString();

                    if (urlHistorie.Contains(found))
                    {
                        //Console.WriteLine("bereits bekannter Link");
                    }
                    else
                    {
                        if (this.urlList.Contains(found) == false)
                        {
                            this.urlList.Add(found);
                            //swHistorie.WriteLine(found);
                        }
                    }
                }
            }

            //Console.WriteLine("next Page " + nextPage);

            return nextPage;
        }

        // submitted by <a href=\"http://www.reddit.com/user/naughtyteacher93\"> naughtyteacher93 </a> to <a href=\"http://www.reddit.com/r/gonewild/\"> gonewild</a> <br/> <a href=\"http://imgur.com/ivqz1PA\">[link]</a> <a href=\"http://www.reddit.com/r/gonewild/comments/2vh56q/something_different_would_you_fuck_me_doggy_style/\">[20 comments]</a>
        public void testRegexpRegulaer(String text)
        {
            //text = text.Replace("[link]", "link");
            Regex regexRegularLink = new Regex(this.patternRegularLink, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            MatchCollection mc = regexRegularLink.Matches(text);
            Console.WriteLine("gefunden " + mc.Count);
            foreach (Match match in mc)
            {
                //Console.WriteLine("match: " + match.Groups["url"]);
                Console.WriteLine("match");
                for (int i = 0; i < match.Groups.Count; i++)
                {
                    Console.WriteLine(i + ": " + match.Groups[i]);
                }
            }
        }

        public void testRegexpImgur(String text)
        {
            Console.WriteLine("durchsuche |" +  text + "|");

            Regex regexImgurLink = new Regex(this.patternImgurLink, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            MatchCollection mc = regexImgurLink.Matches(text);
            Console.WriteLine("gefunden " + mc.Count);
            foreach (Match match in mc)
            {
                //Console.WriteLine("match: " + match.Groups["url"]);
                Console.WriteLine("match url(" + match.Groups["url"] + ")");
                for (int i = 0; i < match.Groups.Count; i++)
                {
                    Console.WriteLine(i + ": " + match.Groups[i]);
                }
            }
        }


    }
}
