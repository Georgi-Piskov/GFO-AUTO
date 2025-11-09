using Microsoft.Extensions.Hosting;

namespace NoActivityFiler.Services;

public class TempFolderCleanupService : BackgroundService
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<TempFolderCleanupService> _logger;

    public TempFolderCleanupService(IWebHostEnvironment env, ILogger<TempFolderCleanupService> logger)
    {
        _env = env;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var root = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "tmp");
        Directory.CreateDirectory(root);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var cutoff = DateTimeOffset.UtcNow - TimeSpan.FromHours(24);
                foreach (var dir in Directory.EnumerateDirectories(root))
                {
                    try
                    {
                        var di = new DirectoryInfo(dir);
                        if (di.CreationTimeUtc < cutoff)
                        {
                            di.Delete(true);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to delete temp dir {Dir}", dir);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Temp cleanup failed");
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}

