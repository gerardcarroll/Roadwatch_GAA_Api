using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MvcApplication2.Models;
using MySql.Data.MySqlClient;

namespace MvcApplication2.Controllers
{
    public class TrafficCamController : ApiController
    {
        #if DEBUG
            private const string ConnectionString = "SERVER=mysql2111.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
        #else
            private const string ConnectionString = "SERVER=mysql2111int.cp.blacknight.com;DATABASE=db1305421_wpdev;UID=u1305421_wpdev;PASSWORD=ggc12003/;PORT=3306;pooling=true;Convert Zero Datetime=true;";
        #endif

        readonly MySqlConnection _connection = new MySqlConnection(ConnectionString);
        public List<Camera> GetCameras()
        {
            List<Camera> cameras = new List<Camera>();

            try
            {
                _connection.Open();

                var sql = string.Format("SELECT * from Roadwatch_Cameras order by ID");

                using (var cmd = new MySqlCommand(sql, _connection))
                {
                    using (var r = cmd.ExecuteReader())
                    {
                         while (r.Read())
                         {
                             Camera camera = new Camera
                                             {
                                                 Area = r.GetString("Area"),
                                                 Id = r.GetInt32("Id"),
                                                 Junction = r.GetString("Junction"),
                                                 Url = r.GetString("Url")
                                             };
                             cameras.Add(camera);
                         }
                    }
                }
            }
            catch (Exception ex)
            {
                var stackFrame = new StackFrame();
                var methodBase = stackFrame.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
            finally
            {
                _connection.Close();
            }

            return cameras;
        }
         public List<Camera> GetCamerasNew(string id)
        {
            List<Camera> cameras = new List<Camera>();
            string sql = "";

            try
            {
                _connection.Open();

                sql = string.Format("SELECT * from Roadwatch_Cameras order by ID");

                using (var cmd = new MySqlCommand(sql, _connection))
                {
                    using (var r = cmd.ExecuteReader())
                    {
                         while (r.Read())
                         {
                             Camera camera = new Camera
                                             {
                                                 Area = r.GetString("Area"),
                                                 Id = r.GetInt32("Id"),
                                                 Junction = r.GetString("Junction"),
                                                 Url = r.GetString("Url"),
                                                 Fav = false
                                             };
                             cameras.Add(camera);
                         }
                    }
                }
                
                sql = string.Format("SELECT * from Roadwatch_CamFav Where Device_Id like '" + id + "' order by ID");
                List<Camera> favCams = new List<Camera>();
                using (var cmd = new MySqlCommand(sql, _connection))
                {
                    using (var r = cmd.ExecuteReader())
                    {
                         while (r.Read())
                         {
                             Camera camera = new Camera
                                             {
                                                 Id = r.GetInt32("Camera_Id")
                                             };
                             Camera q = (from c in cameras
                                         where c.Id == camera.Id
                                         select c).FirstOrDefault();
                             cameras.Remove(q);
                             favCams.Add(camera);
                         }
                    }
                }

                foreach (var c in favCams)
                {
                    sql = string.Format("SELECT * from Roadwatch_Cameras Where Id like '" + c.Id + "' order by ID");
                    
                    using (var cmd = new MySqlCommand(sql, _connection))
                    {
                        using (var r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                Camera camera = new Camera
                                {
                                    Area = "Favourites",
                                    Id = r.GetInt32("Id"),
                                    Junction = r.GetString("Junction"),
                                    Url = r.GetString("Url"),
                                    Fav = true
                                };
                                cameras.Insert(0, camera);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var stackFrame = new StackFrame();
                var methodBase = stackFrame.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
            finally
            {
                _connection.Close();
            }

            return cameras;
        }

        [HttpGet]
        public string UpdateFavCams(string cam, string unId)
        {
            bool remove = false;
            bool add = false;

            try
            {
                _connection.Open();

                var sql = string.Format("SELECT * from Roadwatch_CamFav Where Device_Id like '" + unId + "' and Camera_Id like '" + cam + "'");
                using (var cmd = new MySqlCommand(sql, _connection))
                {
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            remove = true;
                        }
                        else
                        {
                            add = true;
                        }
                        
                    }
                }

                if (add)
                {
                    AddCam(cam, unId);
                }
                if(remove)
                {
                    RemoveCam(cam, unId);
                }
                
            }
            catch (Exception ex)
            {
                var stackFrame = new StackFrame();
                var methodBase = stackFrame.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
            finally
            {
                _connection.Close();
            }

            return "ok";
        }

        private void AddCam(string cam, string unId)
        {
            try
            {
                string query = "INSERT INTO Roadwatch_CamFav (Device_ID, Camera_Id) VALUES (@id, @cam);";

                using (var cmd = new MySqlCommand(query, _connection))
                {
                    // Start using the passed values in our parameters:
                    cmd.Parameters.AddWithValue("@id", unId);
                    cmd.Parameters.AddWithValue("@cam", cam);

                    // Execute the query
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
        }

        private void RemoveCam(string cam, string unId)
        {
            string sql = "Delete from Roadwatch_CamFav where Device_Id like '" + unId + "' and Camera_Id like '" + cam + "' ";

            try
            {
                using (var cmd = new MySqlCommand(sql, _connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                var sf = new StackFrame();
                var methodBase = sf.GetMethod();
                Database.InsertErrorToDb(methodBase.Name, ex.Message, ex.ToString());
            }
        }
   
    }
}
