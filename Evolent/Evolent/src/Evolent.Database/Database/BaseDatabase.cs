using Evolent.Database.Contracts;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Evolent.Database.Database
{
    public class BaseDatabase : IDatabase
    {
        #region Declarations

        public virtual string connString { get; }

        #endregion

        #region Private/Protected Methods

        protected SqlConnection CreateConnection()
        {
            SqlConnection conn = new SqlConnection(this.connString);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            return conn;

        }

        private async Task<SqlConnection> CreateConnectionAsync()
        {
            SqlConnection conn = new SqlConnection(this.connString);
            if (conn.State != ConnectionState.Open)
            {
                await conn.OpenAsync();
            }
            return conn;
        }

        #endregion

        #region Public Methods

        public SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(this.connString);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            return conn;
        }

        public SqlCommand GetStoredProcCommand(string spName)
        {
            SqlCommand cmd = new SqlCommand(spName);
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }

        public void AddInParameter(SqlCommand dbCommand, string parameterName, object value)
        {
            parameterName = ValidateParameterName(parameterName);
            dbCommand.Parameters.AddWithValue(parameterName, value == null ? DBNull.Value : value);
        }

        public void AddInParameter(SqlCommand dbCommand, string parameterName, DbType type, object value)
        {
            parameterName = ValidateParameterName(parameterName);
            SqlParameter param = new SqlParameter(parameterName, type);
            param.Direction = ParameterDirection.Input;
            param.Value = (value == null ? DBNull.Value : value);
            dbCommand.Parameters.Add(param);
        }

        public SqlDataReader ExecuteReader(SqlCommand dbCommand)
        {
            var conn = CreateConnection();
            dbCommand.Connection = conn;
            return dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public async Task<SqlDataReader> ExecuteReaderAsync(SqlCommand dbCommand)
        {
            var conn = await CreateConnectionAsync();
            dbCommand.Connection = conn;
            return await dbCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }

        public object ExecuteScalar(SqlCommand dbCommand)
        {
            var conn = CreateConnection();
            dbCommand.Connection = conn;
            return dbCommand.ExecuteScalar();
        }

        public async Task<object> ExecuteScalarAsync(SqlCommand dbCommand)
        {
            var conn = await CreateConnectionAsync();
            dbCommand.Connection = conn;
            return await dbCommand.ExecuteScalarAsync();
        }

        public int ExecuteNonQuery(SqlCommand dbCommand)
        {
            var conn = CreateConnection();
            dbCommand.Connection = conn;
            return dbCommand.ExecuteNonQuery();
        }

        public async Task<int> ExecuteNonQueryAsync(SqlCommand dbCommand)
        {
            var conn = await CreateConnectionAsync();
            dbCommand.Connection = conn;
            return await dbCommand.ExecuteNonQueryAsync();
        }

        public string ValidateParameterName(string parameterName)
        {
            if (!string.IsNullOrEmpty(parameterName))
            {
                if (!parameterName.Substring(0, 1).Equals("@"))
                {
                    parameterName = "@" + parameterName;
                }
            }
            return parameterName;
        }
        #endregion
    }
}
