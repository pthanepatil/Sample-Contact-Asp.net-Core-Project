
namespace Evolent.Models.Shared
{
    public enum ErrorCodes
    {
        NoError = 0,
        InvalidAppToken = 1000,
        InvalidUserToken = 1001,
        InvalidAppUserToken = 1002,
        RequestTokenAuthenticationFailure = 1003,

        InvalidLoginCredentials = 1010,

        ValidationError = 1020,
        
        FailedToSaveContact = 1027
    }
}
