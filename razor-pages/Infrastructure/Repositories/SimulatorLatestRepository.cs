using Core.Interfaces;
using Infrastructure.Context;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SimulatorLatestRepository : ISimulatorLatestRepository
{
    private readonly MiniTwitContext _context;

    public SimulatorLatestRepository(MiniTwitContext context)
    {
        _context = context;
    }

    public int GetLatestId() =>
        _context.SimulatorLatestState.AsNoTracking()
            .Where(r => r.Id == SimulatorLatest.SingletonRowId)
            .Select(r => r.LatestId)
            .FirstOrDefault();

    public void SetLatestId(int value)
    {
        var row = _context.SimulatorLatestState.Find(SimulatorLatest.SingletonRowId);
        if (row is null)
        {
            _context.SimulatorLatestState.Add(new SimulatorLatest
            {
                Id = SimulatorLatest.SingletonRowId,
                LatestId = value
            });
        }
        else
        {
            row.LatestId = value;
        }

        _context.SaveChanges();
    }

    public void IncrementLatestId()
    {
        var row = _context.SimulatorLatestState.Find(SimulatorLatest.SingletonRowId);
        if (row is null)
        {
            _context.SimulatorLatestState.Add(new SimulatorLatest
            {
                Id = SimulatorLatest.SingletonRowId,
                LatestId = 1
            });
        }
        else
        {
            row.LatestId++;
        }

        _context.SaveChanges();
    }
}
