using LibraryExamApp.Data;
using LibraryExamApp.Models.Dtos;
using LibraryExamApp.Models.Entities;
using LibraryExamApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryExamApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LibraryApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILibraryQueryService _queries;

        public LibraryApiController(AppDbContext db, ILibraryQueryService queries)
        {
            _db = db;
            _queries = queries;
        }

        // 1. GET: api/LibraryApi/books?minPrice=10 (Izmanto 1. LINQ vaicājumu)
        [HttpGet("books")]
        public async Task<ActionResult<List<BookFilterDto>>> GetBooks([FromQuery] decimal minPrice = 0)
        {
            var books = await _queries.GetBooksFilteredAndSortedAsync(minPrice);
            return Ok(books); // 200 OK
        }

        // 2. GET: api/LibraryApi/books/{id} (Pēc ID ar DTO)
        [HttpGet("books/{id:int}")]
        public async Task<ActionResult<BookResponseDto>> GetBookById(int id)
        {
            var b = await _db.Books
                .Include(x => x.Author)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (b == null) return NotFound(new { message = $"Grāmata ar ID {id} nav atrasta." }); // 404

            return Ok(new BookResponseDto(b.Id, b.Title, b.ISBN, b.Price, b.PublishYear, b.Author!.FullName, b.Category!.Title));
        }

        // 3. GET: api/LibraryApi/categories/stats (Izmanto 3. LINQ vaicājumu)
        [HttpGet("categories/stats")]
        public async Task<ActionResult<List<CategoryGroupStatsDto>>> GetCategoryStats()
        {
            var stats = await _queries.GetCategoryStatsAsync();
            return Ok(stats);
        }

        // 4. POST: api/LibraryApi/books (Izveido jaunu grāmatu)
        [HttpPost("books")]
        public async Task<ActionResult<BookResponseDto>> CreateBook([FromBody] CreateBookDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // 400

            var book = new Book
            {
                Title = dto.Title,
                ISBN = dto.ISBN,
                Price = dto.Price,
                PublishYear = dto.PublishYear,
                AuthorId = dto.AuthorId,
                CategoryId = dto.CategoryId
            };

            _db.Books.Add(book);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book); // 201 Created
        }

        // 5. PUT: api/LibraryApi/loans/{id}/status (Atjauno aizņēmuma statusu)
        [HttpPut("loans/{id:int}/status")]
        public async Task<IActionResult> UpdateLoanStatus(int id, [FromBody] UpdateLoanStatusDto dto)
        {
            var loan = await _db.BookLoans.FindAsync(id);
            if (loan == null) return NotFound(new { message = "Aizņēmums nav atrasts" });

            loan.IsReturned = dto.IsReturned;
            loan.FineAmount = dto.FineAmount;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Statuss atjaunots!", loanId = id, loan.IsReturned });
        }
    }
}