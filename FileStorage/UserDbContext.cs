using Microsoft.EntityFrameworkCore;

public class UserDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=filestoragedb;Username=stas;Password=kbn");
    }
}
