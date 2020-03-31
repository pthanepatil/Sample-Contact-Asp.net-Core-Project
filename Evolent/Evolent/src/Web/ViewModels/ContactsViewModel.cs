using Evolent.Models;
using Evolent.Models.Shared;
using System.Collections.Generic;

namespace Web.ViewModels
{
    public class ContactsViewModel
    {
        public List<ContactModel> Contacts { get; set; }
        public PagerModel Pager { get; set; }
    }
}
