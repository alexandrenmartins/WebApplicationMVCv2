using Microsoft.EntityFrameworkCore;
using WebApplicationMVCv2.Models;

namespace WebApplicationMVCv2.Data;

public class AppPostgresContext : DbContext
{
    public AppPostgresContext(DbContextOptions<AppPostgresContext> options) : base(options)
    {
    }

    public DbSet<Lancamento> Lancamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lancamento>()
            .Property(l => l.Data)
            .HasColumnType("timestamp without time zone");
    }
}
