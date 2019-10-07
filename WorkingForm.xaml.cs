using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.IO;
using Ionic.Zip;
using System.Threading;

namespace File_server_Study__
{
    /// <summary>
    /// Логика взаимодействия для WorkingForm.xaml
    /// </summary>
    public partial class WorkingForm : Window
    {
        public WorkingForm()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            if (Config.SavePath == "" || !File.Exists(Config.SavePath+ Config.EndNameZip))
            {
                FormFIrstTime fft = new FormFIrstTime();
                fft.ShowDialog();
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.ShowDialog();
                Config.SavePath = fbd.SelectedPath;
                lblPath.Content = "Локальное хранилище " + Config.SavePath;
                ZipWork.lblstatus = lblStatus;
                Thread th = new Thread(ZipWork.newZip);
                lblStatus.Content = "Создание ZIP архива, ожидайте!";
                th.Start();

            }
            else
            {
                lblPath.Content = "Локальное хранилище " + Config.SavePath;
            }
            ImageList il = new ImageList();
            il.Images.Add(new System.Drawing.Icon("foldericon.ico"));
            il.Images.Add(new System.Drawing.Icon("bigfolder.ico"));
            lvFiles.LargeImageList = il;
            lvFiles.View = View.LargeIcon;
            ZipWork.lvFiles = lvFiles;
            ZipWork.ListViewFill();



        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ZipWork.lblstatus = lblStatus;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            ZipWork.Path = fbd.SelectedPath;
            Thread add = new Thread(ZipWork.addFolderInZip);
            add.Start();
        }
    }
}
