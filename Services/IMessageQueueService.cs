namespace FileUploadPipeline.Services;

public interface IMessageQueueService
{
    ///<summary>
    /// Adds a message to the queue.
    /// <summary>
    /// <param name="message">The message to be enqueued. </param>
    Task EnqueueMessageAsync(string message);

    ///<summary>
    /// Retrieves and removes the message from the queue.
    /// </summary>
    /// <param name="cancellationToken"> A token to observe for cancellation requests. </param>
    /// <returns> The dequeued message, or null if the queue is empty.</returns>
    Task<string?> DequeueMessageAsync(CancellationToken cancellationToken);
}