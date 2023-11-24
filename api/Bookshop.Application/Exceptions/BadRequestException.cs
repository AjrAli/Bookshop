namespace Bookshop.Application.Exceptions
{

    public class BadRequestException: BaseException
    {
        public BadRequestException(string message) : base(message)
        {
        }
        public BadRequestException(string message, IList<string> errors) : base(message, errors)
        {
        }
    }
}
