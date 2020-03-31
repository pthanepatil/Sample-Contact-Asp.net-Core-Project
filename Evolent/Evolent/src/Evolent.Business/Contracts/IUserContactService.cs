using Evolent.Models;
using Evolent.Models.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evolent.Business.Contracts
{
    public interface IUserContactService
    {
        Task<List<ContactModel>> Get(int? contactId, IEvolentUser user);
        Task<CommonResponse> Save(ContactModel contact, IEvolentUser user);
        Task<CommonResponse> UpdateContactStatus(int contactId, bool status, IEvolentUser user);
    }
}
