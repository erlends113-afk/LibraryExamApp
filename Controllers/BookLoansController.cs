using LibraryExamApp.Data;
using LibraryExamApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibraryExamApp.Controllers
{
    public class BookLoansController : Controller
    {
        private readonly AppDbContext _db;
        public BookLoansController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var loans = await _db.BookLoans.Include(b => b.Book).Include(b => b.Member).ToListAsync();
            return View(loans);
        }

        public IActionResult Create()
        {
            ViewBag.BookId = new SelectList(_db.Books, "Id", "Title");
            ViewBag.MemberId = new SelectList(_db.Members, "Id", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookLoan loan)
        {
            if (ModelState.IsValid)
            {
                _db.Add(loan);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.BookId = new SelectList(_db.Books, "Id", "Title", loan.BookId);
            ViewBag.MemberId = new SelectList(_db.Members, "Id", "FullName", loan.MemberId);
            return View(loan);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var loan = await _db.BookLoans.FindAsync(id);
            if (loan == null) return NotFound();

            ViewBag.BookId = new SelectList(_db.Books, "Id", "Title", loan.BookId);
            ViewBag.MemberId = new SelectList(_db.Members, "Id", "FullName", loan.MemberId);
            return View(loan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookLoan loan)
        {
            if (id != loan.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _db.Update(loan);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.BookId = new SelectList(_db.Books, "Id", "Title", loan.BookId);
            ViewBag.MemberId = new SelectList(_db.Members, "Id", "FullName", loan.MemberId);
            return View(loan);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var loan = await _db.BookLoans.Include(l => l.Book).Include(l => l.Member).FirstOrDefaultAsync(m => m.Id == id);
            return loan == null ? NotFound() : View(loan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loan = await _db.BookLoans.FindAsync(id);
            if (loan != null)
            {
                _db.BookLoans.Remove(loan);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}