using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Engine;

namespace simpleConsole
{
    class Program
    {

        // http://jsonformatter.curiousconcept.com/
        // http://json2csharp.com/
        static void Main(string[] args)
        {


            Logger logger = new Logger("text.txt", false, true);

            RedditApi ra = new RedditApi(logger);
            ra.doLogin(System.Configuration.ConfigurationManager.AppSettings["redditLogin"], System.Configuration.ConfigurationManager.AppSettings["redditPassword"]);

            RedditLoader rl = new RedditLoader(ra, logger);
            
            //rl.doGetSavedItems();
            //rl.doSaveItemList(@"d:\reddit_saved-20150215.txt");

            rl.doLoadItemList(@"d:\reddit_saved-20150217.txt");
            rl.doLoadUserHistoryList(@"d:\reddit_history-20150217.txt");

            rl.doGetUserFromList(rl.savedItems.Keys.ToList());
            
            // rl.doSaveUserUrlList(@"d:\reddit_urls_saved-20150215.txt");

            //return;

            //rl.doGetUser("babygirrrl");
            //rl.doSaveUserUrlList(@"d:\myuser_urls_json.txt");
            //rl.doLoadUserUrlList(@"d:\myuser_urls_json.txt");
            //rl.doLoadUserUrlList(@"d:\userurl-021.txt");

            //ImgurLoader il = new ImgurLoader(logger, "5f673558dd0d8ad");
            ImgurLoader il = new ImgurLoader(logger, System.Configuration.ConfigurationManager.AppSettings["imgurKey"], @"D:\Downloads\_xgur\__xperiment3\");
            //il.CheckCredits();

            //il.doDownload_vidme("https://vid.me/zo3o", "xxx");

            //il.doDownload_Image("http://imgur.com/a/uCGz5", "temp");
            //return;

            //il.doDownload_gfycat("http://gfycat.com/BrilliantTemptingBubblefish", "wersame");
            //il.doDownload_vidble("http://www.vidble.com/album/RBNPNdqI", "weresame");
            //il.doDownload_vidble("http://www.vidble.com/show/JlKo18As8c", "weresame");
            //il.doLoad("babygirrrl", rl.userUrls["babygirrrl"]);

            //il.doGetInfo_Image("http://imgur.com/a/uCGz5");

            /* */
            try
            {
                //il.doDownload_Album("http://imgur.com/a/RS8WR", @"babygirrrl");
                //il.doDownload_List("babygirrrl", rl.userUrls["babygirrrl"]);
                int i = 0;

                foreach (KeyValuePair<string, List<string>> userUrl in rl.userUrls)
                {
                    i++;
                    Console.WriteLine(i + "/" +rl.userUrls.Count + ": " + userUrl.Key + " with " + userUrl.Value.Count + " URLs");
                    il.doDownload_List(userUrl.Key, userUrl.Value);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            /**/
            return;

            //rl.runSaved("IchLeseMit", new List<string>(), null);

            //Loader loader = new Loader();

            //StreamWriter sw = new StreamWriter(@"d:\saveduser.txt", true);
            //sw.AutoFlush = true;

            //foreach (string user in rl.userList)
            //{
            //    Console.WriteLine("Lade " + user);
            //    sw.WriteLine(user);
            //}

            //sw.Close();


            StreamReader sr =  new StreamReader(@"d:\saveduser.txt");
            String s = "";

            List<String> user = new List<string>();
            while ((s = sr.ReadLine()) != null)
            {
                user.Add(s);
            }
            sr.Close();


            Loader loader = new Loader();
            foreach (string u in user)
            {
                Console.WriteLine("lade " + u);
                loader.Run(u);
            }

            //loader.Run()

            //rl.doGetLinksFromUser("hisfavoritetoy", "xx", null, null);
            return;

            /*
            List<String> url = new List<String>()
                {
                    "alexandraballerina",
                    "masterslittletoy",
                    "the_crema",
                    "mrmacster",
                    "Papaya_flight",
                    "CatchAsCatCan",
                    "Mandsb",
                    "SirPlusPet",
                    "grabmyassets",
                    "mixedcpl",
                    "LilMissScientist",
                    "dingodile69"

                };
             * */
            //Loader l = new Loader();
            //l.Run("itsnotp0rn");
            //l.RunFromList();

            //foreach (string u in url)
            //{
            //    Console.WriteLine("lade " +u);
            //    l.Run(u);
            //}
        }
    }
}
