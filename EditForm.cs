﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MMM
{
    public partial class EditForm : Form
    {
        Task edittask = new Task();

        public EditForm(string editable)
        {
            InitializeComponent();

            edittask.TaskDescription = editable;

            textBox1.Text = edittask.TaskDescription;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        public string Editable { get { return textBox1.Text; } }

        private void button3_Click(object sender, EventArgs e)
        {
            ConnectionClass.UpdateTaskDescription(textBox1.Text, edittask.TaskDescription);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
