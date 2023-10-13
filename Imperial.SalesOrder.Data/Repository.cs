using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using AutoMapper;
using System.Reflection;

namespace Imperial.SalesOrder.Data
{
    public class Repository : IDisposable
    {
        protected string m_ConnectionString;

        protected int m_Timeout = 60;

        public Repository()
        {
            m_ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            if (ConfigurationManager.AppSettings["DefaultTimeout"] != null)
            {
                m_Timeout = Convert.ToInt16(ConfigurationManager.AppSettings["DefaultTimeout"]);
            }
        }

        public Repository(string connectionString, int timeout)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (timeout < 0)
            {
                throw new ArgumentOutOfRangeException("timeout", "timeout cannot be less than 0");
            }

            m_ConnectionString = connectionString;
            m_Timeout = timeout;
        }

        public Repository(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            m_ConnectionString = connectionString;
            if (ConfigurationManager.AppSettings["DefaultTimeout"] != null)
            {
                m_Timeout = Convert.ToInt16(ConfigurationManager.AppSettings["DefaultTimeout"]);
            }
        }

        public static Repository Create()
        {
            return new Repository();
        }

        public DataSet ExecuteDataset(string procName, List<SqlParameter> parameters = null)
        {
            SqlConnection sqlConnection = new SqlConnection(m_ConnectionString);
            sqlConnection.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = new SqlCommand(procName);
            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    if (parameter.Value == null)
                    {
                        parameter.Value = DBNull.Value;
                    }

                    if (parameter.DbType == DbType.DateTime && Convert.ToDateTime(parameter.Value).CompareTo(new DateTime(1, 1, 1)) == 0)
                    {
                        parameter.Value = DBNull.Value;
                    }

                    sqlDataAdapter.SelectCommand.Parameters.Add(parameter);
                }
            }

            sqlDataAdapter.SelectCommand.Connection = sqlConnection;
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.CommandTimeout = m_Timeout;
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            return dataSet;
        }

        public DataSet ExecuteDatasetSQLStatement(string tSQL)
        {
            SqlConnection sqlConnection = new SqlConnection(m_ConnectionString);
            sqlConnection.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = new SqlCommand(tSQL);
            sqlDataAdapter.SelectCommand.Connection = sqlConnection;
            sqlDataAdapter.SelectCommand.CommandType = CommandType.Text;
            sqlDataAdapter.SelectCommand.CommandTimeout = m_Timeout;
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            return dataSet;
        }

        public long ExecuteScalar_Long(string procName, List<SqlParameter> parameters = null)
        {
            return long.Parse(ExecuteScalar(procName, parameters).ToString());
        }

        public int ExecuteScalar_Int(string procName, List<SqlParameter> parameters = null)
        {
            return int.Parse(ExecuteScalar(procName, parameters).ToString());
        }

        public decimal ExecuteScalar_Decimal(string procName, List<SqlParameter> parameters = null)
        {
            return decimal.Parse(ExecuteScalar(procName, parameters).ToString());
        }

        public bool ExecuteScalar_Bool(string procName, List<SqlParameter> parameters = null)
        {
            return bool.Parse(ExecuteScalar(procName, parameters).ToString());
        }

        public string ExecuteScalar_String(string procName, List<SqlParameter> parameters = null)
        {
            return ExecuteScalar(procName, parameters).ToString();
        }

        public void ExecuteNonQuery(string procName, List<SqlParameter> parameters = null)
        {
            SqlConnection sqlConnection = new SqlConnection(m_ConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(procName, sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = m_Timeout;
            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    if (parameter.Value == null)
                    {
                        parameter.Value = DBNull.Value;
                    }

                    sqlCommand.Parameters.Add(parameter);
                }
            }

            sqlCommand.ExecuteNonQuery();
        }

        public void ExecuteNonQuerySQLStatement(string tSQL)
        {
            SqlConnection sqlConnection = new SqlConnection(m_ConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(tSQL, sqlConnection);
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandTimeout = m_Timeout;
            sqlCommand.ExecuteNonQuery();
        }

        public List<Guid> GetRecords_Guid(string procName, List<SqlParameter> parameters = null)
        {
            DataSet dataSet = ExecuteDataset(procName, parameters);
            if (dataSet == null || dataSet.Tables == null)
            {
                throw new Exception("Query is incorrect");
            }

            List<Guid> list = new List<Guid>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(new Guid(row[0].ToString()));
            }

            return list;
        }

        public List<string> GetRecords_String(string procName, List<SqlParameter> parameters = null)
        {
            DataSet dataSet = ExecuteDataset(procName, parameters);
            if (dataSet == null || dataSet.Tables == null)
            {
                throw new Exception("Query is incorrect");
            }

            List<string> list = new List<string>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(row[0].ToString());
            }

            return list;
        }

        public List<T> GetRecords<T>(string procName, List<SqlParameter> parameters = null) where T : new()
        {
            DataSet dataSet = ExecuteDataset(procName, parameters);
            if (dataSet == null || dataSet.Tables == null)
            {
                throw new Exception("Query is incorrect");
            }

            List<T> list = new List<T>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(row.ToType<T>());
            }

            return list;
        }

        public List<T> GetRecords<T>(string tSQL) where T : new()
        {
            DataSet dataSet = ExecuteDatasetSQLStatement(tSQL);
            if (dataSet == null || dataSet.Tables == null)
            {
                throw new Exception("Query is incorrect");
            }

            List<T> list = new List<T>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(row.ToType<T>());
            }

            return list;
        }

        public T GetRecord<T>(string procName, List<SqlParameter> parameters = null) where T : new()
        {
            DataSet dataSet = ExecuteDataset(procName, parameters);
            if (dataSet == null || dataSet.Tables == null)
            {
                throw new Exception("Query is incorrect");
            }

            List<T> list = new List<T>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(row.ToType<T>());
            }

            if (list.Count > 0)
            {
                return list[0];
            }

            return default(T);
        }

        public T GetRecord<T>(string tSQL) where T : new()
        {
            DataSet dataSet = ExecuteDataset(tSQL);
            if (dataSet == null || dataSet.Tables == null)
            {
                throw new Exception("Query is incorrect");
            }

            List<T> list = new List<T>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(row.ToType<T>());
            }

            if (list.Count > 0)
            {
                return list[0];
            }

            return default(T);
        }

        private object ExecuteScalar(string procName, List<SqlParameter> parameters = null)
        {
            SqlConnection sqlConnection = new SqlConnection(m_ConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(procName, sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = m_Timeout;
            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    if (parameter.Value == null)
                    {
                        parameter.Value = DBNull.Value;
                    }

                    sqlCommand.Parameters.Add(parameter);
                }
            }

            return sqlCommand.ExecuteScalar().ToString();
        }

        public void Dispose()
        {
        }


    }
}
