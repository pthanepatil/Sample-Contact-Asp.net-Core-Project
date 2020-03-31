using Evolent.Models;
using Evolent.Models.Shared;
using System.Threading.Tasks;

namespace EvolentContact.API.Services
{
    public interface IAuthenticationService
    {
        Task<ErrorResponse> IsValidToken(IEvolentUser evolentUser, bool validateBothToken = true);
        Task<UserResponse> LoadUser(UserRequest userRequest);
    }
}
