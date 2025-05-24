namespace BSC.Infrastructure.FileStorage
{
    public class FileStorageLocal : IFileStorageLocal
    {
        public async Task<string> SaveFile(string container, string file, string webRootPath, string scheme, string host)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentException("File content cannot be empty.", nameof(file));
            if (string.IsNullOrWhiteSpace(container))
                throw new ArgumentException("Container cannot be empty.", nameof(container));
            if (string.IsNullOrWhiteSpace(webRootPath))
                throw new ArgumentException("WebRootPath cannot be empty.", nameof(webRootPath));

            // Extract file name and extension (if provided in the format "filename|base64")
            string fileNameExtracted = string.Empty;
            string base64Content = file;
            if (file.Contains("|"))
            {
                var parts = file.Split("|");
                fileNameExtracted = parts[0];
                base64Content = parts[1];
            }

            // Remove the Base64 prefix (e.g., "data:image/jpeg;base64,") if present
            if (base64Content.Contains(","))
            {
                base64Content = base64Content.Substring(base64Content.IndexOf(",") + 1);
            }

            // Generate file name with extension
            var extension = string.IsNullOrEmpty(fileNameExtracted) ? ".bin" : Path.GetExtension(fileNameExtracted);
            if (string.IsNullOrEmpty(extension))
                extension = ".bin"; // Default extension if none provided
            var fileName = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(webRootPath, container);

            // Create directory if it doesn't exist
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string path = Path.Combine(folder, fileName);

            try
            {
                // Decode Base64 string to byte array
                var content = Convert.FromBase64String(base64Content);

                // Write the content to the file
                await File.WriteAllBytesAsync(path, content);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("Invalid Base64 string format.", nameof(file), ex);
            }

            // Construct the URL for the database
            var currentUrl = $"{scheme}://{host}";
            var pathDb = Path.Combine(currentUrl, container, fileName).Replace("\\", "/");
            return pathDb;
        }

        public async Task<string> EditFile(string container, string file, string route, string webRootPath, string scheme, string host)
        {
            // Remove the existing file if route is provided
            await RemoveFile(route, container, webRootPath);

            // Save the new file
            return await SaveFile(container, file, webRootPath, scheme, host);
        }

        public Task RemoveFile(string route, string container, string webRootPath)
        {
            if (string.IsNullOrEmpty(route))
                return Task.CompletedTask;

            var fileName = Path.GetFileName(route);
            var directoryFile = Path.Combine(webRootPath, container, fileName);

            if (File.Exists(directoryFile))
                File.Delete(directoryFile);

            return Task.CompletedTask;
        }
    }
}