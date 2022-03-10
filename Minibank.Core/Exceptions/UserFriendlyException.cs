namespace Minibank.Core.Utility
{
    public class UserFriendlyException : Exception
    {
        public string userFriendlyMessage;

        public UserFriendlyException(string userFriendlyMessage)
        {
            this.userFriendlyMessage = userFriendlyMessage;
        }
    }
}
