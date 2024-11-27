using BiblioFind.Data.Models;
using BiblioFind.Data.Repositories;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BiblioFind.Cmd
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IBookRepository repository = new ApiBookRepository("http://localhost:5253/api");
            while (true)
            {
                Console.Clear();
                Console.WriteLine("====================================");
                Console.WriteLine("       Gestion de la Bibliothèque   ");
                Console.WriteLine("====================================");
                Console.WriteLine("1. Lister les livres empruntés");
                Console.WriteLine("2. Lister les livres par auteur");
                Console.WriteLine("3. Lister les livres par rayon");
                Console.WriteLine("4. Emprunter un livre");
                Console.WriteLine("5. Rendre un livre");
                Console.WriteLine("6. Assigner un rayon à un livre");
                Console.WriteLine("7. Rechercher un livre par titre");
                Console.WriteLine("8. Ajouter un livre avec son auteur");
                Console.WriteLine("9. Quitter");
                Console.Write("Sélectionnez une option : ");

                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        await ListBorrowedBooks();
                        break;
                    case "2":
                        await ListBooksByAuthor();
                        break;
                    case "3":
                        await ListBooksByShelf(repository);
                        break;
                    case "4":
                        await Get(repository);
                        await BorrowBook(repository);
                        break;
                    case "5":
                        await Get(repository);
                        await ReturnBook(repository);
                        break;
                    case "6":
                        await Get(repository);
                        await AssignShelfToBook(repository);
                        break;
                    case "7":
                        await SearchBookByTitle();
                        break;
                    case "8":
                        await AddBookWithAuthor(repository);
                        break;
                    case "9":
                        Console.WriteLine("Au revoir !");
                        return;
                    default:
                        Console.WriteLine("Option invalide. Appuyez sur une touche pour réessayer.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static async Task ListBorrowedBooks()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("http://localhost:5253/api/book/borrowed?IsBorrowed=true");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            else
            {
                Console.WriteLine("Erreur de récupération des livres empruntés.");
            }

            Console.ReadKey();
        }

        private static async Task ListBooksByAuthor()
        {
            Console.Write("Entrez le nom de l'auteur : ");
            string authorName = Console.ReadLine();

            var client = new HttpClient();
            var response = await client.GetAsync($"http://localhost:5253/api/book/author?authorName={authorName}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            else
            {
                Console.WriteLine("Erreur de récupération des livres.");
            }

            Console.ReadKey();
        }

        private static async Task ListBooksByShelf(IBookRepository repository)
        {
            Console.Write("Entrez l'ID du rayon : ");
            int shelfId = int.Parse(Console.ReadLine() ?? "0");
            var result = repository.GetShelf(shelfId).Result;
            if (result == null)
            {
                Console.WriteLine("Error");
                return;
            }
            StringBuilder message = new StringBuilder();
            foreach (var item in result)
            {
                message.Append($"{item.Title} {item.IsBorrowed}\n");
            }
            Console.WriteLine(message.ToString());

            Console.ReadKey();

        }

        private static async Task BorrowBook(IBookRepository repository)
        {

            Console.WriteLine("Quel livre voulez vous emprunter ?");
            int id;
            string input = Console.ReadLine();
            int.TryParse(input, out id);
            var result = repository.SearchBooksById(id).Result;
            Console.WriteLine($"Est ce bien ce livre ? : {result.Title} (Oui/Non)");
            string Response = Console.ReadLine();
            if (Response == "Oui")
            {
                result.IsBorrowed = true;
                result = repository.Update(id, result).Result;
            }
            else if (Response == "Non")
            {

                return;
            }
            else
            {
                Console.WriteLine("Reponse non valide");
                return;
            }
        }

        private static async Task ReturnBook(IBookRepository repository)
        {
            Console.WriteLine("Quel livre voulez vous rendre ?");
            int id;
            string input = Console.ReadLine();
            int.TryParse(input, out id);
            var result = repository.SearchBooksById(id).Result;
            Console.WriteLine($"Est ce bien ce livre ? : {result.Title} (Oui/Non)");
            string Response = Console.ReadLine();
            if (Response == "Oui")
            {
                result.IsBorrowed = false;
                result = repository.Update(id, result).Result;
            }
            else if (Response == "Non")
            {

                return;
            }
            else
            {
                Console.WriteLine("Reponse non valide");
                return;
            }
        }

        private static async Task Get(IBookRepository repository)
        {
            var models = repository.Get().Result;
            if (models == null)
            {
                Console.WriteLine("Error API");
                return;
            }
            StringBuilder message = new StringBuilder();
            foreach (var item in models)
            {
                message.Append($"{item.Id} {item.Title} {item.IsBorrowed}\n");
            }
            Console.WriteLine(message.ToString());

        }

        private static async Task AssignShelfToBook(IBookRepository repository)
        {

            Console.WriteLine("Donnez le numero du livre que vous voulez assigner a une étagère");
            int id;
            string input = Console.ReadLine();
            int.TryParse(input, out id);
            var result = repository.SearchBooksById(id).Result;
            Console.WriteLine($"Est ce bien ce livre ? : {result.Title} (Oui/Non)");
            string Response = Console.ReadLine();
            if (Response == "Oui")
            {
                Console.WriteLine("Chosissez l'Id de la catégorie");
                int idShelf;
                input = Console.ReadLine();
                int.TryParse(input, out idShelf);
                result.ShelfModelId = idShelf;
                Console.WriteLine($"{result.ShelfModelId}");
                result = repository.Update(id, result).Result;
            }
            else if (Response == "Non")
            {

                return;
            }
            else
            {
                Console.WriteLine("Reponse non valide");
                return;
            }
        }

        private static async Task SearchBookByTitle()
        {
            Console.Write("Entrez le titre ou une partie du titre du livre : ");
            string title = Console.ReadLine();

            var client = new HttpClient();
            var response = await client.GetAsync($"http://localhost:5253/api/book/search/{title}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            else
            {
                Console.WriteLine("Erreur lors de la recherche des livres.");
            }

            Console.ReadKey();
        }

        private static async Task AddBookWithAuthor(IBookRepository repository)
        {
            int idAuthor;
            int idShelf;
            Console.Write("Entrez le titre du livre : ");
            string title = Console.ReadLine();

            Console.Write("Le livre est-il emprunté ? (oui/non) : ");
            bool isBorrowed = Console.ReadLine()?.Trim().ToLower() == "oui";
            Console.Write("Rentrez l'id de l'auteur ");
            string input = Console.ReadLine();
            int.TryParse(input, out idAuthor);
            Console.Write("Rentrez l'id de la catégorie ");
            input = Console.ReadLine();
            int.TryParse(input, out idShelf);


            var book = new BookModel() { Title = title, IsBorrowed = isBorrowed, AuthorModelId= idAuthor, ShelfModelId= idShelf };
            book = repository.Create(book).Result;
            if (book == null)
                Console.WriteLine("Raté");
            else
                Console.WriteLine($"Livre Crée: {book.Title} {book.IsBorrowed}");
        }
    }
}
