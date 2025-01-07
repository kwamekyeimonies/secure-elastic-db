using AcceralytDevTest.models;
using Microsoft.EntityFrameworkCore;

namespace AcceralytDevTest.datasources;

public class PostgreSqldbContext : DbContext
{
    public PostgreSqldbContext(DbContextOptions<PostgreSqldbContext> options): base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(x => x.Id);
        
        base.OnModelCreating(modelBuilder);
    }
    
}