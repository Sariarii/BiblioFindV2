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
          
        }

        // Endpoint : Lister les livres par rayon
        [HttpGet("shelf")]
        public async Task<IEnumerable<BookModel>> GetBooksByShelf([FromQuery] int shelfId)
        {
            var booksByShelf = await _bookRepository.GetShelf(shelfId);
            return booksByShelf;
            
        }


        // Endpoint : Lister les livres empruntés
        [HttpGet("borrowed")]
        public async Task<IActionResult> GetBorrowedBooks([FromQuery] bool IsBorrowed)
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

        [HttpPost]
        public async Task<ActionResult<BookModel>> Create(BookModel model)
        {
            var book = await _bookRepository.Create(model);
            if (book == null)
                return StatusCode(500);
            return model;
        }

        // Classe pour la requête
        public class AddBookRequest
        {
            public BookModel Book { get; set; }
            public AuthorModel Author { get; set; }
        }



    }
}
