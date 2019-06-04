using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

namespace MMM
{
    public static class ConnectionClass
    {
        //static string query = "";
        public static string con1 = ConfigurationManager.ConnectionStrings["databaseConnectionString"].ConnectionString;
        public static string con2 = ConfigurationManager.ConnectionStrings["databaseConnectionStringForTasks"].ConnectionString;

        //users from the user table
        public static List<User> ConnectToUsersTable()
        {
            User user;
            List<User> users = new List<User>();
            //string query = "SELECT * FROM users WHERE users.user_name NOT LIKE \"admin\" ORDER BY RAND()";

            string query = "SELECT * FROM users WHERE users.user_name NOT LIKE \"admin\" && rdmdb.users.privilege_id <> 3 ORDER BY RAND()";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(con1))
                {
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                user = new User()
                                {
                                    UserId = reader.GetInt16("id"),
                                    UserName = reader.GetString("user_name"),
                                    //UserDepartment = reader.GetString("department")//,
                                    //UserPicture = reader.GetString("image"),
                                    //UserPrivilege = reader.GetString("privilege")
                                };
                                users.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
            return users;
        }

        //tasks for the selected person
        public static List<Task> ConnectToUnifishedTaskTable(string param)
        {
            Task task;
            List<Task> tasks = new List<Task>();
            string query = "select tasks.description, users.user_name from tasks Left Join tasks_users on tasks.id = tasks_users.tasks_id Left Join users on tasks_users.users_id = users.id WHERE users.user_name LIKE ?param && tasks.finished = 0";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(con1))
                {
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("?param", param); //TODO: sredi parametre da moze da se upise u listu
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                task = new Task()
                                {
                                    //TaskId = reader.GetInt16("id"),
                                    TaskDescription = reader.GetString("description")//,
                                    //TaskFinished = reader.GetInt16("finished"),
                                    //TaskDateStarted = reader.GetDateTime("date_started"),
                                    //TaskDeadline = reader.GetDateTime("end_date")//,
                                    //TaskDateFinished = reader.GetDateTime("date_finished")//,
                                    //TaskDeviceId = reader.GetInt16("id"),
                                    //TaskUserId = reader.GetInt16("id")
                                };
                                tasks.Add(task);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
            return tasks;
        }

        //selection for tasks on hold
        public static List<Task> ConnectToTaskTableOnHold(string param)
        {
            Task task;
            List<Task> tasks = new List<Task>();
            string query = "select tasks.description, users.user_name from tasks Left Join tasks_users on tasks.id = tasks_users.tasks_id Left Join users on tasks_users.users_id = users.id WHERE users.user_name LIKE ?param && tasks.finished = 2";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(con1))
                {
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("?param", param); //TODO: sredi parametre da moze da se upise u listu
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                task = new Task()
                                {
                                    //TaskId = reader.GetInt16("id"),
                                    TaskDescription = reader.GetString("description")//,
                                    //TaskFinished = reader.GetInt16("finished"),
                                    //TaskDateStarted = reader.GetDateTime("date_started"),
                                    //TaskDateFinished = reader.GetDateTime("date_finished")//,
                                    //TaskDeviceId = reader.GetInt16("id"),
                                    //TaskUserId = reader.GetInt16("id")
                                };
                                tasks.Add(task);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
            return tasks;
        }

