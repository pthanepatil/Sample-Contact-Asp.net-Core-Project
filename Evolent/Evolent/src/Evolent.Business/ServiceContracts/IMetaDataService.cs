
namespace Evolent.Business.ServiceContracts
{
    public interface IMetaDataService : IBaseService
    {
        string BaseApiUrl { get; }
        string AuthenticationServiceUrl { get; }
        string ContactServiceUrl { get; }        
    }
}
