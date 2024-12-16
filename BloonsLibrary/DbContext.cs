using System;
using Microsoft.EntityFrameworkCore;

public class GameDbContext : DbContext
{
    public GameDbContext() { }

    public GameDbContext(DbContextOptions<GameDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = "Server=192.168.0.104;Database=bloonsdb;User=austeja;Password=pass;";

            var serverVersion = new MySqlServerVersion(new Version(10, 4, 32));

            optionsBuilder.UseMySql(connectionString, serverVersion);
        }
    }
}
