using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Net.Mail;
using System.Threading;
using System.Windows;
using System.Text.RegularExpressions;

namespace File_server_Study__
{
    public class SendMail
    {

        public string Email { get; set; }
        public string Message { get; set; }

        public Regex email = new Regex(@"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)");
        public Regex login = new Regex(@"^[a-zA-Z][a-zA-Z0-9-_\.]{1,20}$");
        public SendMail(string email, string message)
        {
            Email = email;
            Message = message;
        }

        public void SendMessageAboutPass()
        {

            if (email.IsMatch(Email) || login.IsMatch(Email))
            {
                if (!email.IsMatch(Email))
                {
                    SQLWorker.SQL = "SELECT email, pass FROM users WHERE log = '" + Email + "'";
                    List<List<string>> buf = SQLWorker.SQLQuery();
                    if (buf.Count != 0)
                    {
                        Email = buf[0][0];
                        Message = "Вы попросили напомнить вам пароль. Вот он: "+buf[0][1];
                    }
                    else Email = "";
                }
                else
                {
                    SQLWorker.SQL = "SELECT email, pass FROM users WHERE email = '" + Email + "'";
                    List<List<string>> buf = SQLWorker.SQLQuery();
                    if (buf.Count != 0)
                    {
                        Email = buf[0][0];
                        Message = "Вы попросили напомнить вам пароль. Вот он: " + buf[0][1];
                    }
                    else Email = "";
                }
                if (Email != "")
                    try
                    {
                        try
                        {
                            MailAddress from = new MailAddress("sudyplus@yandex.ru", "Администратор FTP клиента Sudy +");
                            MailAddress to = new MailAddress(Email);
                            MailMessage m = new MailMessage(from, to);
                            m.Subject = "Восстановление пароля";
                            m.Body = Message;
                            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 25);
                            smtp.Credentials = new System.Net.NetworkCredential("sudyplus@yandex.ru", "Qwerty21");
                            smtp.EnableSsl = true;
                            smtp.Send(m);
                            try
                            {
                                Thread.CurrentThread.Abort();
                            }
                            catch
                            { }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    catch
                    { }
                else
                    MessageBox.Show("Email или логин не найден!");
            }
            else
            {
                MessageBox.Show("Некорректный email или логин!");
            }

        }
    }


}

