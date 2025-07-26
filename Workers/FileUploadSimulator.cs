using System.Text;
using FileUploadPipeline.Services;

namespace FileUploadPipeline.Workers;


public class FileUploadSimulator : BackgroundService
{
    private readonly ILogger<FileUploadSimulator> _logger;
    private readonly IServiceProvider _serviceProvider;

    public FileUploadSimulator(ILogger<FileUploadSimulator> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("File Upload Simulator starting. ");

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            using (var scope = _serviceProvider.CreateScope())
            {
                var uploader = scope.ServiceProvider.GetRequiredService<IFileUploader>();
                var fileName = $"document_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                var fileContent = $"This is a sample file uploaded at {DateTime.UtcNow}.";

                _logger.LogInformation("Simulatinf upload for file: {FileName}", fileName);

                var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
                await uploader.UploadFileAsync(fileName, memoryStream);
            }
        }

        _logger.LogInformation("File Upload Simulator stopping");
    }

}