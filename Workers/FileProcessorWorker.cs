using FileUploadPipeline.Services;

namespace FileUploadPipeline.Workers;

public class FileProcessorWorker : BackgroundService
{
    private readonly ILogger<FileProcessorWorker> _logger;
    private readonly IMessageQueueService _messageQueueService;

    public FileProcessorWorker(ILogger<FileProcessorWorker> logger, IMessageQueueService messageQueueService)
    {
        _logger = logger;
        _messageQueueService = messageQueueService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("File Processor worker starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var fileName = await _messageQueueService.DequeueMessageAsync(stoppingToken);

            if (fileName != null)
            {
                _logger.LogInformation("Processing file: {FileName}", fileName);
                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                _logger.LogInformation("Finished processing file: {FileName}", fileName);
            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
        _logger.LogInformation("File Processor Worker stopping.");
    }

}
