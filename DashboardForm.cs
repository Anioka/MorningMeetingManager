using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using MySql.Data.Types;
using System.Windows.Forms;

namespace MMM
{
    public partial class DashboardForm : Form
    {
        User user = new User();
        Task task = new Task();
        Device device = new Device();
        List<Tables> ds = new List<Tables>();

        //string warning_image_path;

        int animate = 0; //0-off, 1-on
        int scrollUpOrDown = 0; //0-up, 1-down
        int locY = 0;
        int labHeight = 0;
        int tableScrollSpeed = 1;

        public DashboardForm()
        {
            InitializeComponent();

            ds = TableItemsClass.GetTables();
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);

            /*tlpPeople.Controls.Add(new Label() { AutoSize = true, Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 15), Text = "NAME" }, 0, 0);
            tlpPeople.Controls.Add(new Label() { AutoSize = true, Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 15), Text = "DEPARTMENT" }, 1, 0);
            tlpPeople.Controls.Add(new Label() { AutoSize = true, Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 15), Text = "ASSIGNMENT" }, 2, 0);*/

            GenerateHeader();
            GenerateTable(ds.Count);

            timer1.Enabled = true;

            WindowState = FormWindowState.Maximized;
        }

        /*private void GenerateTable()
        {
            ds = ConnectionClass.ShowDataTable(); 

            /*List<User> people = new List<User>();

            people = ConnectionClass.ConnectToUsersTable();*/

        /*int counter = ConnectionClass.Countrows();

        tlpPeople.RowCount = counter;
        tlpPeople.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        tlpPeople.ColumnCount = 6;
        tlpPeople.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        // tlpPeople.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
        //tlpPeople.ColumnStyles.Clear();
        if (ds.Tables.Count > 0)
        {
            for (int i = 0; i <= counter - 1; i++)
            {
                if (i == 0)
                {
                    tlpPeople.Controls.Add(new Label() { AutoSize = true, Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 15), Text = "NAME" }, 0, 0);
                    tlpPeople.Controls.Add(new Label() { AutoSize = true, Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 15), Text = "DEPARTMENT" }, 1, 0);
                    tlpPeople.Controls.Add(new Label() { AutoSize = true, Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 15), Text = "ASSIGNMENT" }, 2, 0);
                    tlpPeople.Controls.Add(new Label() { AutoSize = true, Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 15), Text = "START DATE" }, 3, 0);
                    tlpPeople.Controls.Add(new Label() { AutoSize = true, Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 15), Text = "END DATE" }, 4, 0);
                    tlpPeople.Controls.Add(new Label() { AutoSize = true, Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 15), Text = "DEVICE" }, 5, 0);
                }
                else
                {
                    //MessageBox.Show(ds.Tables[0].Rows[i][5].ToString());
                    //MessageBox.Show(ds.Tables[0].Rows[0][i].ToString());
                    //tlpPeople.RowStyles.Add(new RowStyle(SizeType.Absolute, tlpPeople.Size.Height / people.Count));
                    //tlpPeople.Controls.Add(new Label() { Text = "Street, City, State" }, 1, tlpPeople.RowCount - 1);

                    tlpPeople.Controls.Add(new Label() { AutoSize = true, Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 15), Text = ds.Tables[0].Rows[i][0].ToString() }, 0, i);
                    tlpPeople.Controls.Add(new Label() { AutoSize = true, Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 15), Text = ds.Tables[0].Rows[i][1].ToString() }, 1, i);
                    tlpPeople.Controls.Add(new Label() { AutoSize = true, Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 15), Text = ds.Tables[0].Rows[i][2].ToString() }, 2, i);
                    tlpPeople.Controls.Add(new Label() { AutoSize = true, Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 15), Text = ds.Tables[0].Rows[i][3].ToString() }, 3, i);
                    tlpPeople.Controls.Add(new Label() { AutoSize = true, Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 15), Text = ds.Tables[0].Rows[i][4].ToString() }, 4, i);
                    tlpPeople.Controls.Add(new Label() { AutoSize = true, Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 15), Text = ds.Tables[0].Rows[i][5].ToString() }, 5, i);
                }
            }
        }
    }*/

