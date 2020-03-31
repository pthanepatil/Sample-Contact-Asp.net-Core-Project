using Microsoft.AspNetCore.Mvc;
using Evolent.Models;
using System.Threading.Tasks;
using Evolent.Models.Shared;
using EvolentContact.API.Services;

namespace EvolentContact.API.Controllers
{
    public class AuthenticationController : Shared.BaseApiController
    {
        #region Declaration
        private IAuthenticationService _authenticationService;
        #endregion

        #region Constructor

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        #endregion
                
        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> LoadUser([FromBody]UserRequest userRequest)
        {
            ErrorResponse errorResponse = await _authenticationService.IsValidToken(this.EvolentUser, false);
            if (errorResponse.HasError)
                return Json(errorResponse);
            else
            {
                return Json(await _authenticationService.LoadUser(userRequest));
            }
        }
    }
}