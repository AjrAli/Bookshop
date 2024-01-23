
namespace Bookshop.Api.Utility.Service
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;
        public FileService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool IsFileValid(IFormFile? file)
        {
            if (file == null ||
                !file.ContentType.StartsWith("image/") ||
                !IsFileSignatureValid(file) ||
                !int.TryParse(_configuration.GetSection("FileSizeLimit")?.Value, out int fileSizeLimit) ||
                file.Length > fileSizeLimit ||
                file.Length == 0)
                return false;
            return true;
        }
        public async Task<byte[]?> GetFileContent(IFormFile? file)
        {
            if (file == null)
                return null;
            byte[]? fileContents = null;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                fileContents = stream.ToArray();
            }
            return fileContents;
        }
        private bool IsFileSignatureValid(IFormFile file)
        {
            // Define valid file signatures for allowed file types
            var allowedFileSignatures = new Dictionary<string, byte[]>
            {
                { "image/jpeg", new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 } }, // JPEG
                { "image/png", new byte[] { 0x89, 0x50, 0x4E, 0x47 } }, // PNG
                { "image/gif", new byte[] { 0x47, 0x49, 0x46, 0x38 } } // GIF
            };

            // Read the first few bytes of the file to check the signature
            using (var stream = file.OpenReadStream())
            {
                byte[] fileSignature = new byte[4];
                stream.Read(fileSignature, 0, 4);

                // Check if the file signature matches the expected signature
                foreach (var allowedSignature in allowedFileSignatures)
                {
                    if (fileSignature.Take(allowedSignature.Value.Length).SequenceEqual(allowedSignature.Value))
                    {
                        return true; // Valid signature found
                    }
                }
            }
            return false; // No valid signature found
        }
    }
}