        private void GenerateHeader()
        {
            tlpHeader.Controls.Clear();
            tlpHeader.ColumnStyles.Clear();
            tlpHeader.RowStyles.Clear();

            tlpHeader.ColumnCount = 3;
            //tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            List<string> tmpData = new List<string>() { "NAME", "DEPARTMENT", "ASSIGNMENT" };
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            //tlpHeader.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            for (int y = 0; y < 3; y++)
            {
                Label lab = new Label();

                //tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                lab.Text = tmpData[y];
                int w = GetWfromY(y);

                lab.Size = new Size(w, 100);

                lab.Font = new System.Drawing.Font("Verdana", 27, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));

                lab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(40)))), ((int)(((byte)(114)))));
                lab.TextAlign = ContentAlignment.MiddleCenter;
                lab.AutoSize = true;
                lab.Dock = DockStyle.Fill;
                lab.Margin = new Padding(0, 0, 0, 0);

                tlpHeader.Controls.Add(lab, y, 0);

            //tlpHeader.Controls.Add(new Label() { AutoSize = true, Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 15), Text = "NAME" }, 0, 0);
            //tlpHeader.Controls.Add(new Label() { AutoSize = true, Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 15), Text = "DEPARTMENT" }, 1, 0);
            //tlpHeader.Controls.Add(new Label() { AutoSize = true, Dock = DockStyle.Fill, Font = new Font("Microsoft Sans Serif", 15), Text = "ASSIGNMENT" }, 2, 0);
            }
        }

        private void GenerateTable(int rowCount)
        {
            int columnCount = 3;
            tlpPeople.Controls.Clear();
            tlpPeople.ColumnStyles.Clear();
            tlpPeople.RowStyles.Clear();
            tlpPeople.ColumnCount = columnCount;
            tlpPeople.RowCount = rowCount;

            tlpPeople.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            //DateTime date1 = new DateTime(2001, 1, 1, 0, 0, 0);
            for (int x = 0; x < rowCount; x++)
            {
                //int warning = 0;
                List<string> tmpData = new List<string>();
                Tables tmpInfo = ds.ElementAt(x);
                tmpData.Add(tmpInfo.UserName);
                tmpData.Add(tmpInfo.UserDepartment);
                tmpData.Add(tmpInfo.TaskDescription);

                for (int y = 0; y < columnCount; y++) //podesavanje sirine tabele
                {
                    Label lab = new Label();
                    lab.Text = tmpData.ElementAt(y);
                    int w = GetWfromY(y);

                    lab.AutoSize = false;
                    lab.Size = new Size(w, 100);
                    lab.Font = new System.Drawing.Font("Verdana", 27, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
                    lab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));

                    //if (y == 2)
                        lab.TextAlign = ContentAlignment.MiddleLeft;
                    //else
                    //    lab.TextAlign = ContentAlignment.MiddleLeft;

                    lab.Dock = DockStyle.Fill;
                    lab.Margin = new Padding(0, 0, 0, 0);


                    if (x % 2 == 0)
                    {
                        lab.BackColor = Color.LightGray;
                    }

                    tlpPeople.Controls.Add(lab, y, x);

                    labHeight = lab.Size.Height;
                }

            }

            //tlpPeople.Size = new Size(panTable.Size.Width, ds.Count() * labHeight);

            tlpPeople.Size = new Size(Screen.PrimaryScreen.Bounds.Width, (ds.Count() + 1) * labHeight);

            if (tlpPeople.Size.Height > panTable.Size.Height)
            {
                animate = 1;
                locY = tlpPeople.Location.Y;
            }
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

        private void label7_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            UsersForm usersForm = new UsersForm();
            usersForm.Show();
            this.Hide();
            //this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (animate == 1)
            {
                if (scrollUpOrDown == 0)
                {
                    locY = locY - tableScrollSpeed;
                    tlpPeople.Location = new Point(tlpPeople.Location.X, locY);
                    tlpPeople.Invalidate();
                    if (locY <= panel1.Size.Height - tlpPeople.Size.Height)
                        scrollUpOrDown = 1;
                }
                else if (scrollUpOrDown == 1)
                {
                    locY = locY + tableScrollSpeed;
                    tlpPeople.Location = new Point(tlpPeople.Location.X, locY);
                    tlpPeople.Invalidate();
                    if (locY >= tlpHeader.Size.Height)
                        scrollUpOrDown = 0;
                }
            }
        }
    }
}
