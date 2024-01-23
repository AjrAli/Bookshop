namespace Bookshop.Api.Utility.Service
{
    public interface IFileService
    {
        bool IsFileValid(IFormFile? file);
        Task<byte[]?> GetFileContent(IFormFile? file);
    }
}
