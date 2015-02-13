using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Engine
{
    /*
     * http://imgur.com/download/rpegihQ
      * http://imgur.com/rpegihQ

     * http://imgur.com/a/m5iEo
     * http://s.imgur.com/a/m5iEo/zip

     * */
    public class ImgurLoader
    {
        private String logFile = "";
        private StreamWriter sw;

        public Boolean bFehler = false;

        public ImgurLoader(String log = "_log.txt")
        {
            this.logFile = log.Replace(".", "-" + DateTime.Now.ToString("yyyyMMdd") +".");
        }

        ~ImgurLoader()
        {
            if (this.sw != null)
            {
                try
                {
                    this.sw.Close();
                    this.sw = null;

                }
                catch (Exception)
                {
                }
            }
        }

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

                    this.bFehler = true;
                }
            }
        }

    }
}
