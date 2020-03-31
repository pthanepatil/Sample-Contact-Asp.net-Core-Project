using System;

namespace Evolent.Models.Shared
{
    public class EvolentUser : IEvolentUser
    {
        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string AppToken { get; set; }

        public string UserToken { get; set; }
        public string UserName { get; set; }
        public int? UserId { get; set; }
                
        public string IPAddress { get; set; }
        public string RequestUrl { get; set; }
        public string ReferrerUrl { get; set; }
        public string Headers { get; set; }
        public string ClientIPAddress { get; set; }
    }
}
