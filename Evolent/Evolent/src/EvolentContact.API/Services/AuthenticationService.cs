using Evolent.Database.Contracts;
using Evolent.Models;
using Evolent.Models.Shared;
using System.Threading.Tasks;

namespace EvolentContact.API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        IAuthenticationRepository _authenticationRepository;
        public AuthenticationService(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }

        public async Task<ErrorResponse> IsValidToken(IEvolentUser evolentUser, bool validateBothToken = true)
        {
            ErrorResponse error = new ErrorResponse();
            //If ValidateBothToken is false and still user token is received then it will be validated
            if (!validateBothToken && !string.IsNullOrEmpty(evolentUser.UserToken))
            {
                validateBothToken = true;
            }
            if (string.IsNullOrEmpty(evolentUser.AppToken) || (validateBothToken && string.IsNullOrEmpty(evolentUser.UserToken)))
            {
                if (!validateBothToken)
                    error.SetError(ErrorCodes.InvalidAppToken);
                else
                    error.SetError(ErrorCodes.InvalidAppUserToken);

                return error;
            }
            var isValid = await _authenticationRepository.ValidateToken(evolentUser.AppToken, evolentUser.UserToken, validateBothToken);

            if (isValid)
                error.NoError();
            else
                error.SetError(ErrorCodes.RequestTokenAuthenticationFailure);

            return error;
        }

        public async Task<UserResponse> LoadUser(UserRequest userRequest)
        {
            UserResponse userResponse = await _authenticationRepository.LoadUser(userRequest);
            if (userResponse == null || string.IsNullOrEmpty(userResponse.UserToken))
            {
                userResponse.SetError(ErrorCodes.InvalidLoginCredentials);
            }

            return userResponse;
        }
    }
}
