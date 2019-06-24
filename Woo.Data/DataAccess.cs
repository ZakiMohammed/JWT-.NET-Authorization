using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Woo.Data
{
    public class DataAccess
    {
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataAdapter adapter;

        public DataAccess()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["WooConnection"].ConnectionString);
            command = new SqlCommand() { Connection = connection };
            adapter = new SqlDataAdapter(command);
        }

        public class Parameters
        {
            public string Name { get; set; }
            public object Value { get; set; }
        }

        public DataTable ExecuteTable(string query)
        {
            var table = new DataTable();

            connection.Open();
            command.CommandText = query;
            table.Load(command.ExecuteReader());
            connection.Close();

            return table;
        }

        public DataTable ExecuteTable(string query, List<Parameters> param)
        {
            var table = new DataTable();

            connection.Open();
            command.CommandText = query;
            command.Parameters.Clear();
            foreach (var item in param)
            {
                command.Parameters.AddWithValue(item.Name, item.Value);
            }
            table.Load(command.ExecuteReader());
            connection.Close();

            return table;
        }

        public List<T> ExecuteTable<T>(string query)
        {
            var table = new DataTable();

            connection.Open();
            command.CommandText = query;
            table.Load(command.ExecuteReader());
            connection.Close();

            return ToList<T>(table);
        }

        public List<T> ExecuteTable<T>(string query, List<Parameters> param)
        {
            var table = new DataTable();

            connection.Open();
            command.CommandText = query;
            command.Parameters.Clear();
            foreach (var item in param)
            {
                command.Parameters.AddWithValue(item.Name, item.Value);
            }
            table.Load(command.ExecuteReader());
            connection.Close();

            return ToList<T>(table);
        }

        public DataSet ExecuteDataSet(string query)
        {
            var resultSet = new DataSet();

            command.CommandText = query;
            adapter.Fill(resultSet);

            return resultSet;
        }

        public DataSet ExecuteDataSet(string query, List<Parameters> param)
        {
            var resultSet = new DataSet();

            command.CommandText = query;
            command.Parameters.Clear();
            foreach (var item in param)
            {
                command.Parameters.AddWithValue(item.Name, item.Value);
            }
            adapter.Fill(resultSet);

            return resultSet;
        }

        public object ExecuteScalar(string query)
        {
            object result;

            connection.Open();
            command.CommandText = query;
            result = command.ExecuteScalar();
            connection.Close();

            return result;
        }

        public object ExecuteScalar(string query, List<Parameters> param)
        {
            object result;

            connection.Open();
            command.CommandText = query;
            command.Parameters.Clear();
            foreach (var item in param)
            {
                command.Parameters.AddWithValue(item.Name, item.Value);
            }
            result = command.ExecuteScalar();
            connection.Close();

            return result;
        }

        public int ExecuteNonQuery(string query, List<Parameters> param)
        {
            int result;

            connection.Open();
            command.CommandText = query;
            command.Parameters.Clear();
            foreach (var item in param)
            {
                command.Parameters.AddWithValue(item.Name, item.Value);
            }
            result = command.ExecuteNonQuery();
            connection.Close();

            return result;
        }

        public DataSet ExecuteStoredProcedure(string name, List<Parameters> param)
        {
            var resultSet = new DataSet();

            command.CommandText = name;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Clear();
            foreach (var item in param)
            {
                command.Parameters.AddWithValue(item.Name, item.Value);
            }

            adapter.Fill(resultSet);

            return resultSet;
        }

        public List<T> ToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var instance = Activator.CreateInstance<T>();
                foreach (var property in properties)
                {
                    if (columnNames.Contains(property.Name.ToLower()))
                    {
                        var info = instance.GetType().GetProperty(property.Name);
                        property.SetValue(instance, row[property.Name] == DBNull.Value ? null : row[property.Name]);
                    }
                }
                return instance;
            }).ToList();
        }
    }
}
