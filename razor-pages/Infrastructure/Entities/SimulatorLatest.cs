namespace Infrastructure.Entities;

/// <summary>
/// Single-row table: last processed simulator command id (query param <c>latest</c>).
/// </summary>
public class SimulatorLatest
{
    public const int SingletonRowId = 1;

    public int Id { get; set; } = SingletonRowId;

    public int LatestId { get; set; }
}
