using System;

namespace Evolent.Models.Shared
{
    public interface IEvolentUser
    {
        int ApplicationId { get; set; }
        string ApplicationName { get; set; }        
        string AppToken { get; set; }

        string UserToken { get; set; }
        int? UserId { get; set; }
        string UserName { get; set; }

        string IPAddress { get; set; }
        string RequestUrl { get; set; }
        string ReferrerUrl { get; set; }
        string Headers { get; set; }
        string ClientIPAddress { get; set; }
    }
}
