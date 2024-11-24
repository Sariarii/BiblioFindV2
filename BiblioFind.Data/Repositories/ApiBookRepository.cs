using BiblioFind.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiblioFind.Data.Repositories
{
    public class ApiBookRepository : IBookRepository
    {
        private readonly DataContext context;

        public ApiBookRepository(DataContext context)
        {
            this.context = context;
        }

        // Lister les livres par auteur
        public async Task<IEnumerable<BookModel>> GetBooksByAuthor(string authorName)
        {
            return await context.Books
                .Where(b => b.Author.Name.Contains(authorName) || b.Author.FirstName.Contains(authorName))
                .ToListAsync();
        }

        // Lister les livres par rayon
        public async Task<IEnumerable<BookModel>> GetShelf(int shelfId)
        {
            return await context.Books
                .Where(b => b.ShelfModelId == shelfId)
                .ToListAsync();
        }

        // Lister les livres empruntés
        public async Task<IEnumerable<BookModel>> GetBorrowedBooks()
        {
            return await context.Books
                .Where(b => b.IsBorrowed)
                .Include(b => b.Member)
                .ToListAsync();
        }

        // Implémentation de la méthode BorrowBook
        public async Task<bool> BorrowBook(int bookId, int memberId)
        {
            var book = await context.Books.FindAsync(bookId);
            if (book == null || book.IsBorrowed)
                return false;  // Le livre n'existe pas ou est déjà emprunté

            // Marquer le livre comme emprunté
            book.IsBorrowed = true;
            book.MemberModelId = memberId;

            // Sauvegarder les changements dans la base de données
            await context.SaveChangesAsync();
            return true;
        }

        // Implémentation de la méthode ReturnBook
        public async Task<bool> ReturnBook(int bookId)
        {
            var book = await context.Books.FindAsync(bookId);
            if (book == null || !book.IsBorrowed)
                return false;  // Le livre n'existe pas ou n'est pas emprunté

            // Marquer le livre comme retourné
            book.IsBorrowed = false;
            book.MemberModelId = null;  // Réinitialiser l'association du membre

            // Sauvegarder les changements dans la base de données
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignShelfToBookAsync(int bookId, int shelfId)
        {
            // Trouver le livre par son ID
            var book = await context.Books.FindAsync(bookId);
            if (book == null)
            {
                return false;  // Le livre n'existe pas
            }

            // Trouver le rayon par son ID
            var shelf = await context.Shelves.FindAsync(shelfId);
            if (shelf == null)
            {
                return false;  // Le rayon n'existe pas
            }

            // Assigner le rayon au livre
            book.ShelfModelId = shelfId;

            // Sauvegarder les changements dans la base de données
            await context.SaveChangesAsync();
            return true;  // Retourner true si l'assignation a réussi
        }
        public async Task<IEnumerable<BookModel>> SearchBooksByTitleAsync(string title)
        {
            return await context.Books
                .Where(b => b.Title.ToLower().Contains(title.ToLower()))
                .ToListAsync();
        }
    }
}
