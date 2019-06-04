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
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();

            dtLabel.Text = DateTime.Now.ToString("dd.MM.yyyy.  hh:mm:ss");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dtLabel.Text = DateTime.Now.ToString("dd.MM.yyyy.  hh:mm:ss");
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
