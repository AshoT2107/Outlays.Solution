using Microsoft.EntityFrameworkCore;
using Outlays.Data.Data;
using Outlays.Data.Entities;

namespace Outlays.Bot.Repository;

public class UserRepository
{
    private readonly OutlaysDbContext _context;

    public UserRepository(OutlaysDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByChatId(long chatId)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.ChatId == chatId);
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
        _context.Update(user);
        await _context.SaveChangesAsync();
    }
}