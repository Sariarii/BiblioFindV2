﻿using BiblioFind.Data.Models;
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
            return await context.Books
                .Where(b => b.Author.Name.ToLower().Contains(authorName.ToLower()) ||
                            b.Author.FirstName.ToLower().Contains(authorName.ToLower()))
                .ToListAsync();
        }

        // Lister les livres par rayon
        public async Task<IEnumerable<BookModel>> GetShelf(int shelfId)
        {
            return await context.Books
                .AsNoTracking()
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

        

        public async Task<IEnumerable<BookModel>> SearchBooksByTitleAsync(string title)
        {
            return await context.Books
                .Where(b => b.Title.ToLower().Contains(title.ToLower()))
                .ToListAsync();
        }
        public async Task<bool> AddBookWithAuthorAsync(BookModel book, AuthorModel author)
        {
            if (book == null || author == null)
            {
                return false; // Validation des paramètres
            }

            // Construire l'objet de la requête avec les propriétés spécifiques de BookModel
            var requestPayload = new
            {
                Title = book.Title,
                IsBorrowed = book.IsBorrowed,
                AuthorModelId = book.AuthorModelId,
                Author = new
                {
                    author.Name,
                    author.FirstName
                },
                ShelfModelId = book.ShelfModelId,
                MemberModelId = book.MemberModelId // Nullable, donc peut être null
            };

            try
            {
                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri("http://localhost:5253/api/")
                };

                // Envoyer la requête POST
                var response = await httpClient.PostAsJsonAsync("book/add", requestPayload);

                if (response.IsSuccessStatusCode)
                {
                    return true; // Succès
                }

                // Logique en cas d'échec
                Console.WriteLine($"Erreur lors de l'ajout : {response.StatusCode} - {response.ReasonPhrase}");
                return false;
            }
            catch (Exception ex)
            {
                // Gérer les erreurs de communication
                Console.WriteLine($"Exception lors de l'ajout : {ex.Message}");
                return false;
            }
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
