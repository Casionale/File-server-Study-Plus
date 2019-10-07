using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace File_server_Study__
{
    /// <summary>
    /// Логика взаимодействия для FormFIrstTime.xaml
    /// </summary>
    public partial class FormFIrstTime : Window
    {
        public FormFIrstTime()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tbPassword.Password != "")
            {
                Config.FilePass = tbPassword.Password;
                this.Close();
            }
            else
            {
                MessageBox.Show("Пароль пуст!","Сообщение о пустом пароле",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
    }
}
