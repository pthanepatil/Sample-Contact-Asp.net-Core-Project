using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Customization
{
    public class evolentExceptionFilterService : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            //logic to catch excetion logs.
        }
    }
}
