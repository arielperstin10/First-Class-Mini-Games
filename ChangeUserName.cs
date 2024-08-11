﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace final_project
{
    public partial class ChangeUserName : Form
    {
        private User user;
        private Database database;
        public ChangeUserName(User userActive)
        {
            InitializeComponent();
            this.user = userActive;
            this.database = new Database();
        }

        private bool IsInputValid(string input)
        {
            // Validate Username
            if (input.Length < 6 || input.Length > 8)
            {
                MessageBox.Show("Username must be between 6 and 8 characters long.", "Invalid Username", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userInput = newUserName.Text;

            if (IsInputValid(userInput))
            {
                database.SetUsername(int.Parse(user.ID), userInput);
                Close();
            }
            else MessageBox.Show("Error Changing User Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
