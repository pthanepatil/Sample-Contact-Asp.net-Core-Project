
using Evolent.Models.Shared;

namespace Evolent.Models
{
    public class UserResponse : CommonResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserToken { get; set; }
    }
}
