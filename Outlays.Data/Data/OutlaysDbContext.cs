using Microsoft.EntityFrameworkCore;
using Outlays.Data.Entities;

namespace Outlays.Data.Data;

public class OutlaysDbContext : DbContext
{
    public OutlaysDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Outlay> Outlays { get; set; }

}