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
        static void Main(string[] args)
        {
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
            Loader l = new Loader();
            l.Run("itsnotp0rn");
            //l.RunFromList();

            //foreach (string u in url)
            //{
            //    Console.WriteLine("lade " +u);
            //    l.Run(u);
            //}
        }
    }
}
