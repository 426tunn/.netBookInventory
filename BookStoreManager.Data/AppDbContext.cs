using BookStoreManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStoreManager.Data
{
    public class AppDbContext : DbContext
    {
    //   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //   {
    //     //   optionsBuilder.UseNpgsql("BookStoreManagerDb");
    //         optionsBuilder.UseSqlServer("BookStoreManagerDb");
    //   }
    //   public AppDbContext(){}

     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
     {
         
     }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
      base.OnModelCreating(modelBuilder);

          modelBuilder.Entity<Book>()
        .Property(b => b.Price)
        .HasColumnType("decimal(18,2)");
     }
        public DbSet<Author>? Authors { get; set; }
        public DbSet<Book>? Books { get; set; }

    }
}

//Delete Database and Tables