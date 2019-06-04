using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MMM
{
    public partial class UsersForm : Form
    {
        List<Device> devices = new List<Device>();
        DataSet ds;
        List<User> users = new List<User>();
        Label nameLabel;
        //PictureBox pbUser;
        Panel row;
        List<Task> tasks1;
        List<Task> tasks2;
        List<Task> finishedtasks;
        Task task1 = new Task();
        List<Task> cbTask = new List<Task>();

        List<Task> tableList2;

        Task task = new Task();
        User user;

        List<Tables> tableList;

        Task edited;

        bool datepicked = false;
        bool tb = false;
        bool clicked;

        public UsersForm()
        {
            InitializeComponent();
            try
            {
                edited = new Task();

                tableList = new List<Tables>();
                tableList2 = new List<Task>();

                tasks1 = new List<Task>();
                tasks2 = new List<Task>();
                finishedtasks = new List<Task>();
                user = new User();

                tableList = TableItemsClass.GetTables();
                tableList2 = ConnectionClass.SelectFromTasks();

                devices = ConnectionClass.ConnectToDeviceTable();
                foreach (Device d in devices)
                    //MessageBox.Show(d.DeviceName);
                    cbDevices.Items.Add(d.DeviceName);

                cbTask = ConnectionClass.ConnectToTaskTable();
                foreach (Task t in cbTask)
                    //MessageBox.Show(d.DeviceName);
                    cbTasks.Items.Add(t.TaskDescription);

                ds = new DataSet();

                timer1.Enabled = true;

                dtLabel.Text = DateTime.Now.ToString("dd.MM.yyyy.  hh:mm:ss");

                WindowState = FormWindowState.Maximized;

                clicked = false;

                /*tableLayoutPanel1.MaximumSize = new Size(tableLayoutPanel1.Width, tableLayoutPanel1.Height);
                tableLayoutPanel1.AutoScroll = true;*/

                GenerateTable(tableList.Count, tableList, tableLayoutPanel1, 2);
                GenerateProjects(tableList2.Count, tableList2, tableLayoutPanel2, 2);

                //tableLayoutPanel1.AutoScroll = false;
            }
            catch { }
        }

        public void LoadList()
        {
            try
            {
                //int lastuser = users.Count - 1;

                users = ConnectionClass.ConnectToUsersTable();

                if (users.Count > 0)
                {
                    //users.RemoveAt(0);

                    int i = 0;

                    foreach (User u in users)
                    {
                        i++;

                        //MessageBox.Show(u.UserName + " " + u.UserDepartment);
                        row = new Panel();
                        row.Dock = DockStyle.Top;
                        //row.Size = new Size(200, UIpanel.Height/users.Count);
                        row.Height = UIpanel.Height / users.Count;
                        row.Visible = true;
                        row.Show();

                        /*pbUser = new PictureBox();
                         pbUser.Dock = DockStyle.Left;
                         pbUser.Visible = true;
                         pbUser.Show();*/

                        nameLabel = new Label();
                        nameLabel.AutoSize = false;
                        nameLabel.Dock = DockStyle.Fill;
                        //nameLabel.Name = u.UserName + i.ToString();
                        nameLabel.Text = u.UserName;
                        nameLabel.Visible = true;
                        nameLabel.Font = new Font("Microsoft Sans Serif", 20);
                        nameLabel.Show();
                        nameLabel.Click += new System.EventHandler(this.nameLabel_Click);

                        row.Controls.Add(nameLabel);
                        //row.Controls.Add(pbUser);

                        UIpanel.Controls.Add(row);
                    }
                }
            }
            catch { }
        }

        private void nameLabel_Click(object sender, EventArgs e)
        {
            try
            {
                Label lbl = sender as Label;

                labButtSelected.Text = lbl.Text;
                tasks1 = ConnectionClass.ConnectToUnifishedTaskTable(lbl.Text);
                tasks2 = ConnectionClass.ConnectToTaskTableOnHold(lbl.Text);

                user.UserName = lbl.Text;

                lbl.BackColor = Color.MistyRose;

                listBox1.Items.Clear();
                listBox2.Items.Clear();

                if (tasks1.Count > 0)
                {
                    foreach (Task t in tasks1)
                        //MessageBox.Show(t.TaskDescription);
                        listBox1.Items.Add(t.TaskDescription);
                }
                if (tasks2.Count > 0)
                {
                    foreach (Task t in tasks2)
                        //MessageBox.Show(t.TaskDescription);
                        listBox2.Items.Add(t.TaskDescription);
                }

                clicked = true;
            }
            catch { }
        }

        private void UsersForm_Load(object sender, EventArgs e)
        {
            LoadList();
        }

        private void labMM_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show(richTextBox1.Text);
                if (!String.IsNullOrEmpty(richTextBox1.Text) && clicked)
                {
                    task.TaskDescription = richTextBox1.Text;
                    if (datepicked)
                    {
                        task.TaskDeadline = dateTimePicker.Value;
                    }
                    if (!String.IsNullOrEmpty(textBox1.Text))
                    {
                        task.TaskName = textBox1.Text;
                        tb = true;
                    }
                    else if (cbTasks.SelectedItem != null)
                    {
                        task.TaskName = cbTasks.SelectedItem.ToString();
                        //tb = false;
                    }

                    if (ConnectionClass.InsertIntoTaskTable(task, datepicked, user, tb))
                    {
                        Device device = new Device();

                        if (cbDevices.SelectedItem != null)
                        {
                            device.DeviceName = cbDevices.SelectedItem.ToString();
                            ConnectionClass.InsertIntoDeviceTable(task, device);
                        }

                        listBox1.Items.Add(richTextBox1.Text);

                        if (tb)
                        {
                            cbTasks.Items.Add(textBox1.Text);
                        }

                        cbDevices.SelectedIndex = -1;
                        cbTasks.SelectedIndex = -1;

                        richTextBox1.Text = "";
                        textBox1.Text = "";

                        tb = false;
                        datepicked = false;
                    }
                    else
                    {
                        MessageBox.Show("Error with the database; cannot insert data");
                        tb = false;
                        datepicked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                tb = false;
                datepicked = false;
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {
                    listBox2.Items.Add(listBox1.SelectedItem);
                    /*istBox1.SelectedItem = null;*/


                    ConnectionClass.UpdateTaskEndDate(2, listBox1.SelectedItem.ToString());

                    int i = listBox1.SelectedIndex;
                    listBox1.Items.RemoveAt(i);
                }
            }
            catch(Exception ex)
            { }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {
                    /*istBox1.SelectedItem = null;*/

                    ConnectionClass.UpdateTaskEndDate(1, listBox1.SelectedItem.ToString());

                    int i = listBox1.SelectedIndex;
                    listBox1.Items.RemoveAt(i);
                }
            }
            catch { }
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox2.SelectedItem != null)
                {
                    listBox1.Items.Add(listBox2.SelectedItem);
                    /*istBox1.SelectedItem = null;*/

                    ConnectionClass.UpdateTaskEndDate(0, listBox2.SelectedItem.ToString());

                    int i = listBox2.SelectedIndex;
                    listBox2.Items.RemoveAt(i);
                }
            }
            catch { }
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            datepicked = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dtLabel.Text = DateTime.Now.ToString("dd.MM.yyyy.  hh:mm:ss");
        }

        private void GenerateTable(int rowCount, List<Tables> ticList, TableLayoutPanel tlp, int columnCount)
        {
            //int columnCount = 3;
            tlp.Controls.Clear();
            tlp.ColumnStyles.Clear();
            tlp.RowStyles.Clear();
            tlp.ColumnCount = columnCount;
            tlp.RowCount = rowCount;

            int labHeight = 0;

            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            //DateTime date1 = new DateTime(2001, 1, 1, 0, 0, 0);
            for (int x = 0; x < rowCount; x++)
            {
                //int warning = 0;
                List<string> tmpData = new List<string>();
                Tables tmpInfo = ticList.ElementAt(x);
                tmpData.Add(tmpInfo.UserName);
                //tmpData.Add(tmpInfo.UserDepartment);
                tmpData.Add(tmpInfo.TaskDescription);

                for (int y = 0; y < columnCount; y++) //podesavanje sirine tabele
                {
                    Label lab = new Label();
                    lab.Text = tmpData.ElementAt(y);
                    int w = GetWfromY(y);

                    lab.AutoSize = false;
                    lab.Size = new Size(w, 180);
                    lab.Font = new System.Drawing.Font("Verdana", 20, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
                    lab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));

                    //if (y == 2)
                    lab.TextAlign = ContentAlignment.MiddleLeft;
                    //else
                    //lab.TextAlign = ContentAlignment.MiddleCenter;

                    lab.Dock = DockStyle.Fill;
                    lab.Margin = new Padding(0, 0, 0, 0);


                    if (x % 2 == 0)
                    {
                        lab.BackColor = Color.LightGray;
                    }

                    tlp.Controls.Add(lab, y, x);

                    labHeight = lab.Size.Height;
                }

            }

            //tlp.Size = new Size(panel39.Size.Width, (rowCount + 15) * labHeight);

            //tlpPeople.Size = new Size(panel, (list.Count() + 1) * labHeight);
        }

        private void GenerateProjects(int rowCount, List<Task> ticList, TableLayoutPanel tlp, int columnCount)
        {
            //int columnCount = 3;
            tlp.Controls.Clear();
            tlp.ColumnStyles.Clear();
            tlp.RowStyles.Clear();
            tlp.ColumnCount = columnCount;
            tlp.RowCount = rowCount;

            int labHeight = 0;

            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            //DateTime date1 = new DateTime(2001, 1, 1, 0, 0, 0);
            for (int x = 0; x < rowCount; x++)
            {
                //int warning = 0;
                List<string> tmpData = new List<string>();
                Task tmpInfo = ticList.ElementAt(x);
                //tmpData.Add(tmpInfo.TaskName);
                tmpData.Add(tmpInfo.TaskDescription);
                /*tmpData.Add(tmpInfo.TaskDateStarted.ToString("dd.MM.yyyy.  hh:mm:ss"));
                tmpData.Add(tmpInfo.TaskDeadline.ToString("dd.MM.yyyy.  hh:mm:ss"));
                tmpData.Add(tmpInfo.TaskDateFinished.ToString("dd.MM.yyyy.  hh:mm:ss"));*/
                if(tmpInfo.TaskFinished == 0)
                {
                    tmpData.Add("In progress");
                }
                else if (tmpInfo.TaskFinished == 1)
                {
                    tmpData.Add("Finished");
                }
                else if (tmpInfo.TaskFinished == 2)
                {
                    tmpData.Add("On hold");
                }

                for (int y = 0; y < columnCount; y++) //podesavanje sirine tabele
                {
                    Label lab = new Label();
                    lab.Text = tmpData.ElementAt(y);
                    //int w = GetWfromY(y);

                    lab.AutoSize = false;
                    lab.Size = new Size(panel40.Width / 2, 30);
                    lab.Font = new System.Drawing.Font("Verdana", 20, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
                    lab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));

                    if (y == 1)
                        lab.TextAlign = ContentAlignment.MiddleCenter;
                    else
                        lab.TextAlign = ContentAlignment.MiddleLeft;

                    lab.Dock = DockStyle.Fill;

                    lab.Margin = new Padding(0, 0, 0, 0);


                    if (x % 2 == 0)
                    {
                        lab.BackColor = Color.LightGray;
                    }

                    tlp.Controls.Add(lab, y, x);

                    labHeight = lab.Size.Height;
                }

            }

            //tlp.Size = new Size(panel39.Size.Width, (rowCount + 15) * labHeight);

            //tlpPeople.Size = new Size(panel, (list.Count() + 1) * labHeight);
        }

        private int GetWfromY(int y) //podesavanje sirine u odnosu na broj kolona
        {
            if (y == 0)
                return 160;
            else if (y == 1)
                return 360;
            else if (y == 2)
                return 100;
            else if (y == 3)
                return 260;
            else if (y == 4)
                return 260;
            else if (y == 5)
                return 200;
            else if (y == 6)
                return 210;
            else if (y == 7)
                return 180;
            else if (y == 8)
                return 170;
            else
                return 0;
        }

        private void dtLabel_Click(object sender, EventArgs e)
        {
            /*AdminForm usersForm = new AdminForm();
            usersForm.Show();
            this.Hide();*/
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
           /*if (listBox1.SelectedItem != null)
            {
                edited.TaskDescription = listBox1.SelectedItem.ToString();
                EditForm editForm = new EditForm(edited.TaskDescription);
                editForm.ShowDialog();
            }*/
        }

    }
}
