using Azure.Storage.Files.Shares;
using Azure;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main(string[] args)
    {
        // Load configuration from appsettings.json
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        // Get Azure Storage settings from configuration
        string connectionString = configuration["AzureStorage:ConnectionString"]
            ?? throw new InvalidOperationException("Connection string not found in configuration");
        string shareName = configuration["AzureStorage:ShareName"]
            ?? throw new InvalidOperationException("File share name not found in configuration");

        try
        {
            // Create a ShareClient
            var shareClient = new ShareClient(connectionString, shareName);

            // Get the share's root directory
            ShareDirectoryClient rootDir = shareClient.GetRootDirectoryClient();

            // List all directories and their sizes
            await ListDirectoriesAndSizesAsync(rootDir, "");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static async Task ListDirectoriesAndSizesAsync(ShareDirectoryClient directory, string prefix)
    {
        try
        {
            // List all directories
            await foreach (var item in directory.GetFilesAndDirectoriesAsync())
            {
                if (item.IsDirectory)
                {
                    var subDir = directory.GetSubdirectoryClient(item.Name);
                    long directorySize = await CalculateDirectorySizeAsync(subDir);
                    
                    // Convert bytes to MB for better readability
                    double sizeMB = directorySize / (1024.0 * 1024.0);
                    Console.WriteLine($"{prefix}{item.Name} - Size: {sizeMB:F2} MB");
                    
                    // Recursively list subdirectories
                    await ListDirectoriesAndSizesAsync(subDir, prefix + "    ");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error accessing directory: {ex.Message}");
        }
    }

    static async Task<long> CalculateDirectorySizeAsync(ShareDirectoryClient directory)
    {
        long totalSize = 0;

        try
        {
            await foreach (var item in directory.GetFilesAndDirectoriesAsync())
            {
                if (item.IsDirectory)
                {
                    var subDir = directory.GetSubdirectoryClient(item.Name);
                    totalSize += await CalculateDirectorySizeAsync(subDir);
                }
                else
                {
                    var file = directory.GetFileClient(item.Name);
                    var properties = await file.GetPropertiesAsync();
                    totalSize += properties.Value.ContentLength;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calculating size: {ex.Message}");
        }

        return totalSize;
    }
}
