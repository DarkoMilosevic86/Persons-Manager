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
    public partial class Person : Form
    {
        // the ID parameter because the same form has been used for adding and updating persons
        public string ID = "";

        // Object for the APISQL class
        private APISQL sqlAPI;

// Form constructor, same as a class
        public Person()
        {
            InitializeComponent();
        }

// Click event for the abort button
        private void btnAbort_Click(object sender, EventArgs e)
        {
            // Creating the object for the main form
            Main mainform = new Main();
            // Showing the main form
            mainform.Show();
            // Closing this form.
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // We need to check if text fields are blank
            if (txtName.Text == "" || txtSirName.Text == "" || txtOIB.Text == "" || txtPlace.Text == "" || txtAddress.Text == "" || txtPhone.Text == "" || txtEmail.Text == "")
            {
                // Sending a message to the user
                MessageBox.Show("Please correct all fields, and try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // If not blank
            else
            {
                // Checking if user adding a new person, or updating the existing
                if (this.ID != "")
                {
                    // Updating person
                    if (!sqlAPI.UpdatePerson(this.ID, txtOIB.Text, txtName.Text, txtSirName.Text, txtPlace.Text, txtAddress.Text, txtPhone.Text, txtEmail.Text))
                    {
                        // Showing the error message to the user
                        MessageBox.Show("Cannot connect to the SQL database.\nPlease check your database connection, and then try again.", "SQL connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                // If not the ID specifyed, adding a new person
else
                {
                    // Checking if person with the specifyed OIB already exists
if (sqlAPI.CheckOIB(txtOIB.Text))
                    {
                        // Sending message to the user
                        MessageBox.Show("The person with the OIB " + txtOIB.Text + " already exists.\nPlease correct the person's OIB, and then try again.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
// If the OIB does not exists
else
                    {
                        // adding a person
                        if (!sqlAPI.NewPerson(txtOIB.Text, txtName.Text, txtSirName.Text, txtPlace.Text, txtAddress.Text, txtPhone.Text, txtEmail.Text))
                        {
                            // Sending the SQL error message to the user
                            MessageBox.Show("Cannot connect to the SQL database.\nPlease check your database connection, and then try again.", "SQL connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            // Exitting the form
            Main mainform = new Main();
            mainform.Show();
            this.Close();
        }

        private void lblOIB_Click(object sender, EventArgs e)
        {

        }

// Event which occurs when window loads
        private void Person_Load(object sender, EventArgs e)
        {
            this.sqlAPI = new APISQL();
        }

        // Event when form is closed
        private void Person_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.sqlAPI.Dispose();
            this.sqlAPI = null;
            this.Dispose();
        }
    }
}
