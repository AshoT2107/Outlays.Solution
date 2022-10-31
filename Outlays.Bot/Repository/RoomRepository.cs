using Microsoft.EntityFrameworkCore;
using Outlays.Data.Data;
using Outlays.Data.Entities;

namespace Outlays.Bot.Repository;

public class RoomRepository
{
    private readonly OutlaysDbContext _context;

    public RoomRepository(OutlaysDbContext context)
    {
        _context = context;
    }

    public async Task<Room?> GetRoomById(int id)
    {
        return await _context.Rooms.FindAsync(id);
    }
    public async Task AddRoomAsync(Room room)
    {
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRoom(Room room)
    {
        _context.Update(room);
        await _context.SaveChangesAsync();
    }
}