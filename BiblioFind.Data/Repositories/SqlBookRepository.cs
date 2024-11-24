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
                .Include(b => b.Author)
                .Where(b => b.Author.Name.ToLower().Contains(authorName.ToLower()) ||
                            b.Author.FirstName.ToLower().Contains(authorName.ToLower()))
                .ToListAsync();
        }

        // Lister les livres par rayon
        public async Task<IEnumerable<BookModel>> GetShelf(int shelfId)
        {
            return await context.Books
                .AsNoTracking()
                .Include(b => b.Shelf)  // Inclure le modèle Shelf
                .Where(b => b.ShelfModelId == shelfId)  // Filtrer les livres par rayon
                .ToListAsync();
        }

        // Lister les livres empruntés
        public async Task<IEnumerable<BookModel>> GetBorrowedBooks()
        {
            return await context.Books
                .Include(b => b.Member)  // Inclure les informations sur le membre emprunteur
                .Where(b => b.IsBorrowed == true)  // Filtrer les livres empruntés
                .ToListAsync();
        }

        // Emprunter un livre
        public async Task<bool> BorrowBook(int bookId, int memberId)
        {
            // Logique pour marquer un livre comme emprunté
            var book = await context.Books.FindAsync(bookId);
            if (book == null || book.IsBorrowed) return false;  // Le livre est déjà emprunté ou n'existe pas

            book.IsBorrowed = true;
            book.MemberModelId = memberId;  // Assigner le membre
            await context.SaveChangesAsync();
            return true;
        }

        // Implémentation de la méthode ReturnBook
        public async Task<bool> ReturnBook(int bookId)
        {
            // Logique pour marquer un livre comme retourné
            var book = await context.Books.FindAsync(bookId);
            if (book == null || !book.IsBorrowed) return false;  // Le livre n'est pas emprunté ou n'existe pas

            book.IsBorrowed = false;
            book.MemberModelId = null;  // Réinitialiser l'assignation du membre
            await context.SaveChangesAsync();
            return true;
        }
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
    }
}
