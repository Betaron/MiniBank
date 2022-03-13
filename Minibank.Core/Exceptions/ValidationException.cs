namespace Minibank.Core.Utility
{
    public class ValidationException : Exception
    {
        public string validationMessage;

        public ValidationException(string validationMessage)
        {
            this.validationMessage = validationMessage;
        }
    }
}
