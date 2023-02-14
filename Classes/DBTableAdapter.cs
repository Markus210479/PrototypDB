using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;
using System.Data;

namespace PrototypeDB.Classes
{
    public static class DBTableAdapter
    {       
        public static int DBGetRowsCount(SqlConnection connection, string tableName)
        {
            int row = 0;

            try
            {
                using (SqlCommand command = new SqlCommand($"SELECT * FROM {tableName}", connection))
                {
                    connection.Open();
                    SqlDataAdapter TableAdapter = new SqlDataAdapter(command);
                    DataSet Dataset = new DataSet();
                    TableAdapter.Fill(Dataset, tableName);

                    row = Dataset.Tables[tableName].Rows.Count;

                    TableAdapter.Dispose();
                    Dataset.Dispose();
                    connection.Close();
                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show(CreateErrorMessage(ex));
                connection.Close();
                return 0;
            }

            return row;
        }


        public static DataSet DBFilterSearchValues(SqlConnection connection, string tableName, List<string> columns, string searchValue)
        {
            DataSet ds = new DataSet();
            string searchString = string.Empty;

            foreach (string columnName in columns)
            {
                searchString += (columnName != columns[columns.Count - 1]) ? $"({columnName} LIKE @{columnName}) OR " : $"({columnName} LIKE @{columnName})";
            }

            try
            {
                using (SqlCommand command = new SqlCommand($"SELECT * FROM {tableName} WHERE {searchString}", connection))
                {
                    foreach (string columnName in columns)
                    {
                        command.Parameters.AddWithValue($"@{columnName}", searchValue);
                    }
                    
                    SqlDataAdapter TableAdapter = new SqlDataAdapter(command);
                    connection.Open();
                    command.ExecuteNonQuery();
                    TableAdapter.Fill(ds);
                    TableAdapter.Dispose();
                    ds.Dispose();
                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(CreateErrorMessage(ex));
                connection.Close();
                return null;
            }
            return ds;
        }

        public static DataSet GetDataSet(string commandString, string tableName, SqlConnection connection)
        {

            DataSet Dataset = new DataSet();
            using (SqlCommand command = new SqlCommand(commandString, connection))
            {
                connection.Open();
                SqlDataAdapter TableAdapter = new SqlDataAdapter(command);
                TableAdapter.Fill(Dataset, tableName);
                               
                TableAdapter.Dispose();
                Dataset.Dispose();
                connection.Close();
            }

            return Dataset;
        }

        public static DataRow DBGetRow(SqlConnection connection, string tableName, int index)
        {
            DataRow dr = null;
            try
            {
                using (SqlCommand command = new SqlCommand($"SELECT * FROM {tableName}", connection))
                {
                    connection.Open();

                    SqlDataAdapter TableAdapter = new SqlDataAdapter(command);
                    DataSet Dataset = new DataSet();
                    TableAdapter.Fill(Dataset, tableName);

                    index = index > Dataset.Tables[tableName].Rows.Count - 1 ? -1 : index; 
                    dr = Dataset.Tables[tableName].Rows[Dataset.Tables[tableName].Rows.Count - 1];

                    TableAdapter.Dispose();
                    Dataset.Dispose();
                    connection.Close();
                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show(CreateErrorMessage(ex));
                connection.Close();
                return null;
            }

            return dr;
        }


        /// <summary>
        /// Einfügen eines neuen Datensatzes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="returnID">if set to <c>true</c> [return identifier].</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static string DBInsert<T>(SqlConnection connection, bool returnID, string tableName, List<string> columns, ref List<T> values)
        {
            string returnValue = "";                                 
            string columnsString = string.Empty;
            string varString = string.Empty;

            //Erstellung des CommandStrings für die Insert-Anweisung
            foreach (string columnName in columns)
            {
                columnsString += (columnName != columns[columns.Count - 1]) ? $"{columnName}," : $"{columnName}";
                varString += (columnName != columns[columns.Count - 1]) ? $"@{columnName}," : $"@{columnName}";               
            }

            SqlCommand command = new SqlCommand($"INSERT INTO {tableName} ({columnsString}) VALUES ({varString}); SELECT SCOPE_IDENTITY()", connection);
            try
            {
                using (command)
                {
                    connection.Open();

                    foreach (string columnName in columns)
                    {
                        command.Parameters.AddWithValue($"@{columnName}", values[columns.IndexOf(columnName)]);
                    }                   

                    returnValue = returnID == true ? command.ExecuteScalar().ToString() : command.ExecuteNonQuery().ToString();
                    connection.Close();
                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show(CreateErrorMessage(ex));
                connection.Close();
                return string.Empty;
            }

            return returnValue;
        }


        /// <summary>
        /// Löschen eines oder mehrerer vorhandener Einträge mit 'deleteValue'.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="deleteValue">The delete value.</param>
        public static bool Delete<T>(SqlConnection connection, string tableName, string columnName, ref T deleteValue)
        {           
            SqlCommand command = new SqlCommand($"DELETE FROM {tableName} WHERE {columnName} = {deleteValue}", connection);

            try
            {
                using (command)
                {
                    connection.Open();
                    command.ExecuteReader();
                    connection.Close();
                    return true;
                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show(CreateErrorMessage(ex));
                connection.Close();
                return false;
            }
        }

        private static string CreateErrorMessage(SqlException ex)
        {
            StringBuilder errorMessages = new StringBuilder();
            for (int i = 0; i < ex.Errors.Count; i++)
            {
                errorMessages.Append("Index #" + i + "\n" +
                    "Message: " + ex.Errors[i].Message + "\n" +
                    "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                    "Source: " + ex.Errors[i].Source + "\n" +
                    "Procedure: " + ex.Errors[i].Procedure + "\n");
            }

            return errorMessages.ToString();
        }

        /// <summary>
        /// Update eines vorhandenden Eintrags
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="Id">The identifier.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="values">The values.</param>
        public static bool Update(SqlConnection connection, string tableName, int Id, List<string> columns, List<string> values)
        {            

            string columnsString = string.Empty;
            string varString = string.Empty;

            //Beschaffen der jeweiligen Spaltenbezeichung für die ID-Spalte
            SqlDataAdapter TableAdapter = new SqlDataAdapter(new SqlCommand($"SELECT * FROM {tableName}", connection));
            DataSet Dataset = new DataSet();
            TableAdapter.Fill(Dataset, tableName);
            string idName = Dataset.Tables[tableName].Columns[0].ColumnName;

            //Erstellung des CommandStrings für die Update-Anweisung
            foreach (string columnName in columns)
            {
                columnsString += (columnName != columns[columns.Count - 1]) ? $"{columnName}=@{columnName}," : $"{columnName}=@{columnName}";
            }

            SqlCommand command = new SqlCommand($"UPDATE {tableName} SET {columnsString} WHERE {Id} = {idName}", connection);
            try
            {
                using (command)
                {
                    connection.Open();

                    foreach (string columnName in columns)
                    {
                        command.Parameters.AddWithValue($"@{columnName}", values[columns.IndexOf(columnName)]);
                    }                  

                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(CreateErrorMessage(ex));
                connection.Close();
                return false;
            }
        }
    }
}
