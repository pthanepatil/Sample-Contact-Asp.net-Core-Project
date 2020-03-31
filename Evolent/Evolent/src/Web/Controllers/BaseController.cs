using Evolent.Models.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using Web.Customization;

namespace Web.Controllers
{
    [ServiceFilter(typeof(evolentExceptionFilterService))]
    public class BaseController : Controller
    {
        public IEvolentUser EvolentUser
        {
            get
            {
                return HttpContext.RequestServices.GetService(typeof(IEvolentUser)) as IEvolentUser;
            }
        }

        protected DateTime GetDateFromMMDDYYYY(string date)
        {
            bool isValidDate = false;
            DateTime dt = new DateTime();
            isValidDate = DateTime.TryParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "MM/dd/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "MM-dd-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "MMM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "MMM/dd/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "MMM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "MMM-dd-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);

            return dt;
        }

        protected DateTime? GetParseDateValue(string date)
        {
            bool isValidDate = false;
            DateTime dt = new DateTime();
            isValidDate = DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "dd/MMM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "dd/MMM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "dd-MMM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);

            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "d/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "dd/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "d/M/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);

            if (!isValidDate)
                return null;
            return dt;
        }

        protected DateTime? GetParseDateTimeValue(string date)
        {
            bool isValidDate = false;
            DateTime dt = new DateTime();
            isValidDate = DateTime.TryParseExact(date, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "dd/MM/yy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "dd-MM-yyyy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "dd-MM-yy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "dd/MMM/yyyy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "dd/MMM/yy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                isValidDate = DateTime.TryParseExact(date, "dd-MMM-yy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (!isValidDate)
                return null;
            return dt;
        }

        protected bool GetBoolFromString(string value)
        {
            if (value == null)
                return false;
            switch (value.Trim().ToLower())
            {
                case "true":
                    return true;
                case "t":
                    return true;
                case "1":
                    return true;
                case "yes":
                    return true;
                case "y":
                    return true;
                default:
                    return false;
            }
        }

        protected int GetInt32FromString(string value)
        {
            int i;
            int.TryParse(value, out i);
            return i;
        }

        protected long GetLongFromString(string value)
        {
            long i;
            long.TryParse(value, out i);
            return i;
        }

        protected decimal GetDecimalFromString(string value)
        {
            decimal i;
            decimal.TryParse(value, out i);
            return i;
        }

    }
}
