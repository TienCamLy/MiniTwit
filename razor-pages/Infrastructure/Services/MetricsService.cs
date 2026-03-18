using Infrastructure.Context;
using Prometheus;

public class MetricsService
{
    private static readonly Gauge UsersGauge = Metrics.CreateGauge(
        "minitwit_users_total",
        "Total registered users"
    );

    private static readonly Gauge TweetsGauge = Metrics.CreateGauge(
        "minitwit_tweets_total",
        "Total tweets created"
    );

    private readonly MiniTwitContext _context;

    public MetricsService(MiniTwitContext context)
    {
        _context = context;
        UpdateMetrics();
    }

    public void UpdateMetrics()
    {
        UsersGauge.Set(_context.Users.Count());
        TweetsGauge.Set(_context.Messages.Count());
    }
}