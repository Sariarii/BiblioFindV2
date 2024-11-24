using BiblioFind.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BiblioFind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // Endpoint : Lister les livres par auteur
        [HttpGet("author/{authorName}")]
        public async Task<IActionResult> GetBooksByAuthor(string authorName)
        {
            try
            {
                var books = await _bookRepository.GetBooksByAuthor(authorName);
                if (books == null || !books.Any())
                {
                    return NotFound(new { message = "Aucun livre trouvé pour cet auteur." });
                }
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur interne du serveur.", details = ex.Message });
            }
        }

        // Endpoint : Lister les livres par rayon
        [HttpGet("shelf/{shelfId}")]
        public async Task<IActionResult> GetBooksByShelf(int shelfId)
        {
            try
            {
                // Vérification si l'ID du rayon est valide
                if (shelfId <= 0)
                {
                    return BadRequest(new { message = "ID de rayon invalide." });
                }

                // Appel au repository pour récupérer les livres par rayon
                var books = await _bookRepository.GetShelf(shelfId);

                if (books == null || !books.Any())
                {
                    return NotFound(new { message = "Aucun livre trouvé pour ce rayon." });
                }

                return Ok(books);
            }
            catch (Exception ex)
            {
                // En cas d'erreur serveur, on renvoie un message d'erreur
                return StatusCode(500, new { message = "Erreur interne du serveur.", details = ex.Message });
            }
        }


        // Endpoint : Lister les livres empruntés
        [HttpGet("borrowed")]
        public async Task<IActionResult> GetBorrowedBooks()
        {
            try
            {
                var books = await _bookRepository.GetBorrowedBooks();
                if (books == null || !books.Any())
                {
                    return NotFound(new { message = "Aucun livre emprunté trouvé." });
                }
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur interne du serveur.", details = ex.Message });
            }
        }

        // Endpoint : Emprunter un livre
        [HttpPost("borrow/{bookId}/{memberId}")]
        public async Task<IActionResult> BorrowBook(int bookId, int memberId)
        {
            try
            {
                bool success = await _bookRepository.BorrowBook(bookId, memberId);
                if (!success)
                {
                    return NotFound(new { message = "Le livre est déjà emprunté ou n'existe pas." });
                }
                return Ok(new { message = "Le livre a été emprunté avec succès." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur interne du serveur.", details = ex.Message });
            }
        }

        // Endpoint : Rendre un livre
        [HttpPost("return/{bookId}")]
        public async Task<IActionResult> ReturnBook(int bookId)
        {
            try
            {
                bool success = await _bookRepository.ReturnBook(bookId);
                if (!success)
                {
                    return NotFound(new { message = "Le livre n'est pas emprunté." });
                }
                return Ok(new { message = "Le livre a été rendu avec succès." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur interne du serveur.", details = ex.Message });
            }
        }

        // BookController.cs

        [HttpPut("assignshelf/{bookId}")]
        public async Task<IActionResult> AssignShelfToBook(int bookId, [FromBody] int shelfId)
        {
            try
            {
                // Vérifier que l'ID du rayon est valide
                if (shelfId <= 0)
                {
                    return BadRequest(new { message = "ID du rayon invalide." });
                }

                // Appeler la méthode dans le repository pour assigner le rayon au livre
                var result = await _bookRepository.AssignShelfToBookAsync(bookId, shelfId);

                // Si l'opération a échoué (livre ou rayon introuvable), renvoyer un message d'erreur
                if (!result)
                {
                    return NotFound(new { message = "Livre ou rayon non trouvé." });
                }

                // Sinon, renvoyer une réponse positive
                return Ok(new { message = "Rayon assigné au livre avec succès." });
            }
            catch (Exception ex)
            {
                // Si une exception se produit, renvoyer un message d'erreur
                return StatusCode(500, new { message = "Erreur interne du serveur.", details = ex.Message });
            }
        }
        [HttpGet("search/{title}")]
        public async Task<IActionResult> SearchBooksByTitle(string title)
        {
            var books = await _bookRepository.SearchBooksByTitleAsync(title);
            if (books == null || !books.Any())
            {
                return NotFound("Aucun livre trouvé avec ce titre.");
            }

            return Ok(books);
        }

    }
}
