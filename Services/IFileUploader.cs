namespace FileUploadPipeline.Services;

public interface IFileUploader
{
    /// <summary>
    /// Uploads a file's content to a storage location. 
    /// </summary>
    /// <param name="fileName">Teh name of the file.</param>
    /// <param name="content">The stream of the file's content</param>
    /// <returns>a string representing the unique identifier or path to the uploaded file.</returns>
    Task<string> UploadFileAsync(string fileName, Stream content);
}