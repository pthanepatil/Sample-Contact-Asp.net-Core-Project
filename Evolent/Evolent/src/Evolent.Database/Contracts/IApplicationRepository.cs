using System.Threading.Tasks;

namespace Evolent.Database.Contracts
{
    public interface IApplicationRepository
    {
        Task<bool> ValidateToken(string appToken, string userToken, bool validateBothToken = true);
    }
}
