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
            

            ImgurLoader il = new ImgurLoader(logger, System.Configuration.ConfigurationManager.AppSettings["imgurKey"], @"D:\Downloads\_xgur\__xperiment3\");
            //il.CheckCredits();
            try
            {
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

        }
    }
}
