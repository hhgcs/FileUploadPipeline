using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace FileUploadPipeline.Services;

public class LocalFileUploader : IFileUploader
{
    private readonly IMessageQueueService _messageQueueService;
    private readonly ILogger<LocalFileUploader> _logger;
    string tempPath = Path.GetTempPath();
    private readonly string _uploadDirectory;

    public LocalFileUploader(IMessageQueueService messageQueueService, ILogger<LocalFileUploader> logger)
    {
        _messageQueueService = messageQueueService;
        _logger = logger;
        var tempPath = Path.GetTempPath();
        _uploadDirectory = Path.Combine(tempPath, "FileUploadPipeline", "uploads");
        Directory.CreateDirectory(_uploadDirectory);
    }


    public async Task<string> UploadFileAsync(string fileName, Stream content)
    {
        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
        var filePath = Path.Combine(_uploadDirectory, uniqueFileName);

        _logger.LogInformation("UPloading file to: {FilePath}", filePath);

        // use the stream to write to the file
        try
        {
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await content.CopyToAsync(fileStream);
            }

            await _messageQueueService.EnqueueMessageAsync(uniqueFileName);
            _logger.LogInformation("Successfully enqueued message for file: {FileName}", uniqueFileName);
            return filePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload file {FileName}", fileName);
            throw;
        }
    }
} 