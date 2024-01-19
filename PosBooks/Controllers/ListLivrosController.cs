using Microsoft.AspNetCore.Mvc;
using PosBooks.Services;
using PosBooksCore.Models;

namespace PosBooks.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ListLivrosController : ControllerBase
    {
        private readonly IBookService _bookService;

        public ListLivrosController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            var books = await _bookService.ListBooks();

            return Ok(books);
        }
    }
}
