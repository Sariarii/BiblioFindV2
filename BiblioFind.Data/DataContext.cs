using BiblioFind.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioFind.Data
{
    public class DataContext : DbContext
    {
        public DbSet<AuthorModel> Authors { get; set; }
        public DbSet<BookModel> Books { get; set; }
        public DbSet<ShelfModel> Shelves { get; set; }
        public DbSet<MemberModel> Members { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Données des auteurs
            modelBuilder.Entity<AuthorModel>().HasData(
                new AuthorModel() { Id = 1, FirstName = "J.K.", Name = "Rowling" },
                new AuthorModel() { Id = 2, FirstName = "J.R.R.", Name = "Tolkien" },
                new AuthorModel() { Id = 3, FirstName = "George R.R.", Name = "Martin" },
                new AuthorModel() { Id = 4, FirstName = "Agatha", Name = "Christie" }
            );

            // Données des rayons
            modelBuilder.Entity<ShelfModel>().HasData(
                new ShelfModel() { Id = 1, Name = "Fantasy" },
                new ShelfModel() { Id = 2, Name = "Mystery" },
                new ShelfModel() { Id = 3, Name = "Science Fiction" },
                new ShelfModel() { Id = 4, Name = "Non-Fiction" }
            );

            // Données des livres
            modelBuilder.Entity<BookModel>().HasData(
                new BookModel() { Id = 1, Title = "Harry Potter and the Philosopher's Stone", IsBorrowed = false, AuthorModelId = 1, ShelfModelId = 1 },
                new BookModel() { Id = 2, Title = "Harry Potter and the Chamber of Secrets", IsBorrowed = false, AuthorModelId = 1, ShelfModelId = 1 },
                new BookModel() { Id = 3, Title = "The Lord of the Rings", IsBorrowed = false, AuthorModelId = 2, ShelfModelId = 1 },
                new BookModel() { Id = 4, Title = "A Game of Thrones", IsBorrowed = true, AuthorModelId = 3, ShelfModelId = 1, MemberModelId = 1 },
                new BookModel() { Id = 5, Title = "Murder on the Orient Express", IsBorrowed = false, AuthorModelId = 4, ShelfModelId = 2 },
                new BookModel() { Id = 6, Title = "And Then There Were None", IsBorrowed = true, AuthorModelId = 4, ShelfModelId = 2, MemberModelId = 2 },
                new BookModel() { Id = 7, Title = "Dune", IsBorrowed = false, AuthorModelId = 5, ShelfModelId = 3 }
            );

            // Données des membres
            modelBuilder.Entity<MemberModel>().HasData(
                new MemberModel() { Id = 1, FirstName = "John", LastName = "Doe" },
                new MemberModel() { Id = 2, FirstName = "Jane", LastName = "Smith" },
                new MemberModel() { Id = 3, FirstName = "Alice", LastName = "Brown" }
            );

        }
    }
}
