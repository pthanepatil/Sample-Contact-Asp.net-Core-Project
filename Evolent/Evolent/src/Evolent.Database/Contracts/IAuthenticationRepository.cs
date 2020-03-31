using Evolent.Models;
using System.Threading.Tasks;

namespace Evolent.Database.Contracts
{
    public interface IAuthenticationRepository : IApplicationRepository
    {
        Task<UserResponse> LoadUser(UserRequest user);
    }
}
