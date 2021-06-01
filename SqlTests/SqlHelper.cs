using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Security;
using System.Data.SqlClient;
using System.Data;

namespace SqlTests
{
    public class SqlHelper
    {
        private readonly SqlConnectionStringBuilder _sqlConnectionString;
        private SqlConnection _sqlConnection;

        public SqlHelper(string dbName)
        {
            _sqlConnectionString = new SqlConnectionStringBuilder
            {
                DataSource = "localhost",
                InitialCatalog = dbName,
                IntegratedSecurity = true
            };
        }

        public void OpenConnection()
        {
            _sqlConnection = new SqlConnection(_sqlConnectionString.ConnectionString);
            _sqlConnection.Open();
        }

        public void CloseConnection() => _sqlConnection.Close();

        public void ExecuteNonQuery(string request)
        {
            var command = new SqlCommand(request, _sqlConnection);
            command.ExecuteNonQuery();
        }

        public void Insert(string table, Dictionary<string, string> parameters)
        {
            var columns = string.Empty;
            var values = string.Empty;

            foreach (var (key, value) in parameters)
            {
                columns += $"{key},";
                values += $"{value},";
            }

            var command = new SqlCommand($"insert into {table} ({columns.TrimEnd(',')}) values ({values.TrimEnd(',')})",
                _sqlConnection);
            command.ExecuteNonQuery();
        }

        public void Update(string table, Dictionary<string, string> parameters, Dictionary<string, string> newParameters)
        {
            List<string> columns = new List<string>();
            List<string> values = new List<string>();

            List<string> newColumns = new List<string>();
            List<string> newValues = new List<string>();

            foreach (var (key, value) in parameters)
            {
                columns.Add(key);
                values.Add(value);
            }

            foreach (var (key, value) in newParameters)
            {
                newColumns.Add(key);
                newValues.Add(value);
            }

            var requesr = $"UPDATE {table} SET ";
            for (int i = 0; i < newParameters.Count; i++)
            {
                requesr += $"{newColumns[i]} = {newValues[i]} , ";
            }

            requesr = requesr.Remove(requesr.Length-3);
            requesr += " WHERE ";

            for (int j = 0; j < parameters.Count; j++)
            {
                requesr += $"{columns[j]} = {values[j]} AND ";
            }

            requesr = requesr.Remove(requesr.Length - 5);
            

            var command = new SqlCommand(requesr, _sqlConnection);
            command.ExecuteNonQuery();
        }

        public void Delete(string table, Dictionary<string, string> parameters)
        {
            List<string> columns = new List<string>();
            List<string> values = new List<string>();

            foreach (var (key, value) in parameters)
            {
                columns.Add(key);
                values.Add(value);
            }

            var request = $"DELETE FROM {table} WHERE ";

            for (int j = 0; j < parameters.Count; j++)
            {
                request += $"{columns[j]} = {values[j]} AND ";
            }

            request = request.Remove(request.Length - 5);

            var command = new SqlCommand(request, _sqlConnection);
            command.ExecuteNonQuery();
        }

        public bool IsRowExistedInTable(string table, Dictionary<string, string> parameters)
        {
            var whereParameters = string.Empty;

            foreach (var (key, value) in parameters)
                whereParameters += $" and {key}={value}";

            var sqlDataAdapter = new SqlDataAdapter(
                $"select * from {table} where {whereParameters.Substring(5)}",
                _sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            return dataTable.Rows.Count > 0;
        }
    }
}
