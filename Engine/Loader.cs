using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Engine
{
    public class Loader
    {
        public void RunFromList(String datei = @"d:\userliste.txt", String datei_done = @"d:\userliste_done.txt")
        {
            List<String > user = new List<string>();

            StreamReader  sr = new StreamReader(datei);
            String s = "";

            while ((s = sr.ReadLine()) != null)
            {
                if (user.Contains(s) == false)
                    user.Add(s);
            }

            sr.Close();

            sr = new StreamReader(datei_done);
            while ((s = sr.ReadLine()) != null)
            {
                if (user.Contains(s))
                {
                    Console.WriteLine("entferne " + s);
                    user.Remove(s);
                }
            }
            sr.Close();

            StreamWriter sw = new StreamWriter(datei_done);
            sw.AutoFlush = true;

            int i = 1;
            foreach (string s1 in user)
            {
                try
                {
                    Console.WriteLine(DateTime.Now.ToString("yyyy-mm-dd hh:MM") + ": " + i + "/" + user.Count + " - " + s1);
                    Boolean b = this.Run(s1);

                    if (b == false)
                    {
                        Console.WriteLine("alles ok fuer " + s1);
                        sw.WriteLine(s1);
                    }
                    else
                    {
                        Console.WriteLine("Fehler bei " + s1);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("###  Fehler bei " + s1);
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }

            sw.Close();
        }

        public Boolean Run(String user, String target = @"D:\Downloads\_xgur\__xperiment\")
        {
            List<String> urlHistorie = new List<String>();
            String s = "";


            String targetDirectory = target + user + @"\";

            // Erstelle Ziel
            Directory.CreateDirectory(targetDirectory);


            if (File.Exists(targetDirectory + @"_historie.txt"))
            {

                StreamReader sr = new StreamReader(targetDirectory + @"_historie.txt");

                while ((s = sr.ReadLine()) != null)
                {
                    if (s != "")
                        urlHistorie.Add(s);
                }

                sr.Close();
            }

            StreamWriter swHistorie = new StreamWriter(targetDirectory + @"_historie.txt", true);
            swHistorie.AutoFlush = true;

            RedditLoader rl = new RedditLoader();
            rl.run(user, target, urlHistorie, swHistorie);

            ImgurLoader il = new ImgurLoader();
            il.doGetImagesFromList(rl.urlList, user, target, swHistorie);

            swHistorie.Close();

            return il.bFehler;
        }

        public void generateUserList(String datei)
        {
            StreamReader  sr = new StreamReader(datei);
            String s = "";

            List<String> userList = new List<string>();

            while ((s = sr.ReadLine()) != null)
            {
                if (s.StartsWith("http://www.reddit.com/user/"))
                {
                    String user = Path.GetFileName(s).Trim();

                    if (user == "#" || user.StartsWith("?") || user == "")
                        continue;

                    Console.WriteLine(user);

                    if (userList.Contains(user) == false)
                        userList.Add(user);
                }

            }

            sr.Close();

            StreamWriter sw = new StreamWriter(@"d:\userliste.txt", false);
            sw.AutoFlush = true;

            foreach (string s1 in userList)
            {
                sw.WriteLine(s1);
            }

            sw.Close();
        }


    }
}
