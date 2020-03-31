using Evolent.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace EvolentContact.API.Controllers.Shared
{
    public class BaseApiController : Controller
    {
        /// <summary>
        /// Get Value from claim
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="claimsType">Type of claim.</param>
        /// <returns></returns>
        protected T GetValueFromClaims<T>(string claimsType)
        {
            var claim = ((ClaimsIdentity)User.Identity).FindFirst(claimsType);
            if (claim != null)
            {
                object claimValue = claim.Value;
                return (T)Convert.ChangeType(claimValue, typeof(T));
            }

            return default(T);
        }

        public IEvolentUser EvolentUser
        {
            get
            {
                return HttpContext.RequestServices.GetService(typeof(IEvolentUser)) as IEvolentUser;
            }
        }
    }
}
