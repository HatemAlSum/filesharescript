# Azure File Share Directory Size Calculator

A .NET console application that helps you analyze and monitor the size of directories in your Azure File Share. This tool recursively traverses through your Azure File Share directories and displays their sizes in a hierarchical format.

## Features

- Connects to Azure File Share using secure connection strings
- Recursively lists all directories in the specified file share
- Calculates total size of each directory including subdirectories
- Displays directory sizes in megabytes (MB)
- Presents directory structure in an easy-to-read hierarchical format
- Handles errors gracefully with informative error messages

## Prerequisites

- .NET 8.0 SDK or later
- An Azure Storage Account with File Share
- Access to Azure File Share connection string and share name

## Configuration

1. Create an `appsettings.json` file in the project root (if not exists) with the following structure:

```json
{
  "AzureStorage": {
    "ConnectionString": "your_connection_string_here",
    "ShareName": "your_share_name_here"
  }
}
```

2. Replace the placeholder values:
   - `your_connection_string_here`: Your Azure Storage connection string
   - `your_share_name_here`: The name of your Azure File Share

## Building and Running

1. Clone this repository
2. Navigate to the project directory
3. Build the project:
   ```
   dotnet build
   ```
4. Run the application:
   ```
   dotnet run
   ```

## Output Format

The application outputs directory information in the following format:
```
DirectoryName - Size: XX.XX MB
    SubDirectory1 - Size: XX.XX MB
    SubDirectory2 - Size: XX.XX MB
        SubSubDirectory - Size: XX.XX MB
```

## Dependencies

- Azure.Storage.Files.Shares (12.22.0)
- Microsoft.Extensions.Configuration.Json (9.0.4)

## Error Handling

The application includes comprehensive error handling for common scenarios:
- Missing configuration values
- Invalid connection strings
- Network connectivity issues
- Access permission problems

## Security Note

Never commit your actual connection string to source control. Consider using:
- Environment variables
- Azure Key Vault
- User secrets in development

## License
This project is licensed under the MIT License. Feel free to use, modify, and contribute!
