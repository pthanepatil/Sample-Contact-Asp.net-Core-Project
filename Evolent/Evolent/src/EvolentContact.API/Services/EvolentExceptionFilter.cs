using Microsoft.AspNetCore.Mvc.Filters;

namespace EvolentContact.API.Services
{
    public class EvolentExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            //Logic to log exception
        }
    }
}