        //tasks from the technical department
        public static DataSet ConnectToNEWTaskTable()
        {
            string qry = "SELECT categories.name AS CATEGORY, persons.firstname AS NAME, devices.device_type AS DEVICE, reports.serialnumber AS SERIALNUM, reports.description AS DESCRIPTION, reports.deadline_date AS DEADLINE FROM categories, persons, devices, reports WHERE reports.category_id = categories.id && reports.user_id = persons.id && reports.device_id = devices.id";
            DataSet ds = new DataSet();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(con2))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(qry, conn))
                    {
                        conn.Open();
                        adapter.Fill(ds, "model");
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Could not retrieve data: no valid connection hosts");
                //MessageBox.Show(ex.ToString());
                Debug.Write(ex.ToString());
                //Debug.Write(ex.ToString());
            }
            return ds;
        }

        //first table to be visible
        public static List<User> ShowDataTable()
        {
            //string qry = "SELECT morning_meeting.users.user_name AS NAME, morning_meeting.users.department AS DEPARTMENT, morning_meeting.tasks.description AS TASK, morning_meeting.tasks.date_started AS BEGINDATE, morning_meeting.tasks.date_finished AS ENDDATE, morning_meeting.devices.name AS DEVICE FROM morning_meeting.users, morning_meeting.tasks, morning_meeting.devices";
            string qry = "select users.user_name AS NAME, department.department_name AS DEPARTMENT from users left join department on department.id = users.department_id where users.user_name not like 'admin' && rdmdb.users.privilege_id <> 3";

            List<User> ds = new List<User>();
            User users;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(con1))
                {
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand(qry, conn))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users = new User()
                                {
                                    UserName = reader.GetString("name"),
                                    UserDepartment = reader.GetString("department")
                                };
                                ds.Add(users);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Could not retrieve data: no valid connection hosts");
               //MessageBox.Show(ex.ToString());
                Debug.Write(ex.ToString());
                //Debug.Write(ex.ToString());
            }
            return ds;
        }

        //int for creating rows in the starting table
        public static int Countrows()
        {
            //string qry = "SELECT morning_meeting.users.user_name AS NAME, morning_meeting.users.department AS DEPARTMENT, morning_meeting.tasks.description AS TASK, morning_meeting.tasks.date_started AS BEGINDATE, morning_meeting.tasks.date_finished AS ENDDATE, morning_meeting.devices.name AS DEVICE FROM morning_meeting.users, morning_meeting.tasks, morning_meeting.devices";
            string qry = "select COUNT(users.user_name) from tasks Left Join tasks_users on tasks.id = tasks_users.tasks_id Left Join devices_tasks on tasks.id = devices_tasks.tasks_id Left Join users on tasks_users.users_id = users.id left join devices on devices_tasks.devices_id = devices.id";
            int ds = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(con1))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                    {
                        ds = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Could not retrieve data: no valid connection hosts");
                //MessageBox.Show(ex.ToString());
                Debug.Write(ex.ToString());
            }
            return ds;
        }

        //list of devices available
        public static List<Device> ConnectToDeviceTable()
        {
            Device device;
            List<Device> devices = new List<Device>();
            string query = "select * from devices";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(con1))
                {
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                device = new Device()
                                {
                                    DeviceId = reader.GetInt16("id"),
                                    DeviceName = reader.GetString("device_name")//,
                                };
                                devices.Add(device);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
            return devices;
        }

        //list of tasks available
        public static List<Task> ConnectToTaskTable()
        {
            Task task;
            List<Task> tasks = new List<Task>();
            string query = "select * from projects";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(con1))
                {
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                task = new Task()
                                {
                                    TaskId = reader.GetInt16("id"),
                                    TaskDescription = reader.GetString("task_name")//,
                                };
                                tasks.Add(task);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
            return tasks;
        }

        //new projects and tasks
        public static bool InsertIntoTaskTable(Task task, bool dp, User user, bool tb)
        {
            //spoji ovo sa covekom taman
            //MessageBox.Show(projects.TaskName);
            bool executed = false;
            string query1 = "INSERT INTO `projects`(`task_name`) VALUES (?param1)";
            string query2 = "INSERT INTO `tasks`(`description`, `date_started`, `end_date`, `tasks_id`) VALUES (?param1, ?param2, ?param3, (SELECT id FROM projects WHERE projects.task_name LIKE ?param4))";
            string query3 = "INSERT INTO `tasks_users`(`tasks_id`, `users_id`) VALUES ((SELECT MAX(id) FROM tasks WHERE tasks.description LIKE ?param1 and tasks.finished = 0), (SELECT id FROM users WHERE users.user_name LIKE ?param2))";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(con1))
                {
                    conn.Open();

                    if (tb)
                    {
                        using (MySqlCommand cmd1 = new MySqlCommand(query1, conn))
                        {
                            cmd1.Parameters.AddWithValue("?param1", task.TaskName);
                            cmd1.ExecuteNonQuery();
                            executed = true;
                            //Debug.Write(query);
                        }
                    }

                    using (MySqlCommand cmd2 = new MySqlCommand(query2, conn))
                    {
                        cmd2.Parameters.AddWithValue("?param1", task.TaskDescription);
                        cmd2.Parameters.AddWithValue("?param2", DateTime.Now);
                        if (!dp)
                        {
                            cmd2.Parameters.AddWithValue("?param3", DBNull.Value);
                        }
                        else
                        {
                            cmd2.Parameters.AddWithValue("?param3", task.TaskDeadline);
                        }
                        cmd2.Parameters.AddWithValue("?param4", task.TaskName);
                        cmd2.ExecuteNonQuery();
                        executed = true;
                        //Debug.Write(query);
                    }

                    using (MySqlCommand cmd3 = new MySqlCommand(query3, conn))
                    {
                        cmd3.Parameters.AddWithValue("?param1", task.TaskDescription);
                        cmd3.Parameters.AddWithValue("?param2", user.UserName);
                        cmd3.ExecuteNonQuery();
                        executed = true;
                        //Debug.Write(query);
                    }
                }
            }
            catch (Exception ex)
            {
                /*string newfolder = direktorijum + "\\Logs";
                if (!Directory.Exists(newfolder)) Directory.CreateDirectory(newfolder);
                newfolder += "\\log13" + DateTime.Now.ToString("dd_MM_yyyy hh_mm_ss") + ".txt";
                StreamWriter sw = new StreamWriter(newfolder);
                sw.Write(ex.ToString());
                sw.Close();*/

                Debug.Write(ex.ToString());
                executed = false;
            }
            return executed;
            //executed = false
        }

        //connect devices and tasks
        public static bool InsertIntoDeviceTable(Task task, Device device)
        {
            //spoji ovo sa covekom taman
            //MessageBox.Show(projects.TaskName);
            bool executed = false;
            string query = "INSERT INTO `devices_tasks`(`devices_id`, `tasks_id`) VALUES ((SELECT id FROM devices WHERE devices.device_name LIKE ?param1), (SELECT id FROM tasks WHERE tasks.description LIKE ?param2 and tasks.finished = 0));";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(con1))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?param1", device.DeviceName);
                        cmd.Parameters.AddWithValue("?param2", task.TaskDescription);
                        cmd.ExecuteNonQuery();
                        executed = true;
                        //Debug.Write(query);
                    }
                }
            }
            catch (Exception ex)
            {
                /*string newfolder = direktorijum + "\\Logs";
                if (!Directory.Exists(newfolder)) Directory.CreateDirectory(newfolder);
                newfolder += "\\log13" + DateTime.Now.ToString("dd_MM_yyyy hh_mm_ss") + ".txt";
                StreamWriter sw = new StreamWriter(newfolder);
                sw.Write(ex.ToString());
                sw.Close();*/

                Debug.Write(ex.ToString());
                executed = false;
            }
            return executed;
            //executed = false
        }

        public static void UpdateTaskEndDate(int parameter, string param)
        {
            string query = "UPDATE tasks SET `finished` = ?param1, `date_finished` = ?param2 WHERE tasks.description = ?param3";
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(con1))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if(parameter != 0)
                            cmd.Parameters.AddWithValue("?param2", DateTime.Now);
                        else if (parameter == 0)
                            cmd.Parameters.AddWithValue("?param2", DBNull.Value);

                        cmd.Parameters.AddWithValue("?param1", parameter);
                        cmd.Parameters.AddWithValue("?param3", param);
                        cmd.ExecuteNonQuery();
                        //Debug.Write(query);
                    }
                }
            }
            catch (Exception ex)
            {
                /*string newfolder = direktorijum + "\\Logs";
                if (!Directory.Exists(newfolder)) Directory.CreateDirectory(newfolder);
                newfolder += "\\log13" + DateTime.Now.ToString("dd_MM_yyyy hh_mm_ss") + ".txt";
                StreamWriter sw = new StreamWriter(newfolder);
                sw.Write(ex.ToString());
                sw.Close();*/

                Debug.Write(ex.ToString());
            }
        }

        //retrieving detailed tasks information
        public static List<Task> SelectFromTasks()
        {
            Task task;
            List<Task> tasks = new List<Task>();
            string query = "select projects.task_name, tasks.description, tasks.date_started, tasks.end_date, tasks.finished from tasks Left Join projects on tasks.tasks_id = projects.id order by tasks.finished";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(con1))
                {
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                task = new Task()
                                {
                                    //TaskName = reader.GetString("task_name"),
                                    TaskDescription = reader.GetString("description"),
                                    /*TaskDateStarted = reader.GetDateTime("date_started"),
                                    TaskDateFinished = reader.GetDateTime("date_finished"),
                                    TaskDeadline = reader.GetDateTime("end_date"),*/
                                    TaskFinished = reader.GetInt16("finished")
                                };
                                tasks.Add(task);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
            return tasks;
        }

        public static void UpdateTaskDescription(string param1, string param2)
        {
            string query = "UPDATE `tasks` SET `description` = ?param1 WHERE (`id` = ?param2)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(con1))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?param1", param1);
                        cmd.Parameters.AddWithValue("?param2", param2);
                        cmd.ExecuteNonQuery();
                        //Debug.Write(query);
                    }
                }
            }
            catch (Exception ex)
            {
                /*string newfolder = direktorijum + "\\Logs";
                if (!Directory.Exists(newfolder)) Directory.CreateDirectory(newfolder);
                newfolder += "\\log13" + DateTime.Now.ToString("dd_MM_yyyy hh_mm_ss") + ".txt";
                StreamWriter sw = new StreamWriter(newfolder);
                sw.Write(ex.ToString());
                sw.Close();*/

                Debug.Write(ex.ToString());
            }
        }
    }
}
