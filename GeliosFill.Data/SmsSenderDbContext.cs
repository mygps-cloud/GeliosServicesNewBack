using GeliosFill.Models;
using Microsoft.EntityFrameworkCore;

namespace GeliosFill.Data;

public class SmsSenderDbContext : DbContext
{
    public SmsSenderDbContext(DbContextOptions<SmsSenderDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ReceivedSms>().HasNoKey();
    }

    public DbSet<ReceivedSms> ReceivedSms { get; set; }
}