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
        Task<IEnumerable<BookModel>> GetShelf(int shelfId);
        Task<IEnumerable<BookModel>> GetBorrowedBooks();
        Task<BookModel?> Create(BookModel model);
        Task<BookModel> Update(int bookId, BookModel model);
        Task<IEnumerable<BookModel>> SearchBooksByTitleAsync(string title);
        Task<IEnumerable<BookModel>> Get();
        Task<BookModel> SearchBooksById(int id);
        Task<IEnumerable<BookModel>> GetBooksByAuthor(string authorName);
    }
}
