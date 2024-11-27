using BiblioFind.Data.Models;
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
        [HttpGet("author")]
        public async Task<IEnumerable<BookModel>> GetBooksByAuthor([FromQuery] string authorName)
        {
            var booksByAuthor = await _bookRepository.GetBooksByAuthor(authorName);
            return booksByAuthor;
            //try
            //{
            //    var books = await _bookRepository.GetBooksByAuthor(authorName);
            //    if (books == null || !books.Any())
            //    {
            //        return NotFound(new { message = "Aucun livre trouvé pour cet auteur." });
            //    }
            //    return Ok(books);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, new { message = "Erreur interne du serveur.", details = ex.Message });
            //}
        }

        // Endpoint : Lister les livres par rayon
        [HttpGet("shelf")]
        public async Task<IEnumerable<BookModel>> GetBooksByShelf([FromQuery] int shelfId)
        {
            var booksByShelf = await _bookRepository.GetShelf(shelfId);
            return booksByShelf;
            //try
            //{
            //    // Vérification si l'ID du rayon est valide
            //    if (shelfId <= 0)
            //    {
            //        return BadRequest(new { message = "ID de rayon invalide." });
            //    }

            //    // Appel au repository pour récupérer les livres par rayon
            //    var books = await _bookRepository.GetShelf(shelfId);

            //    if (books == null || !books.Any())
            //    {
            //        return NotFound(new { message = "Aucun livre trouvé pour ce rayon." });
            //    }

            //    return Ok(books);
            //}
            //catch (Exception ex)
            //{
            //    // En cas d'erreur serveur, on renvoie un message d'erreur
            //    return StatusCode(500, new { message = "Erreur interne du serveur.", details = ex.Message });
            //}
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

        // BookController.cs

        [HttpGet]
        public async Task<IEnumerable<BookModel>> Get()
        {
            return await _bookRepository.Get();
        }

        [HttpGet("{bookId}")]
        public async Task<ActionResult<BookModel>> SearchBooksById(int bookId)
        {
            var model = await _bookRepository.SearchBooksById(bookId);
            if (model == null)
                return NotFound();
            return model;
        }

        [HttpPut("{bookId}")]
        public async Task<ActionResult<BookModel>> Update(int bookId, BookModel model)
        {
            var book = await _bookRepository.Update(bookId, model);
            if (book == null)
                return NotFound();
            return model;
        }


        //public async Task<IActionResult> AssignShelfToBook(int bookId, [FromBody] int shelfId)
        //{
        //    try
        //    {
        //        // Vérifier que l'ID du rayon est valide
        //        if (shelfId <= 0)
        //        {
        //            return BadRequest(new { message = "ID du rayon invalide." });
        //        }

        //        // Appeler la méthode dans le repository pour assigner le rayon au livre
        //        var result = await _bookRepository.AssignShelfToBookAsync(bookId, shelfId);

        //        // Si l'opération a échoué (livre ou rayon introuvable), renvoyer un message d'erreur
        //        if (!result)
        //        {
        //            return NotFound(new { message = "Livre ou rayon non trouvé." });
        //        }

        //        // Sinon, renvoyer une réponse positive
        //        return Ok(new { message = "Rayon assigné au livre avec succès." });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Si une exception se produit, renvoyer un message d'erreur
        //        return StatusCode(500, new { message = "Erreur interne du serveur.", details = ex.Message });
        //    }
        //}
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

        [HttpPost("add")]
        public async Task<IActionResult> AddBookWithAuthor([FromBody] AddBookRequest request)
        {
            if (request == null || request.Book == null || request.Author == null)
            {
                return BadRequest("Données invalides.");
            }

            var result = await _bookRepository.AddBookWithAuthorAsync(request.Book, request.Author);
            if (result)
            {
                return Ok("Le livre et l'auteur ont été ajoutés avec succès.");
            }
            return StatusCode(500, "Erreur lors de l'ajout du livre et de l'auteur.");
        }

        // Classe pour la requête
        public class AddBookRequest
        {
            public BookModel Book { get; set; }
            public AuthorModel Author { get; set; }
        }



    }
}
