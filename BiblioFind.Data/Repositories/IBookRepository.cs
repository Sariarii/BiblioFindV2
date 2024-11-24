using BiblioFind.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioFind.Data.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookModel>> GetBooksByAuthor(string authorName);
        Task<IEnumerable<BookModel>> GetShelf(int shelfId);
        Task<IEnumerable<BookModel>> GetBorrowedBooks();
        Task<bool> BorrowBook(int bookId, int memberId);
        Task<bool> ReturnBook(int bookId);
        Task<bool> AssignShelfToBookAsync(int bookId, int shelfId);
        Task<IEnumerable<BookModel>> SearchBooksByTitleAsync(string title);
    }
}
