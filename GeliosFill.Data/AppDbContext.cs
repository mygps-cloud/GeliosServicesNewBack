using GeliosFill.Models;
using Microsoft.EntityFrameworkCore;

namespace GeliosFill.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
    }


    public DbSet<UserFillInfo> UserFillInfos { get; set; }
    public DbSet<FuelFillHistory> FuelFillHistories { get; set; }
}