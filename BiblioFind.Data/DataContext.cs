using BiblioFind.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioFind.Data
{
    public class DataContext : DbContext 
    {
        public DbSet<AuthorModel> Author { get; set; }
        public DbSet<BookModel> Book { get; set; }

        public DbSet<ShelfModel> Shelf { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated(); 
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuthorModel>().HasData(
                new AuthorModel() { Id = 1, FirstName = "JK", Name = "Rowlling"},
                new AuthorModel() { Id = 2, FirstName = "JRR", Name = "Tolkien" },
                new AuthorModel() { Id = 3, FirstName = "RR", Name = "Martin" }
            );

            modelBuilder.Entity<BookModel>().HasData(
                new BookModel() { Id = 1, Title = "HP1", Status = false, AuthorModelId=1,ShelfModelId = 1 },
                new BookModel() { Id = 2, Title = "HP2", Status = false, AuthorModelId=1, ShelfModelId = 1 },
                new BookModel() { Id = 3, Title = "Seigneurs des anneaux", Status = false, AuthorModelId=2, ShelfModelId = 1 }
                );

            modelBuilder.Entity<ShelfModel>().HasData(
                new ShelfModel() { Id = 1 , Name = "Fantastique"}
            );
        }
    }
}
