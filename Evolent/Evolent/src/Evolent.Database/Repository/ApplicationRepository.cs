using Evolent.Database.Contracts;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Evolent.Database.Repository
{
    public class ApplicationRepository : IApplicationRepository
    {
        #region Declarations
        private readonly IDefaultDatabase _database;

        public ApplicationRepository(IDefaultDatabase database)
        {
            this._database = database;
        }

        #endregion

        public async Task<bool> ValidateToken(string appToken, string userToken, bool validateBothToken = true)
        {
            var dbCommand = _database.GetStoredProcCommand("sp_validate_token");
            _database.AddInParameter(dbCommand, "@appToken", DbType.String, appToken);
            _database.AddInParameter(dbCommand, "@userToken", DbType.String, userToken);
            _database.AddInParameter(dbCommand, "@validateBothToken", DbType.String, validateBothToken);
            return Convert.ToBoolean(await _database.ExecuteScalarAsync(dbCommand));
        }
    }
}
