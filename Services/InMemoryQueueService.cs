using System.Collections.Concurrent;

namespace FileUploadPipeline.Services;

public class InMemoryQueueService : IMessageQueueService
{
    private readonly ConcurrentQueue<string> _queue = new();
    private readonly ILogger<InMemoryQueueService> _logger;

    public InMemoryQueueService(ILogger<InMemoryQueueService> logger)
    {
        _logger = logger;
    }

    public Task<string?> DequeueMessageAsync(CancellationToken cancellationToken)
    {
        if (_queue.TryDequeue(out var message))
        {
            _logger.LogInformation("Dequeuing message: {Message}", message);
            return Task.FromResult<string?>(message);
        }
        return Task.FromResult <string?> (null);
    }

    public Task EnqueueMessageAsync(string message)
    {
        _logger.LogInformation("enqueueing message: {Message}", message);
        _queue.Enqueue(message);
        return Task.CompletedTask;
    }
}