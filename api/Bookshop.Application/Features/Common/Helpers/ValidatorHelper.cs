namespace Bookshop.Application.Features.Common.Helpers
{
    public static class ValidatorHelper
    {
        public static bool BeAValidDate(string value)
        {
            return DateTime.TryParse(value, out _);
        }
    }
}
