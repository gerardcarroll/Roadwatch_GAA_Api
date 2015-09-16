using System;
using System.Diagnostics;
using System.Web.Http;
using MvcApplication2.Models;
using MySql.Data.MySqlClient;

namespace MvcApplication2.Controllers
{
    public class RoadwatchUserController : ApiController
    {
#if DEBUG
        private const string ConnectionString =
            "SERVER=mysql2111.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
#else
        private const string ConnectionString = "SERVER=mysql2111int.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
#endif
        private static MySqlConnection _connection;
        private bool _newUser;
        private string lastDate = "";
        private int dayTotal;

        // GET api/user
        public String Get(string id, string model)
        {
            var i = 0;
            var updateTimes = false;
            try
            {
                _connection = new MySqlConnection(ConnectionString);

                var query = string.Format("SELECT * from Roadwatch_Users where Unique_ID like '{0}';", id);

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
                    InsertUser(id, model);
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
                Database.InsertRoadwatchErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }

            return "1120";
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
                    var query = "UPDATE RoadwatchUsersToday SET Count='" + count + "', Date='" +
                                DateTime.Now.ToShortDateString() + "' WHERE Id = 1 ;";

                    using (_connection)
                    {
                        _connection.Open();
                        using (var cmd = new MySqlCommand(query, _connection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                    query = "SELECT * from RoadwatchUsersToday WHERE Id = 2;";
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
                        query = "UPDATE RoadwatchUsersToday SET Count='" + dayTotal + "', Date='" + lastDate +
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
                    var query = "UPDATE RoadwatchUsersToday SET Count='" + count + "' WHERE Id = 1 ;";

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

                var query = "SELECT * from RoadwatchUsersToday Where Id = 1 ;";

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

        private void UpdateTimesUsed(int i, string id)
        {
            try
            {
                var query = "UPDATE Roadwatch_Users SET Times_Used='" + i + "', Last_Used= '" + DateTime.Now +
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

        private static void InsertUser(string id, string m)
        {
            try
            {
                const string query =
                    "INSERT INTO Roadwatch_Users (Unique_ID, Date_Entered, Model, Times_Used) VALUES (@id, @date, @model, @times);";

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
                Database.InsertRoadwatchErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
        }
    }
}