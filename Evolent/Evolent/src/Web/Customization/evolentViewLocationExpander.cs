using Evolent.Models.Shared;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;

namespace Web.Customization
{
    public class evolentViewLocationExpander : IViewLocationExpander
    {
        string customer = string.Empty;
        public evolentViewLocationExpander()
        {
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            IEnumerable<string> evolentViewLocations = new string[] {
                                                                    "/Views/" + customer + "/{1}/{0}.cshtml",
                                                                    "/Views/" + customer + "/Shared/{0}.cshtml",
                                                                    "/Views/Shared/{0}.cshtml"
                                                                    };

            //return evolentViewLocations.Union(viewLocations);
            return evolentViewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            if (string.IsNullOrEmpty(customer))
            {
                customer = EvolentAppSettings.Customer;
            }
        }
    }
}
