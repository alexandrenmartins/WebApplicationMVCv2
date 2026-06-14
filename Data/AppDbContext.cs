using Microsoft.EntityFrameworkCore;
using WebApplicationMVCv2.Models;

namespace WebApplicationMVCv2.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Lancamento> Lancamentos { get; set; }
}
