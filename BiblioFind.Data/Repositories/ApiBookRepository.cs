using BiblioFind.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BiblioFind.Data.Repositories
{
    public class ApiBookRepository : IBookRepository
    {
        private readonly string url;
        private readonly HttpClient client;
        public ApiBookRepository(string url)
        {
            this.url = url;
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
        }

        // Lister les livres par auteur
        public async Task<IEnumerable<BookModel>> GetBooksByAuthor(string authorName)
        {
            throw new NotImplementedException();
            //return await context.Books
            //    .Where(b => b.Author.Name.ToLower().Contains(authorName.ToLower()) ||
            //                b.Author.FirstName.ToLower().Contains(authorName.ToLower()))
            //    .ToListAsync();
        }

        // Lister les livres par rayon
        public async Task<IEnumerable<BookModel>> GetShelf(int shelfId)
        {
            var client = new HttpClient();
            var response = await client.GetAsync($"{url}/book/shelf?shelfId={shelfId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<BookModel>>();

            }
            else
            {
                return null;
            }
        }

        // Lister les livres empruntés
        public async Task<IEnumerable<BookModel>> GetBorrowedBooks()
        {
            throw new NotImplementedException();
            //return await context.Books
            //    .Where(b => b.IsBorrowed)
            //    .Include(b => b.Member)
            //    .ToListAsync();
        }

        

        public async Task<IEnumerable<BookModel>> SearchBooksByTitleAsync(string title)
        {
            throw new NotImplementedException();
            //return await context.Books
            //    .Where(b => b.Title.ToLower().Contains(title.ToLower()))
            //    .ToListAsync();
        }

        public async Task<BookModel?> Create(BookModel model)
        {
            var response = await client.PostAsJsonAsync($"{url}/book", model);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<BookModel?>();
            return null;
        }


        public async Task<BookModel> Update(int bookId, BookModel model)
        {
            var response = await client.PutAsJsonAsync($"{url}/book/{bookId}", model);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<BookModel?>();
            return null;
        }

        public async Task<IEnumerable<BookModel>> Get()
        {
            var response = await client.GetAsync($"{url}/book");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<BookModel>>();

            }
            return null;
        }

        public async Task<BookModel> SearchBooksById(int bookId)
        {
            var response = await client.GetAsync($"{url}/book/{bookId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<BookModel>();
            }
            return null;
        }
    }
}
