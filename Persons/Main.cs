/*
Copyright (C) 2023 Darko Milosevic <daremc86@gmail.com>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 2.1 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.

*/

// Library imports

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Persons.API;

namespace Persons
{
    public partial class Main : Form
    {
        // API object
        APISQL SqlAPI;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.SqlAPI = new APISQL();
            this.DataView.DataSource = SqlAPI.getAllPersons();
            this.Show();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.SqlAPI.Dispose();
            this.SqlAPI = null;
            this.Dispose();
            Application.Exit();
        }

        // New button event click
        private void btnNew_Click(object sender, EventArgs e)
        {
            // Creating object to the person form
            Person person = new Person();
            // Setting person text property to "Add new person"
            person.Text = "Add new person";
            // Showing the form
            person.Show();
            // Hiding this form.
            this.Hide();
        }

// Click event for the exit button
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

// Event for changing the filter text. If text is blank, it show's all data. If not, it show's only data with specifyed OIB, name, or sirname
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            // Checking if text appears to be blank
            if (txtFilter.Text == "")
            {
                this.DataView.DataSource = this.SqlAPI.getAllPersons();
            }
            // If text nott appears to be blank
            else
            {
                this.DataView.DataSource = SqlAPI.FilterPersons(this.txtFilter.Text);
            }
        }

        // Event for clicking on edit button
        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Creating person form object
            Person person = new Person();
            // Setting properties
            person.ID = DataView.CurrentRow.Cells[0].Value.ToString();
            person.txtOIB.Text = DataView.CurrentRow.Cells[1].Value.ToString();
            person.txtName.Text = DataView.CurrentRow.Cells[2].Value.ToString();
            person.txtSirName.Text = DataView.CurrentRow.Cells[3].Value.ToString();
            person.txtPlace.Text = DataView.CurrentRow.Cells[4].Value.ToString();
            person.txtAddress.Text = DataView.CurrentRow.Cells[5].Value.ToString();
            person.txtPhone.Text = DataView.CurrentRow.Cells[6].Value.ToString();
            person.txtEmail.Text = DataView.CurrentRow.Cells[7].Value.ToString();
            person.Text = "Edit person";
            // Showing the form.
            person.Show();
            this.Hide();
        }

        // Event for the delete button click
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Creating dialog with message box for deleting confirmation
            DialogResult dr = MessageBox.Show("Are you sure you want to delete this person?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            // Checking if user has clicked 'Yes'
            if (dr == DialogResult.Yes)
            {
                // Deleting the data using the current row ID
                if (!SqlAPI.DeletePerson(DataView.CurrentRow.Cells[0].Value.ToString()))
                {
                    // If an error, send message to the user.
                    MessageBox.Show("Cannot connect to the SQL database.\nPlease check the SQL connection, and then try again.", "SQL connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Refresh the data
                    this.DataView.DataSource = SqlAPI.getAllPersons();
                }
            }
        }
    }
}
