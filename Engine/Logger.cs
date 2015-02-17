using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Engine
{
    internal enum LOGLEVEL
    {
        VERBOSE = 3,
        INFO = 2,
        DEBUG = 1,
        NULL = 0
    }

    public class Logger
    {
        private String logFile;
        private Boolean logToConsole;

        private StreamWriter sw = null;

        public Logger(String file, Boolean withConsole = false, Boolean append = true)
        {
            this.logFile = file;
            this.logToConsole = withConsole;

            if (file != "")
            {
                this.sw = new StreamWriter(this.logFile, append);
                this.sw.AutoFlush = true;

                this.WriteLine("Initialized");
            }
        }

        ~Logger()
        {
            if (this.sw != null)
            {
                try
                {
                    this.sw.Close();
                }
                catch (Exception)
                {
                }

                this.sw = null;
            }
        }

        public void WriteLine(String text)
        {
            String logText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + text;

            if (this.logToConsole)
            {
                Console.WriteLine(logText);
            }

            if (this.sw != null)
            {
                this.sw.WriteLine(logText);
            }
        }
    }
}
