using GeliosFill.Models;
using Microsoft.EntityFrameworkCore;

namespace GeliosFill.Data;

public class MyGpsDbContext : DbContext
{
    public MyGpsDbContext(DbContextOptions<MyGpsDbContext> options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Fill>().HasNoKey();
        modelBuilder.Entity<Ags>().HasNoKey();
    }

    public DbSet<Fill> Fills { get; set; }
    public DbSet<Ags> AGS { get; set; }
}