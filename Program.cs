using FileUploadPipeline.Services;
using FileUploadPipeline.Workers;

var builder = Host.CreateApplicationBuilder(args);

// --- register the Services for Dependency Injection

// the Queue is a singleton because we need one central, shared queue for the entire application lifetime
builder.Services.AddSingleton<IMessageQueueService, InMemoryQueueService>();

// the File Uploader can be scoped.  A new instance will be created for each logical operation (like a single simulated upload)
builder.Services.AddScoped<IFileUploader, LocalFileUploader>();

// --- register the background Workers

// add the processor that consumes messages from the queue. 
builder.Services.AddHostedService<FileProcessorWorker>();

// add the simulator that producess files and messages
builder.Services.AddHostedService<FileUploadSimulator>();

var host = builder.Build();

host.Run();