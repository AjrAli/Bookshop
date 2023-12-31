namespace Bookshop.Api.Utility
{
    /// <summary>
    /// Configuration options for serving static files with custom caching.
    /// </summary>
    public class StaticFilesConfig
    {
        /// <summary>
        /// Gets or sets the path to the client folder.
        /// </summary>
        public string ClientFolderPath { get; set; } = "Client";

        /// <summary>
        /// Gets or sets the request path for serving the client files.
        /// </summary>
        public string RequestPath { get; set; } = "/client";

        /// <summary>
        /// Gets or sets the duration for which the files should be cached in seconds.
        /// </summary>
        public int CacheDurationInSeconds { get; set; } = 60 * 60 * 24; // 1 day

        /// <summary>
        /// Gets or sets the mappings of file extensions to MIME types for custom content types.
        /// </summary>
        public Dictionary<string, string> ContentTypeMappings { get; set; } = new Dictionary<string, string>
    {
        { ".css", "text/css" },
        { ".js", "application/javascript" },
        { ".png", "image/png" },
        { ".jpg", "image/jpeg" },
        // Add more mappings as needed
    };
    }
}
