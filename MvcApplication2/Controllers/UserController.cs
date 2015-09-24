using System;
using System.Diagnostics;
using System.Web.Http;
using MvcApplication2.Models;
using MySql.Data.MySqlClient;

namespace MvcApplication2.Controllers
{
    public class UserController : ApiController
    {
#if DEBUG
        private const string ConnectionString =
            "SERVER=mysql2111.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
#else
            private const string ConnectionString = "SERVER=mysql2111int.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
        #endif

        private static MySqlConnection _connection;
        private bool _newUser;
        private bool updateTimes;

        private string lastDate = "";
        private int dayTotal;

        // GET api/user
        public String GetPhoneUsers(string id)
        {
            var i = 0;
            try
            {
                _connection = new MySqlConnection(ConnectionString);


                var query = string.Format("SELECT * from WP_GAA_Users where Unique_ID like '{0}';", id);

                using (_connection)
                {
                    _connection.Open();
                    using (var cmd = new MySqlCommand(query, _connection))
                    {
                        using (var r = cmd.ExecuteReader())
                        {
                            if (!r.HasRows)
                            {
                                _newUser = true;
                            }
                            else
                            {
                                while (r.Read())
                                {
                                    i = r.GetInt32("Times_Used") + 1;
                                    updateTimes = true;
                                }
                            }
                        }
                    }
                }

                if (updateTimes)
                {
                    UpdateTimesUsed(i, id);
                }

                if (_newUser)
                {
                    InsertUser(id);
                }

                UpdateCountToday();
            }

            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

            return "1603";
        }


        private void UpdateTimesUsed(int i, string id)
        {
            try
            {
                var query = "UPDATE WP_GAA_Users SET Times_Used='" + i + "', Last_Used= '" + DateTime.Now +
                            "' WHERE Unique_ID = '" + id + "' ;";

                using (_connection)
                {
                    _connection.Open();
                    using (var cmd = new MySqlCommand(query, _connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
        }

        private static void InsertUser(string id)
        {
            try
            {
                const string query =
                    "INSERT INTO WP_GAA_Users (Unique_ID, Date_Entered, Times_Used) VALUES (@id, @date, @times);";

                using (_connection)
                {
                    _connection.Open();
                    using (var cmd = new MySqlCommand(query, _connection))
                    {
                        // Start using the passed values in our parameters:
                        cmd.Parameters.AddWithValue("@id", id);

                        cmd.Parameters.AddWithValue("@date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@times", 1);

                        // Execute the query
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
        }

        public String GetPhoneUsersWitModel(string id, string model)
        {
            var modelNull = false;
            var lastLogin = "";

            var i = 0;
            try
            {
                _connection = new MySqlConnection(ConnectionString);


                var query = string.Format("SELECT * from WP_GAA_Users where Unique_ID like '{0}';", id);

                using (_connection)
                {
                    _connection.Open();
                    using (var cmd = new MySqlCommand(query, _connection))
                    {
                        using (var r = cmd.ExecuteReader())
                        {
                            if (!r.HasRows)
                            {
                                _newUser = true;
                            }
                            else
                            {
                                while (r.Read())
                                {
                                    var ordinal = r.GetOrdinal("Model");
                                    if (r.IsDBNull(ordinal))
                                    {
                                        modelNull = true;
                                    }
                                    i = r.GetInt32("Times_Used") + 1;

                                    updateTimes = true;
                                }
                            }
                        }
                    }
                }

                if (updateTimes)
                {
                    UpdateTimesUsed(i, id);
                }

                if (_newUser)
                {
                    InsertUserWithModel(id, model);
                }
                if (modelNull)
                {
                    InsertModel(model, id);
                }
                if (id != "59AC8239E2CE2B6116A00549986CBBF58DB700D0" && id != "92A0CB490AFA78C63723C921090648D9050575B2" &&
                    id != "747B38EBCF294EA96067C8312455CAA7344C3ADB" && model != "Microsoft XDeviceEmulator")
                {
                    UpdateCountToday();
                }
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

            return "1603";
        }

        private void InsertModel(string model, string id)
        {
            try
            {
                var query = "UPDATE WP_GAA_Users SET Model= '" + model + "' WHERE Unique_ID = '" + id + "' ;";

                using (_connection)
                {
                    _connection.Open();
                    using (var cmd = new MySqlCommand(query, _connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
        }

        private static void InsertUserWithModel(string id, string m)
        {
            try
            {
                const string query =
                    "INSERT INTO WP_GAA_Users (Unique_ID, Date_Entered, Model, Times_Used) VALUES (@id, @date, @model, @times);";

                using (_connection)
                {
                    _connection.Open();
                    using (var cmd = new MySqlCommand(query, _connection))
                    {
                        // Start using the passed values in our parameters:
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@model", m);
                        cmd.Parameters.AddWithValue("@date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@times", 1);

                        // Execute the query
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
        }

        private void UpdateCountToday()
        {
            var lastTotal = 0;
            var count = GetCount();
            try
            {
                if (count == 0)
                {
                    count++;
                    var query = "UPDATE GAAUsersToday SET Count='" + count + "', Date='" +
                                DateTime.Now.ToShortDateString() + "' WHERE Id = 1 ;";

                    using (_connection)
                    {
                        _connection.Open();
                        using (var cmd = new MySqlCommand(query, _connection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                    query = "SELECT * from GAAUsersToday WHERE Id = 2;";
                    using (_connection)
                    {
                        _connection.Open();
                        using (var cmd = new MySqlCommand(query, _connection))
                        {
                            using (var r = cmd.ExecuteReader())
                            {
                                while (r.Read())
                                {
                                    lastTotal = r.GetInt32("Count");
                                }
                            }
                        }
                    }
                    if (dayTotal > lastTotal)
                    {
                        query = "UPDATE GAAUsersToday SET Count='" + dayTotal + "', Date='" + lastDate +
                                "' WHERE Id = 2 ;";
                        using (_connection)
                        {
                            _connection.Open();
                            using (var cmd = new MySqlCommand(query, _connection))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                else
                {
                    count++;
                    var query = "UPDATE GAAUsersToday SET Count='" + count + "' WHERE Id = 1 ;";

                    using (_connection)
                    {
                        _connection.Open();
                        using (var cmd = new MySqlCommand(query, _connection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertRoadwatchErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
        }

        private int GetCount()
        {
            var i = 0;
            try
            {
                _connection = new MySqlConnection(ConnectionString);

                var query = "SELECT * from GAAUsersToday Where Id = 1 ;";

                using (_connection)
                {
                    _connection.Open();
                    using (var cmd = new MySqlCommand(query, _connection))
                    {
                        using (var r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                i = r.GetInt32("Count");
                                lastDate = r.GetString("Date");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertRoadwatchErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
            dayTotal = i;
            return DateTime.Parse(lastDate).ToShortDateString() == DateTime.Now.ToShortDateString() ? i : 0;
        }
    }
}