using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Services;

public class MetricsUpdater : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public MetricsUpdater(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var metricsService = scope.ServiceProvider.GetRequiredService<MetricsService>();
                metricsService.UpdateMetrics();
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}