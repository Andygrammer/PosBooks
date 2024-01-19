using Microsoft.AspNetCore.Mvc;
using PosBooks.Services;
using PosBooksCore.Models;

namespace PosBooks.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class GetLivroController : ControllerBase
    {
        private readonly IBookService _bookService;

        public GetLivroController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Book>> GetBook([FromRoute] int id)
        {
            var book = await _bookService.GetBook(id);

            if (book == null) return NotFound();

            return Ok(book);
        }
    }
}
