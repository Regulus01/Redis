using Microsoft.EntityFrameworkCore;
using RedisAPI.Entities;

namespace RedisAPI.Infra;

public class ProductListDbContext : DbContext
{
    public ProductListDbContext(DbContextOptions<ProductListDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>()
            .HasKey(x => x.Id);
    }
}