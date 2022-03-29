namespace Minibank.Core.Exceptions
{
    public class ObjectNotFoundException : Exception
    {
        public string Message;
        public ObjectNotFoundException(string message)
        {
            Message = message;
        }
    }
}