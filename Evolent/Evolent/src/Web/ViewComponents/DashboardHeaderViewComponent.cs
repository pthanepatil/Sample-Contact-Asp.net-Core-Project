using Evolent.Models.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewComponents
{
    public class DashboardHeaderViewComponent : ViewComponent
    {
        #region Declaration
        private IEvolentUser evolentUser
        {
            get
            {
                return HttpContext.RequestServices.GetService(typeof(IEvolentUser)) as IEvolentUser;
            }
        }
        #endregion

        #region Constructor 
        public DashboardHeaderViewComponent()
        {            
        }
        #endregion

        #region Public Methods
        public IViewComponentResult Invoke()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.DisplayName = evolentUser.UserName.Trim();
                ViewBag.CompanyName = "Evolent Health";
                ViewBag.PendingNotificationsCount = 0;
            }

            return View();
        }
        #endregion
    }
}
