using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<AppUser> Users { get; set; }
}