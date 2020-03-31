using Evolent.Business.Contracts;
using Evolent.Models;
using Evolent.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels;
using static Evolent.Models.Shared.Constants;

namespace Web.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private IUserContactService _userContactService;

        public DashboardController(IUserContactService userContactService)
        {
            _userContactService = userContactService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.SelectedMenu = AppMenus.HOME;
            return View("Index");
        }

        public async Task<IActionResult> GetContacts(int? contactId, int? PageNo = 1, int PageSize = 10, string SortBy = null)
        {
            ContactsViewModel model = new ContactsViewModel();
            var results = await _userContactService.Get(contactId, this.EvolentUser);

            model.Contacts = results.Skip((PageNo.Value - 1) * PageSize).Take(PageSize).ToList();
            model.Pager = new PagerModel();
            model.Pager.TotalRecords = results.Count;
            model.Pager.CurrentPage = PageNo.Value;
            model.Pager.PageSize = PageSize;

            return View("_ContactList", model);
        }

        [HttpGet]
        public async Task<IActionResult> AddContact()
        {
            ViewBag.SelectedMenu = AppMenus.ADD_CONTACT;
            ViewBag.HasModelError = false;
            ContactViewModel model = new ContactViewModel();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditContact(string encryptedContactId)
        {
            ContactViewModel model = new ContactViewModel();

            int contactId = Convert.ToInt32(Common.EncryptDecryptAlgorithm.Decrypt(encryptedContactId));
            var results = await _userContactService.Get(contactId, this.EvolentUser);
            foreach (ContactModel contact in results)
            {
                model.encryptedContactId = Common.EncryptDecryptAlgorithm.Encrypt(contact.ContactId.ToString());
                model.FirstName = contact.FirstName;
                model.LastName = contact.LastName;
                model.PhoneNumber = contact.PhoneNumber;
                model.Email = contact.Email;
                model.Status = contact.Status;
                break;
            }

            ViewBag.SelectedMenu = AppMenus.EDIT_CONTACT;
            ViewBag.HasModelError = false;
            return View("AddContact", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(ContactViewModel model)
        {
            CommonResponse response = null;
            if (!ModelState.IsValid)
            {
                ViewBag.HasModelError = true;
                return View(model);
            }
            else
            {
                ContactModel contact = new ContactModel();
                if (!string.IsNullOrEmpty(model.encryptedContactId))
                    contact.ContactId = Convert.ToInt32(Common.EncryptDecryptAlgorithm.Decrypt(model.encryptedContactId));
                contact.FirstName = model.FirstName;
                contact.LastName = model.LastName;
                contact.PhoneNumber = model.PhoneNumber;
                contact.Email = model.Email;
                contact.Status = model.Status;

                response = await _userContactService.Save(contact, EvolentUser);
            }

            if (response == null || response.HasError)
            {
                ViewBag.SelectedMenu = (string.IsNullOrEmpty(model.encryptedContactId)) ? AppMenus.ADD_CONTACT : AppMenus.EDIT_CONTACT;
                ViewBag.Message = (response == null) ? "Contact not saved!" : response.GetErrorMessage();

                return View(model);
            }

            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> DeleteContact(string encryptedContactId)
        {
            int contactId = Convert.ToInt32(Common.EncryptDecryptAlgorithm.Decrypt(encryptedContactId));
            CommonResponse response = await _userContactService.UpdateContactStatus(contactId, false, this.EvolentUser);
            if (response == null || response.HasError)
            {
                ViewBag.Message = (response == null) ? "Failed to delete contact!" : response.GetErrorMessage();
            }

            return RedirectToAction("Index");
        }
    }
}
