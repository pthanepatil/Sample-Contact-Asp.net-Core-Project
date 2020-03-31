using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Evolent.Database.Contracts
{
    public interface IDatabase
    {
        string connString { get; }
        SqlConnection GetConnection();
        SqlCommand GetStoredProcCommand(string spName);
        void AddInParameter(SqlCommand dbCommand, string parameterName, object value);
        void AddInParameter(SqlCommand dbCommand, string parameterName, DbType type, object value);
        SqlDataReader ExecuteReader(SqlCommand dbCommand);
        Task<SqlDataReader> ExecuteReaderAsync(SqlCommand dbCommand);
        object ExecuteScalar(SqlCommand dbCommand);
        Task<object> ExecuteScalarAsync(SqlCommand dbCommand);
        int ExecuteNonQuery(SqlCommand dbCommand);
        Task<int> ExecuteNonQueryAsync(SqlCommand dbCommand);
    }
}
