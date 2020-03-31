using Evolent.Business.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Evolent.Models;
using Evolent.Models.Shared;
using Evolent.Business.Helpers;
using Evolent.Business.ServiceContracts;

namespace Evolent.Business.Contact
{
    public class UserContactService : IUserContactService
    {
        #region Declarations

        protected ApiHelper apiHelper;
        private IMetaDataService _metadataService;

        #endregion

        #region Constructor

        public UserContactService(IMetaDataService metadataService)
        {
            this._metadataService = metadataService;
            apiHelper = new ApiHelper(metadataService);
        }

        #endregion

        public async Task<List<ContactModel>> Get(int? contactId, IEvolentUser user)
        {
            string queryString = "?contactId=" + contactId;
            return await apiHelper.GetAsync<List<ContactModel>>(_metadataService.ContactServiceUrl + "Get", queryString, user);
        }

        public async Task<CommonResponse> Save(ContactModel contact, IEvolentUser user)
        {
            return await apiHelper.PostAsync<CommonResponse>(_metadataService.ContactServiceUrl + "Save", contact, user);
        }

        public async Task<CommonResponse> UpdateContactStatus(int contactId, bool status, IEvolentUser user)
        {
            ContactModel contact = new ContactModel();
            contact.ContactId = contactId;
            contact.Status = status;
            return await apiHelper.PutAsync<CommonResponse>(_metadataService.ContactServiceUrl + "UpdateContactStatus", contact, user);
        }
    }
}
