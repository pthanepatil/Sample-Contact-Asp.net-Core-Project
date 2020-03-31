using Evolent.Models.Resource.Shared;

namespace Evolent.Models.Shared
{
    public class ErrorResponse
    {
        private const string RESOURCE_MESSAGE_KEY_PREFIX = "ERROR";

        public ErrorCodes ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public ErrorResponse()
        {
            NoError();
        }

        public ErrorResponse(ErrorCodes errorCode)
        {
            ErrorCode = errorCode;

            string messageKey = RESOURCE_MESSAGE_KEY_PREFIX + errorCode.ToString("D"); // D is the format specifier to get the integer value associated with the specified enum value
            ExceptionMessages.ResourceManager.IgnoreCase = true; // allows to search using case in-sensitive key search for GetString function
            ErrorMessage = ExceptionMessages.ResourceManager.GetString(messageKey); // if specified messageKey is not available in the resource file, this would set null value

        }

        public ErrorResponse(ErrorCodes errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public ErrorResponse(string errorMessage)
        {
            ErrorCode = ErrorCodes.ValidationError;
            ErrorMessage = errorMessage;
        }

        public void NoError()
        {
            ErrorCode = ErrorCodes.NoError;
            ErrorMessage = string.Empty;
        }

        public static ErrorResponse GetNoError()
        {
            return new ErrorResponse(ErrorCodes.NoError, string.Empty);
        }

        public void SetError(ErrorCodes ErrorCode)
        {
            this.ErrorCode = ErrorCode;
            this.ErrorMessage = ErrorCode.ToString();
            //this.ErrorMessage = ??? from Resource file
        }

        public void SetError(ErrorResponse err)
        {
            this.ErrorCode = err.ErrorCode;
            this.ErrorMessage = err.ErrorMessage;
            //this.ErrorMessage = ??? from Resource file
        }

        public bool HasError
        {
            get
            {
                if (ErrorCode == ErrorCodes.NoError)
                    return false;
                else
                    return true;
            }
        }
    }
}
