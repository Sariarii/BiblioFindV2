using BiblioFind.Data.Repositories;
using System.Text;

namespace BiblioFind.Cmd
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IBookRepository bookRepository = new ApiBookRepository("http://localhost:5253");
            Console.WriteLine("Hello, World!");
            ListBookShelf(bookRepository);
            Console.ReadLine();


        }

        private static void ListBookShelf(IBookRepository bookRepository)
        {
            Console.WriteLine("Entrez l'id de la catégorie");
            if (int.TryParse(Console.ReadLine(), out int id))
            {

                var books = bookRepository.GetShelf(id).Result;
                if (books != null)
                {
                    StringBuilder message = new StringBuilder();
                    foreach (var book in books)
                    {
                        message.Append($"{book.Title}\n");
                    }
                    Console.WriteLine(message.ToString());
                }

                else
                {

                    Console.WriteLine("Erreur : aucune catégorie trouvée avec cet ID.");
                }
            }
            else
            {
                Console.WriteLine("Erreur : ID invalide.");


            }
        }
    }
}
