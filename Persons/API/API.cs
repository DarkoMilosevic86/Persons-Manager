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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace Persons.API
{
    // Public APISQL class which contains the API for managing persons in the SQL database

        /// <summary>
        ///  APISQL object for persons manipulation
        /// </summary>
    public class APISQL
    {
        // Connection string property
        private string ConnectionString;
        // SQL connection property
        private SqlConnection sqlCon;

        // Class constructor
        public APISQL()
        {
            ConnectionString = @"Data Source="+SQLConfig.ServerName+";Initial Catalog="+SQLConfig.DBName+";User ID="+SQLConfig.UserName+";Password="+SQLConfig.Password+"";
            sqlCon = new SqlConnection(ConnectionString);
        }

        // Object disposing

            public void Dispose ()
        {
            this.sqlCon.Dispose();
            this.sqlCon = null;
        }

        // Get all persons method

/// <summary>
/// Gets all persons from the database
/// </summary>
/// <returns>Data table collection of a table rows</returns>
            public DataTable getAllPersons()
        {
            // Query string
            string query = "select * from " + SQLConfig.PersonsTable + "";
            // Open the SQL connection
            sqlCon.Open();
            // Creating the SqlCommand object
            SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
            // Setting the SQL command type to text
            sqlcmd.CommandType = CommandType.Text;
            // Creating the SQL data adapter
            SqlDataAdapter sqladapter = new SqlDataAdapter(sqlcmd);
            // Creating the data table and feeling the data from the table
            DataTable dtab = new DataTable();
            // Feeling the data from the SQL adapter
            sqladapter.Fill(dtab);
            // Close the connection and dispose all objects
            sqlCon.Close();
            sqladapter.Dispose();
            sqladapter = null;
            sqlcmd.Dispose();
            sqlcmd = null;
            // Return the data
            return dtab;
        }

        // Checking if an OIB exists
/// <summary>
/// Checking if specifyed OIB exists
/// </summary>
/// <param name="OIB">Person's OIB</param>
/// <returns>True if the OIB exists. Otherwise, returns false.</returns>
        public bool CheckOIB(string OIB)
        {
            // Query string
            string query = "select * from " + SQLConfig.PersonsTable + " where oib='" + OIB + "'";
            // Opening the connection
            sqlCon.Open();
            // Creating the sql command
            SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
            // Setting the SQL command type property to text
            sqlcmd.CommandType = CommandType.Text;
            // creating the data table
            DataTable tbl = new DataTable();
            // Creating the SQL data adapter
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcmd);
            // Feeling the data
            adapter.Fill(tbl);
            // Closing the connection and disposing the objects
            sqlCon.Close();
            adapter.Dispose();
            sqlcmd.Dispose();
            adapter = null;
            sqlcmd = null;
            // Checking if tbl.Rows has greater than 0. If yes, the OIB exists
            if (tbl.Rows.Count > 0)
            {
                tbl.Dispose();
                tbl = null;
                return true;
            }
            // If count has less or equal to 0, the OIB does not exists
            tbl.Dispose();
            tbl = null;
            return false;
        }

        // Filterring persons by OIB, name, or sirname

            /// <summary>
            /// Searches persons by specifying OIB, name, or sirname
            /// </summary>
            /// <param name="FilterInput">Filter input as string. It should be OIB, name, or sirname</param>
            /// <returns>Data table with the search results</returns>
        public DataTable FilterPersons(string FilterInput)
        {
            // Query string
            string query = "select * from " + SQLConfig.PersonsTable + " where oib='" + FilterInput + "' or name='" + FilterInput + "' or sirname='" + FilterInput + "'";
            // Openning the connection
            sqlCon.Open();
            // Creating the SQL command
            SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
            // Setting the CommandType to text
            sqlcmd.CommandType = CommandType.Text;
            // Creating the data table
            DataTable tbl = new DataTable();
            // Creating the SQL data adapter
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcmd);
            // Filling the data
        adapter.Fill(tbl);
            // Disposing objects and closing the connection
sqlCon.Close();
            adapter.Dispose();
            sqlcmd.Dispose();
            adapter = null;
            sqlcmd = null;
            // Returns data
            return tbl;
        }
        // New person method

            /// <summary>
            /// Adds a new person to the database
            /// </summary>
            /// <param name="OIB">Person's OIB</param>
            /// <param name="FirstName">Person's name</param>
            /// <param name="LastName">Person's sir name</param>
            /// <param name="Place">Person's place/city</param>
            /// <param name="Address">Person's address</param>
            /// <param name="Phone">Person's phone number</param>
            /// <param name="EMail">Person's email address</param>
            /// <returns>True if data were added to the database. Otherwise, returns false</returns>
        public bool NewPerson (string OIB, string FirstName, string LastName, string Place, string Address, string Phone, string EMail)
        {
            // Checking if params are not blank
            if (OIB != "" || FirstName != "" || LastName != "" || Place != "" || Address != "" || Phone != "" || EMail != "")
            {
                // Inserting data into table
                string query = "insert into "+SQLConfig.PersonsTable+" (oib,name,sirname,place,address,phone,email) values ('"+OIB+"','" + FirstName + "','" + LastName + "','" + Place + "','" + Address + "','" + Phone + "','" + EMail + "')";
                // Opening connection
                sqlCon.Open();
// Creating object for the SqlCommand
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                // Setting CommandText property of the sqlcmd object
                sqlcmd.CommandText = query;
                // Executing NonQuery
                int nQuery = sqlcmd.ExecuteNonQuery();
                // Closing the connection
                sqlCon.Close();
                    // Releasing the sqlcmd object
                    sqlcmd.Dispose();
                sqlcmd = null;
                // If query succeed
                if (nQuery == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
// If blank
else
            {
                return false;
            }
        }

        // Updating person by id

/// <summary>
/// Updates the person in the database using the person's ID
/// </summary>
/// <param name="ID">Person's ID</param>
/// <param name="OIB">Persons's OIB</param>
/// <param name="FirstName">Person's name</param>
/// <param name="LastName">Person's sir name</param>
/// <param name="Place">Person's place/city</param>
/// <param name="Address">Person's address</param>
/// <param name="Phone">Person's phone number</param>
/// <param name="EMail">Person's email address</param>
/// <returns>True if updating succeed. Otherwise, returns false</returns>
        public bool UpdatePerson (string ID, string OIB, string FirstName, string LastName, string Place, string Address, string Phone, string EMail)
        {
            // Checking if params are not blank
            if (ID != "" || OIB != "" || FirstName != "" || LastName != "" || Place != "" || Address != "" || Phone != "" || EMail != "")
            {
                // Creating the SQL query
                string query = "update " + SQLConfig.PersonsTable + " set oib='" + OIB + "', name='" + FirstName + "', sirname='" + LastName + "', place='" + Place + "', address='" + Address + "', phone='" + Phone + "', email='" + EMail + "' where id=" + ID + "";
                // Opening the SQL connection
                sqlCon.Open();
                // Creating the SQL command object
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                // Setting the SqlCommandText property
                sqlcmd.CommandText = query;
                // Executing the query
                int nQuery = sqlcmd.ExecuteNonQuery();
                // Closing the connection and releasing the sqlcmd object
                sqlCon.Close();
                sqlcmd.Dispose();
                sqlcmd = null;
                // Checking if query succeeded
                if (nQuery == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // If blank
            else
            {
                return false;
            }
        }

        // Deleting the person

            /// <summary>
            ///  Deletes the person from the database using the person's ID
            /// </summary>
            /// <param name="ID">Person's ID</param>
            /// <returns>true if person has been deleted. Otherwise, returns false</returns>
        public bool DeletePerson (string ID)
        {
// Checking if person's ID is not blank
if (ID != "")
            {
                // Query string
                string query = "delete from " + SQLConfig.PersonsTable + " where id=" + ID + "";
                // Opening the SQL connection
                sqlCon.Open();
                // Creating the instance to the SqlCommand object
                SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
                // Setting CommandText property to the query text
                sqlcmd.CommandText = query;
                // Executing the query
                int nQuery = sqlcmd.ExecuteNonQuery();
                // Clocing the connection and disposing the SqlCommand object
                sqlCon.Close();
                sqlcmd.Dispose();
                sqlcmd = null;
                // Checking if query
                if (nQuery == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
// Otherwise
else
            {
                return false;
            }
        }
    }
}
