using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Engine
{
    internal class redditLoginRoot
    {
        public redditLogin json { get; set; }
    }

    public class RedditApi
    {
        private String urlLoginSSL = "https://ssl.reddit.com/api/login/";

        private String redditLogin = "";

        private Boolean isLoggedIn = false;
        private String redditCookie = "";
        private String redditModhash = "";

        private Logger logger;

        public Boolean LoggedIn
        {
            get { return this.isLoggedIn; }
        }

        public String Cookie
        {
            get { return this.redditCookie; }
        }

        public String Modhash
        {
            get { return this.redditModhash; }
        }

        public String Login
        {
            get { return this.redditLogin; }
        }

        public RedditApi(Logger log)
        {
            this.logger = log;
        }

        private void WriteToLog(String text)
        {
            if (this.logger != null)
            {
                this.logger.WriteLine(text);
            }
        }

        public void doLogin(String login, String password)
        {
            this.redditLogin = login;

            String myurl = this.urlLoginSSL; //  +login;
            String myparam = "op=login-main&user=" + login + "&passwd=" + password + "&rem=on&api_type=json";

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string json = wc.UploadString(myurl, myparam);

                redditLoginRoot root = JsonConvert.DeserializeObject<redditLoginRoot>(json);
                if (root != null)
                {
                    redditLogin rj = root.json;

                    if (rj != null)
                    {
                        if (rj.errors.Count == 0)
                        {
                            // eingelogged
                            this.isLoggedIn = true;
                            this.redditCookie = HttpUtility.UrlEncode(rj.data.cookie);
                            this.redditModhash = HttpUtility.UrlEncode(rj.data.modhash);

                            //Console.WriteLine("cookie " + this.redditCookie);
                            //Console.WriteLine("hash   " + this.redditModhash);

                            this.WriteToLog("logged in as '" + login + "'");

                            return;
                        }
                    }

                    this.WriteToLog("login FAILED");
                }

            }

        }

    }
}
