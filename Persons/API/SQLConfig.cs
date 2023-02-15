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

namespace Persons.API
{
    // Class for the SQL configuration, this should be static
    public static class SQLConfig
    {
        // Properties
        // If you are connecting to another server, you can change these properties here
        public static string ServerName = "DARIVOJE\\SQLEXPRESS";
        public static string UserName = "sa";
        public static string Password = "pass123";
        public static string DBName = "persondata";
        public static string PersonsTable = "persons";
    }
}
