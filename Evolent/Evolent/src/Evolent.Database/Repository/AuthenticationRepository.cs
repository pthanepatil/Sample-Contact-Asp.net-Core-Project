using Evolent.Database.Contracts;
using Evolent.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Evolent.Database.Repository
{
    public class AuthenticationRepository : ApplicationRepository, IAuthenticationRepository
    {
        #region Declarations
        private readonly IDefaultDatabase _database;

        public AuthenticationRepository(IDefaultDatabase database) :
            base(database)
        {
            this._database = database;
        }

        #endregion

        public async Task<UserResponse> LoadUser(UserRequest user)
        {
            var dbCommand = _database.GetStoredProcCommand("sp_validate_user");
            _database.AddInParameter(dbCommand, "@userName", DbType.String, user.UserName);
            _database.AddInParameter(dbCommand, "@password", DbType.String, user.Password);

            UserResponse userResponse = null;
            using (var reader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (reader.Read())
                {
                    userResponse = new UserResponse();
                    userResponse.UserId = Convert.ToInt32(reader["Id"]);
                    userResponse.UserName = Convert.ToString(reader["UserName"]);
                    userResponse.UserToken = Convert.ToString(reader["UserToken"]);
                }
            }

            return userResponse;
        }
    }
}
