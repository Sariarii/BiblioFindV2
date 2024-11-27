using BiblioFind.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiblioFind.Data.Repositories
{
    public class SqlBookRepository : IBookRepository
    {
        private readonly DataContext context;

        public SqlBookRepository(DataContext context)
        {
            this.context = context;
        }

        // Lister les livres par auteur
        public async Task<IEnumerable<BookModel>> GetBooksByAuthor(string authorName)
        {
            return await context.Books
                .AsNoTracking()
                .Where(b => b.Author.Name.ToLower().Contains(authorName.ToLower()) ||
                            b.Author.FirstName.ToLower().Contains(authorName.ToLower()))
                .ToListAsync();
        }

        // Lister les livres par rayon
        public async Task<IEnumerable<BookModel>> GetShelf(int shelfId)
        {
            return await context.Books
                .AsNoTracking()
                .Where(b => b.ShelfModelId == shelfId)  // Filtrer les livres par rayon
                .ToListAsync();
        }

        // Lister les livres empruntés
        public async Task<IEnumerable<BookModel>> GetBorrowedBooks()
        {
            return await context.Books
                .AsNoTracking()
                .Where(b => b.IsBorrowed == true)  // Filtrer les livres par rayon
                .ToListAsync();
        }

        // Emprunter un livre
        public async Task<bool> AssignShelfToBookAsync(int bookId, int shelfId)
        {
            // Trouver le livre par son ID
            var book = await context.Books.FindAsync(bookId);

            // Si le livre n'existe pas, retourner false
            if (book == null)
            {
                return false;  // Le livre n'existe pas
            }

            // Trouver le rayon par son ID
            var shelf = await context.Shelves.FindAsync(shelfId);

            // Si le rayon n'existe pas, retourner false
            if (shelf == null)
            {
                return false;  // Le rayon n'existe pas
            }

            // Assigner le rayon au livre
            book.ShelfModelId = shelfId;

            // Sauvegarder les changements dans la base de données
            await context.SaveChangesAsync();

            return true;  // Retourner true si tout s'est bien passé
        }

        public async Task<IEnumerable<BookModel>> SearchBooksByTitleAsync(string title)
        {
            return await context.Books
                .AsNoTracking()
                .Where(b => EF.Functions.Like(b.Title.ToLower(), $"%{title.ToLower()}%"))
                .ToListAsync();
        }
        public async Task<IEnumerable<BookModel>> Get()
        {

            return await context.Books.ToListAsync();
        }
        public async Task<BookModel?> SearchBooksById(int bookId)
        {
            return await context.Books.FirstOrDefaultAsync(b => b.Id == bookId);
        }


        public async Task<bool> AddBookWithAuthorAsync(BookModel book, AuthorModel author)
        {
            if (book == null || author == null)
            {
                return false; // Valider les entrées
            }

            // Vérifiez si l'auteur existe déjà
            var existingAuthor = await context.Authors
                .FirstOrDefaultAsync(a => a.Name.ToLower() == author.Name.ToLower() && a.FirstName.ToLower() == author.FirstName.ToLower());

            if (existingAuthor != null)
            {
                // Associez l'auteur existant au livre
                book.AuthorModelId = existingAuthor.Id;
            }
            else
            {
                // Ajoutez le nouvel auteur à la base de données
                context.Authors.Add(author);
                await context.SaveChangesAsync();

                // Associez l'auteur nouvellement créé au livre
                book.AuthorModelId = author.Id;
            }

            // Ajoutez le livre à la base de données
            context.Books.Add(book);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<BookModel> Update(int bookId, BookModel model)
        {
            var entity = await context.Set<BookModel>().FirstOrDefaultAsync(x => x.Id == bookId);
            if (entity == null)
                return null;
            context.Entry(entity).CurrentValues.SetValues(model);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<BookModel?> Create(BookModel model)
        {
            context.Set<BookModel>().Add(model); //Rend le repository generique
            await context.SaveChangesAsync(); //Execute la requete du ADD
            return model;
        }
    }
}
