using Evolent.Business.Contracts;
using Evolent.Models;
using Evolent.Models.Shared;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class AuthenticationController : BaseController
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

        #region Public Methods

        [HttpGet]
        public IActionResult Login()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity.IsAuthenticated)
            {
                //GO TO DASHBOARD
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return View(new UserRequest() { UserName = "evolent", Password = "" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserRequest userLoginRequest)
        {
            if (ModelState.IsValid)
            {
                UserResponse response = new UserResponse();
                response = await _authenticationService.Login(userLoginRequest, this.EvolentUser);

                if (!response.HasError)
                {
                    //Set user claims
                    ClaimsIdentity userClaims = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
                    await SetUserClaims(userClaims, userLoginRequest, response);

                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ViewBag.LoginStatus = "ERROR";
                    return View(userLoginRequest);
                }
            }

            return View(userLoginRequest);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            //await _authenticationService.Logout(EvolentUser);
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Authentication");
        }

        #endregion

        #region Private Methods

        private async Task SetUserClaims(ClaimsIdentity userClaims, UserRequest loginRequest, UserResponse loginResponse)
        {
            userClaims.AddClaim(new Claim(EvolentClaimTypes.AppToken, this.EvolentUser.AppToken));
            userClaims.AddClaim(new Claim(EvolentClaimTypes.UserToken, loginResponse.UserToken));
            userClaims.AddClaim(new Claim(EvolentClaimTypes.UserId, loginResponse.UserId.ToString()));
            userClaims.AddClaim(new Claim(EvolentClaimTypes.UserName, loginResponse.UserName));

            //userClaims.AddClaim(new Claim(EvolentClaimTypes.CompanyName, await _metaDataService.GetCompanyName(loginResponse.CompanyId.GetValueOrDefault())));
            //SetUserRoleClaims(userClaims, loginResponse.UserRoles);
            //SetModuleAccessClaims(userClaims, loginResponse.ModuleAccess);

            ClaimsPrincipal principal = new ClaimsPrincipal(userClaims);
            Thread.CurrentPrincipal = principal;
            await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                IsPersistent = loginRequest.RememberMe
            });
        }


        #endregion

    }
}
