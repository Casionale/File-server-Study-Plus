using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace File_server_Study__
{
    public static class SQLWorker
    {
        static MySqlConnection conn = new MySqlConnection();
        static MySqlCommand cmd = new MySqlCommand();
        private static Regex email = new Regex(@"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)");
        private static Regex login = new Regex(@"^[a-zA-Z][a-zA-Z0-9-_\.]{1,20}$");
        static public string SQL { get; set; }
        static public string Login { get; set; }
        static public string Password { get; set; }
        public static void SQLNonQuery()
        {
            try
            {
                GetConnection();
                cmd.CommandText = SQL;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка соединения с сервером");
            }

        }

        private static void GetConnection()
        {
            MySqlConnectionStringBuilder str_connect = new MySqlConnectionStringBuilder();
            str_connect.Server = "f0335575.xsph.ru";
            str_connect.UserID = "f0335575_users";
            str_connect.Password = "123456";
            str_connect.SslMode = MySqlSslMode.None;
            str_connect.Database = "f0335575_users";
            str_connect.PersistSecurityInfo = true;
            conn.ConnectionString = str_connect.ToString();
            cmd.Connection = conn;
        }

        public static List<List<string>> SQLQuery()
        {
            GetConnection();
            List<List<string>> res = new List<List<string>>();
            cmd.CommandText = SQL;
            conn.Open();
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                List<string> buf = new List<string>();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    buf.Add(dr.GetString(i));
                }
                res.Add(buf);
            }

            dr.Close();
            conn.Close();
            return res;
        }

        public static bool Autorization()
        {
            if ((email.IsMatch(Login) || login.IsMatch(Login)) && Password!="")
            {
                if (email.IsMatch(Login))
                {
                    SQL = "SELECT COUNT(log) FROM users WHERE email = '"+Login+"' AND pass = '"+Password+"'";
                    List<List<string>> buf = SQLQuery();
                    if (Convert.ToInt32(buf[0][0]) == 1)
                        return true;

                }
                else
                {
                    SQL = "SELECT COUNT(log) FROM users WHERE log = '" + Login + "' AND pass = '" + Password + "'";
                    List<List<string>> buf = SQLQuery();
                    if (Convert.ToInt32(buf[0][0]) == 1)
                        return true;
                }
                
            }
            return false;
        }
    }
}
