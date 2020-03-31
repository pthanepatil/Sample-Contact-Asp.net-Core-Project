using Evolent.Models;
using Evolent.Models.Shared;
using System.Threading.Tasks;

namespace Evolent.Business.Contracts
{
    public interface IAuthenticationService
    {
        Task<UserResponse> Login(UserRequest req, IEvolentUser user);
    }
}
