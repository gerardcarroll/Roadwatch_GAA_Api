using System;
using System.Net;
using System.Net.Mail;
using MySql.Data.MySqlClient;

namespace MvcApplication2.Models
{
    public class Database
    {
#if DEBUG
        private const string ConnectionString =
            "SERVER=mysql2111.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
#else
                private const string ConnectionString = "SERVER=mysql2111int.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
        #endif

        private static MySqlConnection _connection;

        public static MySqlConnection Connect()
        {
            _connection = new MySqlConnection(ConnectionString);
            _connection.Open();
            return _connection;
        }

        public static void Disconnect(MySqlConnection connection)
        {
            connection.Close();
            connection.Dispose();
        }

        public static void InsertErrorToDb(string p1, string p2, string p3)
        {
            _connection = Connect();

            const string query =
                "INSERT INTO Api_Error (Method, Exception, Date_Time, Full_Ex) VALUES (@method, @exception, @date, @full);";

            using (var cmd = new MySqlCommand(query, _connection))
            {
                // Start using the passed values in our parameters:
                cmd.Parameters.AddWithValue("@method", p1);
                cmd.Parameters.AddWithValue("@exception", p2);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                cmd.Parameters.AddWithValue("@full", p3);

                // Execute the query
                cmd.ExecuteNonQuery();
            }

            if (!p1.Contains("Resize") && !p1.Contains("Heading"))
            {
                EmailError(p1, p2, p3, "GAA API");
            }
        }

        private static void EmailError(string s1, string s2, string s3, string s4)
        {
            var m = new MailMessage();
            var SmtpServer = new SmtpClient("smtp1r.cp.blacknight.com");
            m.From = new MailAddress("apierror@selectunes.eu");
            m.To.Add("gcwpdev@gmail.com");
            m.Subject = s4 + " Error; " + DateTime.Now;
            m.Body = s1 + ";    " + s2 + ";     " + s3;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new NetworkCredential("apierror@selectunes.eu", "Ggc12003/");
            SmtpServer.EnableSsl = true;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

            SmtpServer.Send(m);
        }

        public static void InsertRoadwatchErrorToDb(string p1, string p2, string p3)
        {
            _connection = Connect();

            const string query =
                "INSERT INTO Roadwatch_Api_Error (Method, Exception, DateTime, Full_Ex) VALUES (@method, @exception, @date, @full);";

            using (var cmd = new MySqlCommand(query, _connection))
            {
                // Start using the passed values in our parameters:
                cmd.Parameters.AddWithValue("@method", p1);
                cmd.Parameters.AddWithValue("@exception", p2);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                cmd.Parameters.AddWithValue("@full", p3);

                // Execute the query
                cmd.ExecuteNonQuery();
            }
            EmailError(p1, p2, p3, "Roadwatch API");
        }

        public static void InsertPhoneErrorToDb(string p1, string p2, string p3)
        {
            _connection = Connect();

            const string query =
                "INSERT INTO PhoneApp_Error (Method, Exception, Date_Time, Full_Ex) VALUES (@method, @exception, @date, @full);";

            using (var cmd = new MySqlCommand(query, _connection))
            {
                // Start using the passed values in our parameters:
                cmd.Parameters.AddWithValue("@method", p1);
                cmd.Parameters.AddWithValue("@exception", p2);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                cmd.Parameters.AddWithValue("@full", p3);

                // Execute the query
                cmd.ExecuteNonQuery();
            }
            //dont email errors about schedule service
            if (!p2.Contains("BNS Error"))
            {
                EmailError(p1, p2, p3, "GAA Phone");
            }
        }

        public static void InsertRoadwatchPhoneErrorToDb(string p1, string p2, string p3, string p4)
        {
            _connection = Connect();

            const string query =
                "INSERT INTO Roadwatch_Phone_Error (Method, Exception, DateTime, Full_Ex) VALUES (@method, @exception, @date, @full);";

            using (var cmd = new MySqlCommand(query, _connection))
            {
                // Start using the passed values in our parameters:
                cmd.Parameters.AddWithValue("@method", p1);
                cmd.Parameters.AddWithValue("@exception", p2);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                cmd.Parameters.AddWithValue("@full", p3);
                //cmd.Parameters.AddWithValue("@model", p4);

                // Execute the query
                cmd.ExecuteNonQuery();
            }

            EmailError(p1, p2, p3, "Roadwatch Phone");
        }
    }
}