using BiblioFind.Data.Repositories;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BiblioFind.Cmd
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
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
                Console.WriteLine("7. Quitter");
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
                        await AssignShelfToBook();
                        break;
                    case "7":
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
            var response = await client.GetAsync($"http://localhost:5253/api/book/author/{authorName}");

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
            var response = await client.GetAsync($"http://localhost:5253/api/book/shelf/{shelfId}");

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

        private static async Task AssignShelfToBook()
        {
            Console.Write("Entrez l'ID du livre : ");
            int bookId = int.Parse(Console.ReadLine() ?? "0");
            Console.Write("Entrez l'ID du rayon : ");
            int shelfId = int.Parse(Console.ReadLine() ?? "0");

            using var client = new HttpClient();
            var response = await client.PostAsync($"http://localhost:5253/api/book/assignshelf/{bookId}/{shelfId}", null);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Le rayon a été assigné au livre avec succès.");
            }
            else
            {
                Console.WriteLine("Erreur lors de l'assignation du rayon.");
            }

            Console.ReadKey();
        }
    }
}
