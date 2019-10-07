using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using System.Windows;
using System.Threading;
using System.Windows.Controls;


namespace File_server_Study__
{
    public static class ZipWork
    {
        static public Label lblstatus;
        static public System.Windows.Forms.ListView lvFiles;
        static public string Path;
        public static void newZip()
        {
            ZipFile zf = new ZipFile(Config.SavePath + Config.EndNameZip);
            zf.Password = Config.FilePass;
            zf.Save();
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                lblstatus.Content = "ZIP архив успешно создан!";
            }));
        }

        public static void ListViewFill()
        {
            ZipFile zf = new ZipFile(Config.SavePath + Config.EndNameZip);
            zf.Password = Config.FilePass;
            ICollection<ZipEntry> files = zf.SelectEntries("*\\*.*");

            List<string> FilesList = new List<string>();
            foreach (ZipEntry entry in files)
                if (!entry.IsDirectory)
                {
                    FilesList.Add(entry.FileName.Substring(entry.FileName.IndexOf('/') + 1));
                }
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                lvFiles.Items.Clear();
                for (int i = 0; i < FilesList.Count; i++)
                    lvFiles.Items.Add(FilesList[i], 0);
            }));
        }

        public static void addFolderInZip() 
        {
            ZipFile zf = new ZipFile(Config.SavePath + Config.EndNameZip);
            zf.Password = Config.FilePass;
            zf.AddDirectory(Path,"folder");
            //zf.AddItem(Path);
            zf.Save();
            ListViewFill();
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                lblstatus.Content = Path+" успешно добавлен!";
            }));
        }
    }
}
