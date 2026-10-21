using LibraryExamApp.Data;
using LibraryExamApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibraryExamApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _db;
        public BooksController(AppDbContext db) => _db = db;

        // READ (Index)
        public async Task<IActionResult> Index()
        {
            var books = await _db.Books.Include(b => b.Author).Include(b => b.Category).ToListAsync();
            return View(books);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(_db.Authors, "Id", "FullName");
            ViewBag.CategoryId = new SelectList(_db.Categories, "Id", "Title");
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _db.Books.Add(book);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AuthorId = new SelectList(_db.Authors, "Id", "FullName", book.AuthorId);
            ViewBag.CategoryId = new SelectList(_db.Categories, "Id", "Title", book.CategoryId);
            return View(book);
        }

        // EDIT (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var book = await _db.Books.FindAsync(id);
            if (book == null) return NotFound();

            ViewBag.AuthorId = new SelectList(_db.Authors, "Id", "FullName", book.AuthorId);
            ViewBag.CategoryId = new SelectList(_db.Categories, "Id", "Title", book.CategoryId);
            return View(book);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _db.Update(book);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AuthorId = new SelectList(_db.Authors, "Id", "FullName", book.AuthorId);
            ViewBag.CategoryId = new SelectList(_db.Categories, "Id", "Title", book.CategoryId);
            return View(book);
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var book = await _db.Books.Include(b => b.Author).Include(b => b.Category).FirstOrDefaultAsync(m => m.Id == id);
            if (book == null) return NotFound();
            return View(book);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _db.Books.FindAsync(id);
            if (book != null)
            {
                _db.Books.Remove(book);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // DETAILS (GET)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var book = await _db.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Loans)
                    .ThenInclude(l => l.Member)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null) return NotFound();

            return View(book);
        }
    }
}