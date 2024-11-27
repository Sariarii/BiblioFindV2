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
                        await ListBooksByShelf();
                        break;
                    case "4":
                        await BorrowBook();
                        break;
                    case "5":
                        await ReturnBook();
                        break;
                    case "6":
                        await Get(repository);
                        await AssignShelfToBook(repository);
                        break;
                    case "7":
                        await SearchBookByTitle();
                        break;
                    case "8":
                        await AddBookWithAuthor();
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
            var response = await client.GetAsync("http://localhost:5253/api/book/borrowed");

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

            using var client = new HttpClient();
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

        private static async Task ListBooksByShelf()
        {
            Console.Write("Entrez l'ID du rayon : ");
            int shelfId = int.Parse(Console.ReadLine() ?? "0");

            using var client = new HttpClient();
            var response = await client.GetAsync($"http://localhost:5253/api/book/shelf?shelfId={shelfId}");

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

        private static async Task BorrowBook()
        {
            Console.Write("Entrez l'ID du livre à emprunter : ");
            int bookId = int.Parse(Console.ReadLine() ?? "0");
            Console.Write("Entrez l'ID du membre : ");
            int memberId = int.Parse(Console.ReadLine() ?? "0");

            using var client = new HttpClient();
            var response = await client.PostAsync($"http://localhost:5253/api/book/borrow/{bookId}/{memberId}", null);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Le livre a été emprunté avec succès.");
            }
            else
            {
                Console.WriteLine("Erreur lors de l'emprunt du livre.");
            }

            Console.ReadKey();
        }

        private static async Task ReturnBook()
        {
            Console.Write("Entrez l'ID du livre à rendre : ");
            int bookId = int.Parse(Console.ReadLine() ?? "0");

            using var client = new HttpClient();
            var response = await client.PostAsync($"http://localhost:5253/api/book/return/{bookId}", null);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Le livre a été rendu avec succès.");
            }
            else
            {
                Console.WriteLine("Erreur lors du retour du livre.");
            }

            Console.ReadKey();
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
                message.Append($"{item.Id} {item.Title} {item.Author}\n");
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
                result = repository.AssignShelfToBookAsync(id, result).Result;
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

            using var client = new HttpClient();
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

        private static async Task AddBookWithAuthor()
        {
            Console.Write("Entrez le titre du livre : ");
            string title = Console.ReadLine();

            Console.Write("Le livre est-il emprunté ? (oui/non) : ");
            bool isBorrowed = Console.ReadLine()?.Trim().ToLower() == "oui";

            Console.Write("Entrez l'ID du rayon : ");
            int shelfId = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Entrez le nom de l'auteur : ");
            string authorName = Console.ReadLine();

            Console.Write("Entrez le prénom de l'auteur : ");
            string authorFirstName = Console.ReadLine();

            // Construire les modèles pour le livre et l'auteur
            var book = new
            {
                Title = title,
                IsBorrowed = isBorrowed,
                ShelfModelId = shelfId
            };

            var author = new
            {
                Name = authorName,
                FirstName = authorFirstName
            };

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:5253/api/");

                var payload = new
                {
                    Book = book,
                    Author = author
                };

                // Envoyer la requête POST
                var response = await client.PostAsJsonAsync("book/add", payload);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Le livre avec son auteur a été ajouté avec succès.");
                }
                else
                {
                    Console.WriteLine($"Erreur lors de l'ajout : {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception lors de l'ajout : {ex.Message}");
            }

            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }


    }
}
