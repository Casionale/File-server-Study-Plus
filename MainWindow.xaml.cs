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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading;
using System.Net.Mail;
using System.IO;

namespace File_server_Study__
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MySqlConnectionStringBuilder str_connect = new MySqlConnectionStringBuilder();
        MySqlConnection conn = new MySqlConnection();
        public Regex numberPhone = new Regex(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$");
        public Regex email = new Regex(@"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)");
        public Regex name = new Regex(@"^[a-zA-Z][a-zA-Z0-9-_\.]{1,20}$");
        public Regex strongPass = new Regex(@"(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$");

        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists("config.conf"))
            {
                cbAutonomy.Visibility = Visibility.Hidden;
            }
        }

        public bool isLogin = true;
        public bool loginIsValid = false;
        public bool emailIsValid = false;
        public bool nameIsValid = false;
        public bool phoneIsValid = false;
        string capcha = "";

        private void TbLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            tbLogin.Text = "";
        }

        private void BtnSelecting_Click(object sender, RoutedEventArgs e)
        {
            if (!isLogin)
            {
                btnSelecting.Content = "Логин";
                isLogin = !isLogin;
                panelLogin.Visibility = Visibility.Hidden;
                Canvas.SetTop(panelRegistration, Canvas.GetTop(panelLogin));
                Canvas.SetLeft(panelRegistration, Canvas.GetLeft(panelLogin));
                Canvas.SetTop(lCapcha, Canvas.GetTop(lTextCapcha));
                Canvas.SetLeft(lCapcha, Canvas.GetLeft(lTextCapcha));
                capcha = "";
                string capchaTrash = "";
                Random ran = new Random();
                for (int i=0; i<10; i++)
                {
                    capcha +=(char)ran.Next(65,88);
                    capchaTrash += "-";
                }
                lTextCapcha.Content = capcha;
                lCapcha.Content = capchaTrash;
                panelRegistration.Visibility = Visibility.Visible;

            }
            else
            {
                btnSelecting.Content = "Регистрация";
                isLogin = !isLogin;
                panelLogin.Visibility = Visibility.Visible;
                Canvas.SetLeft(panelRegistration, windowFirst.Width + 100);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLogin)
                btnSelecting.Content = "Логин";
            else
                btnSelecting.Content = "Регистрация";

            Canvas.SetTop(lCapcha, Canvas.GetTop(lTextCapcha));
            Canvas.SetLeft(lCapcha, Canvas.GetLeft(lTextCapcha));


            str_connect.Server = "f0335575.xsph.ru";
            str_connect.UserID = "f0335575_users";
            str_connect.Password = "123456";
            str_connect.SslMode = MySqlSslMode.None;
            str_connect.Database = "f0335575_users";
            str_connect.PersistSecurityInfo = true;
            conn.ConnectionString = str_connect.ToString();
            Config.ReadSettings();
            if (Config.Remember && Config.Savelogin != "" && Config.SavePass != "")
            {
                cbMemory.IsChecked = true;
                tbLogin.Text = Config.Savelogin;
                tbPassword.Password = Config.SavePass;
                Button_Click_1(null, null);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            panelLogin.Visibility = Visibility.Hidden;
            Canvas.SetTop(panelRePass, Canvas.GetTop(panelLogin));
            Canvas.SetLeft(panelRePass, Canvas.GetLeft(panelLogin));
            panelRePass.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SQLWorker.Login = tbLogin.Text;
            SQLWorker.Password = tbPassword.Password;
            if (SQLWorker.Autorization())
            {
                this.Visibility = Visibility.Hidden;
                WorkingForm wf = new WorkingForm();
                wf.Show();
                Config.WriteSettings();
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль!");
            }


        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!numberPhone.IsMatch(tbPhone.Text))
            {
                tbPhone.Foreground = Brushes.Red;
                phoneIsValid = false;
            }
            else
            {
                tbPhone.Foreground = Brushes.Green;
                phoneIsValid = true;

            }
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (!email.IsMatch(tbEmail.Text))
            {
                tbEmail.Foreground = Brushes.Red;
                emailIsValid = false;
            }
            else
            {
                tbEmail.Foreground = Brushes.Green;
                emailIsValid = true;
            }
        }

        private void TbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!name.IsMatch(tbName.Text))
            {
                tbName.Foreground = Brushes.Red;
                nameIsValid = false;
            }
            else
            {
                tbName.Foreground = Brushes.Green;
                nameIsValid = true;
            }
        }

        private void TbPass_TextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!strongPass.IsMatch(tbPass.Password))
            {

                labelPass.Content = "Пароль слишком слаб!";
               
                tbPass.Foreground = Brushes.Red;
                tbPass2.Foreground = Brushes.Red;

            }
            else
            {
                if (tbPass2.Password != tbPass.Password)
                {
                    labelPass.Content = "Пароли не то чтобы очень похожи, кнчн!";
                    
                    tbPass.Foreground = Brushes.Red;
                    tbPass2.Foreground = Brushes.Red;
                }
                else
                {
                    if (nameIsValid && phoneIsValid && emailIsValid && loginIsValid && tbCapcha.Text == capcha)
                    {
                        tbPass.Foreground = Brushes.Green;
                        tbPass2.Foreground = Brushes.Green;
                        labelPass.Content = "";

                        Thread registrationThr = new Thread(registration);
                        registrationThr.Start();

                        
                    }
                    else
                    {
                        MessageBox.Show("Проверьте введённые данные!");
                    }
                    
                }
            }
        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {
            if (!name.IsMatch(tbLoginNew.Text))
            {
                tbLoginNew.Foreground = Brushes.Red;
                loginIsValid = false;
            }
            else
            {
                tbLoginNew.Foreground = Brushes.Green;
                loginIsValid = true;
            }
        }

        private void registration()
        {
            string login = "";
            string pass = "";
            string email = "";
            string name = "";
            string phone = "";
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                login = tbLoginNew.Text;
                pass = tbPass.Password;
                email = tbEmail.Text;
                name = tbName.Text;
                phone = tbPhone.Text;
            }));
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                SQLWorker.SQL = "SELECT COUNT(email), COUNT(log) FROM users WHERE email = '" + email + "' OR log = '"+login+"'";
                List<List<string>> count = SQLWorker.SQLQuery();

                if (Convert.ToInt32(count[0][0]) == 0)
                {

                    while (conn.State != ConnectionState.Open)
                    {
                        try
                        {

                            SQLWorker.SQL = "INSERT INTO users(log,pass,email,name,phone) VALUES('" + login + "','" +
                                pass + "','" + email + "','" + name + "','" + phone + "')";
                            SQLWorker.SQLNonQuery();

                            Application.Current.Dispatcher.Invoke((Action)(() =>
                            {
                                BtnSelecting_Click(null, null);
                                tbLoginNew.Text = "";
                                tbPass.Password = "";
                                tbEmail.Text = "";
                                tbName.Text = "";
                                tbPhone.Text = "";
                            }));
                            conn.Close();
                            try
                            {
                                Thread.CurrentThread.Abort();
                            }
                            catch
                            {
                                MessageBox.Show("Регистрация прошла успешно!");
                            }

                        }
                        catch (MySqlException ex)
                        {
                            MessageBox.Show(ex.Message);
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Данный адрес электронной почты или логин занят!");
                }

            }
            else
            {
                MessageBox.Show("Нет соединения!");
            }
        }

        private void Recapcha(object sender, MouseButtonEventArgs e) 
        {
            capcha = "";
            string capchaTrash = "";
            Random ran = new Random();
            for (int i = 0; i < 10; i++)
            {
                capcha += (char)ran.Next(65, 88);
                capchaTrash += "-";
            }
            lTextCapcha.Content = capcha;
            lCapcha.Content = capchaTrash;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SendMail sm = new SendMail(tbEmailPass.Text,"");
            Thread send = new Thread(new ThreadStart(sm.SendMessageAboutPass));
            send.Start();

        }

        private void CbMemory_Checked(object sender, RoutedEventArgs e)
        {
            Config.Remember = true;
        }
    }
}
