using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMM
{
    public static class TableItemsClass
    {
        static public List<Tables> GetTables()
        {
            string task = "";
            List<Tables> tables = new List<Tables>();

            Tables table;

            List<User> users = new List<User>();

            List<Task> tasks = new List<Task>();

            try
            {
                users = ConnectionClass.ShowDataTable();

                foreach (User u in users)
                {
                    tasks = ConnectionClass.ConnectToUnifishedTaskTable(u.UserName);

                    foreach (Task t in tasks)
                    {
                        task += " - " + t.TaskDescription + "\n";

                    }

                    table = new Tables()
                    {
                        UserName = u.UserName,
                        //UserDepartment = u.UserDepartment,
                        TaskDescription = task
                    };

                    tables.Add(table);

                    task = "";
                }
            }
            catch (Exception ex)
            { }

            return tables;
        }

        static public List<Task> ProjectsOverview()
        {
            List<Task> tasks = new List<Task>();

            return tasks;
        }
    }
}
