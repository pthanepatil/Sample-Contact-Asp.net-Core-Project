using Evolent.Database.Contracts;
using Evolent.Models;
using Evolent.Models.Shared;
using EvolentContact.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EvolentContact.API.Controllers
{
    public class UserContactController : Shared.BaseApiController
    {
        #region Declaration
        private IAuthenticationService _authenticationService;
        private IContactRepository _contactRepository;
        #endregion

        #region Constructor

        public UserContactController(IAuthenticationService authenticationService, IContactRepository contactRepository)
        {
            _authenticationService = authenticationService;
            _contactRepository = contactRepository;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> Get(int? contactId)
        {
            ErrorResponse errorResponse = await _authenticationService.IsValidToken(this.EvolentUser, true);
            if (errorResponse.HasError)
                return Json(errorResponse);
            else
            {
                return Json(await _contactRepository.LoadUserContacts(this.EvolentUser.UserId.Value, contactId));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody]ContactModel contact)
        {
            ErrorResponse errorResponse = await _authenticationService.IsValidToken(this.EvolentUser, true);
            if (errorResponse.HasError)
                return Json(errorResponse);
            else
            {
                try
                {
                    if ((await _contactRepository.SaveUserContacts(this.EvolentUser.UserId.Value, contact)) > 0)
                        return Json(new CommonResponse());
                }
                catch (Exception ex)
                {
                    ErrorResponse error = new ErrorResponse(ErrorCodes.FailedToSaveContact);
                    return Json(error);
                }
            }

            return null;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateContactStatus([FromBody]ContactModel contact)
        {
            ErrorResponse errorResponse = await _authenticationService.IsValidToken(this.EvolentUser, true);
            if (errorResponse.HasError)
                return Json(errorResponse);
            else
            {
                try
                {
                    if((await _contactRepository.UpdateContactStatus(this.EvolentUser.UserId.Value, contact.ContactId, contact.Status)) > 0)
                        return Json(new CommonResponse());
                }
                catch (Exception ex)
                {
                    ErrorResponse error = new ErrorResponse(ErrorCodes.FailedToSaveContact);
                    return Json(error);
                }
            }

            return null;
        }
    }
}
