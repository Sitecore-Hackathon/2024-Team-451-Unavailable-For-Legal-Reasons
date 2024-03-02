using NewsMixer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly Pipeline _pipeline;

    public Worker(ILogger<Worker> logger, Pipeline pipeline)
    {
        _logger = logger;
        _pipeline = pipeline;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("This is the NewsMixer!");
            _logger.LogInformation("Error 451 Unavailable for Legal Reasons");
        }

        await _pipeline.Execute(stoppingToken);
    }
}
