using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Net.Mail;

namespace Reports
{
    public  class Log
    {
        private static string p;

        //Log.Inst.WriteToLogDEBUG(string.Format("{0} - {1}",
        //                "var item in items", sw.ElapsedMilliseconds.ToString()));

        private static Log i;
        public static Log Inst
        {
            get
            {
                if (i == null)
                {
                    i = new Log();
                }
                
            
                    return i;
            }
        }

   
         public Log()
        {

            p = Path.Combine(Environment.CurrentDirectory, @"logs\\start.log"
                + string.Format("-{0:yyyy}-{0:MM}-{0:dd}", DateTime.Now));

            if (!Directory.Exists("logs")) {
                Directory.CreateDirectory("logs");
            }

            if (!File.Exists(p)) {
                File.Create(p);
            }

        }

        public  void WriteToLogDEBUG(string m)
        {
            try
            {
#if DEBUG
                System.IO.StreamWriter file = new System.IO.StreamWriter(p, true);
                file.WriteLine(string.Format("{0}:{1}", DateTime.Now, m));

                file.Close();
#endif
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }

        public  void WriteToLog(string m)
        {
            
                lock (i)
                {

                    System.IO.StreamWriter file = new System.IO.StreamWriter(p, true);
                    file.WriteLine(string.Format("{0}: {1}", DateTime.Now.ToString("HH:mm:ss"), m));

                    file.Close();

                }
           
        }
    }
}
