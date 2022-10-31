using Microsoft.EntityFrameworkCore;
using Outlays.Data.Data;
using Outlays.Data.Entities;

namespace Outlays.Bot.Repository;

public class OutlayRepository
{
    private readonly OutlaysDbContext _context;

    public OutlayRepository(OutlaysDbContext context)
    {
        _context = context;
    }
    public async Task<Outlay?> GetOutlayByUserId(int userId)
    {
        return await _context.Outlays.Where(u => u.UserId == userId && u.Cost == 0).FirstOrDefaultAsync();
    }
    public async Task<List<Outlay>>GetOutlaysOfUser(int userId)
    {
        var outlays = await _context.Outlays.Where(o => o.UserId == userId).ToListAsync();
        return outlays;
    }
    public async Task AddOutlayAsync(Outlay outlay)
    {
        await _context.Outlays.AddAsync(outlay);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateOutlay(Outlay outlay)
    {
        _context.Outlays.Update(outlay);
        await _context.SaveChangesAsync();
    }
}