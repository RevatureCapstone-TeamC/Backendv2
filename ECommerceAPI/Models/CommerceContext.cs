using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ECommerce.Models;
public class CommerceContext : DbContext 
{
    public CommerceContext(DbContextOptions<CommerceContext> options) : base(options)
    {}

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Wishlist> Wishlist { get; set; } = null!;
    public DbSet<Deal> Deals { get; set; } = null!;
    public DbSet<Cart> Cart { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelbuilder){
        modelbuilder.Entity<Product>(entity =>
            { entity.ToTable("Products", "ecd") ;} );
        modelbuilder.Entity<User>(entity => 
            { entity.ToTable("Users", "ecd") ; });
        modelbuilder.Entity<Wishlist>(entity =>
            { entity.ToTable("Wishlist", "ecd") ;} );
        modelbuilder.Entity<Deal>(entity => 
            { entity.ToTable("Deals", "ecd") ; });
        modelbuilder.Entity<Cart>(entity => 
            { entity.ToTable("Cart", "ecd"); });
    }
}