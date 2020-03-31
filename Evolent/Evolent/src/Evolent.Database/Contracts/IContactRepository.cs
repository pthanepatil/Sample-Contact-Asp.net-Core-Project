using Evolent.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evolent.Database.Contracts
{
    public interface IContactRepository
    {
        Task<List<ContactModel>> LoadUserContacts(int userId, int? contactId);
        Task<int> SaveUserContacts(int userId, ContactModel contact);
        Task<int> UpdateContactStatus(int userId, int contactId, bool status);
    }
}
