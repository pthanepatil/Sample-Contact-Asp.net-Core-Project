using System.Collections.Generic;
using System.Text;

namespace Evolent.Models.Shared
{
    public class CommonResponse
    {
        public bool HasError
        {
            get
            {
                if (Errors == null)
                    return false;
                if (Errors.Count == 0)
                    return false;
                else
                    return true;
            }
        }
        public List<ErrorResponse> Errors { get; set; }      
        public void SetError(ErrorResponse error)
        {
            if (Errors == null)
                Errors = new List<ErrorResponse>();
            Errors.Add(error);
        }
        public void SetError(ErrorCodes errorCode)
        {
            if (Errors == null)
                Errors = new List<ErrorResponse>();
            ErrorResponse error = new ErrorResponse();
            error.SetError(errorCode);
            Errors.Add(error);
        }
        public void SetError(List<ErrorResponse> errors)
        {
            Errors = errors;
        }
        public string GetErrorMessage()
        {
            StringBuilder errorMessage = new StringBuilder();
            if (HasError)
            {
                foreach (ErrorResponse error in Errors)
                {
                    errorMessage.AppendLine(error.ErrorMessage);
                }
            }
            return errorMessage.ToString();
        }
    }
}
