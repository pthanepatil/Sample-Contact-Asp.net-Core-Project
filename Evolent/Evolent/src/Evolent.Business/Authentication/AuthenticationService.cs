using Evolent.Business.Contracts;
using Evolent.Business.Helpers;
using Evolent.Business.ServiceContracts;
using Evolent.Models;
using Evolent.Models.Shared;
using System.Threading.Tasks;

namespace Evolent.Business.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Declarations

        protected ApiHelper apiHelper;
        private IMetaDataService _metadataService;

        #endregion

        #region Constructor

        public AuthenticationService(IMetaDataService metadataService)
        {
            this._metadataService = metadataService;
            apiHelper = new ApiHelper(metadataService);
        }

        #endregion

        public async Task<UserResponse> Login(UserRequest request, IEvolentUser user)
        {
            return await apiHelper.PostAsync<UserResponse>(_metadataService.AuthenticationServiceUrl + "Login", request, user);
        }
    }
}
