using Evolent.Business.ServiceContracts;
using System;

namespace Evolent.Business.Service
{
    public class MetaDataService : IMetaDataService
    {
        public string AuthenticationServiceUrl
        {
            get
            {
                return this.BaseApiUrl + "Authentication/";
            }
        }

        public string BaseApiUrl
        {
            get
            {
                return @"http://localhost:55815/api/";
            }
        }

        public string ContactServiceUrl
        {
            get
            {
                return this.BaseApiUrl + "UserContact/";
            }
        }
    }
}
