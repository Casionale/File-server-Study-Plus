using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace File_server_Study__
{
    static class Config
    {
        static public string SavePath { get; set; } = "";
        static public string Savelogin { get; set; } = "";

        static public string SavePass { get; set; } = "";

        static public bool Remember { get; set; } = false;

        static public string FilePass { get; set; } = "";
        static public bool Autonomy { get; set; } = false;

        static private int settingsCount = 4;

        static public string EndNameZip { set;  get; } =  "\\Study plus.zip";
        static public void ReadSettings()
        {
            StreamReader sr = new StreamReader("config.conf");
            for (int i = 0; i < settingsCount; i++)
            {
                switch (i)
                {
                    case 0: { Savelogin = sr.ReadLine(); break; }
                    case 1: { SavePass = sr.ReadLine(); break; }
                    case 2: { Remember = (sr.ReadLine() == "RememberMyAccount") ? true: false; break; }
                    case 3: { SavePath = sr.ReadLine(); break; }
                }
            }
            sr.Close();
            EndNameZip =  "\\Study plus "+Savelogin+" .zip";
        }

        static public void WriteSettings()
        {
            FileStream fs = new FileStream("config.conf", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(Savelogin);
            sw.WriteLine(SavePass);
            sw.WriteLine((Remember)? "RememberMyAccount":"NoRem");
            sw.WriteLine(SavePath);
            sw.Close();
            fs.Close();
        }

    }
}
