using LibraryExamApp.Data;
using LibraryExamApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryExamApp.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly AppDbContext _db;
        public AuthorsController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index() => View(await _db.Authors.Include(a => a.Books).ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author)
        {
            if (ModelState.IsValid)
            {
                _db.Add(author);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var a = await _db.Authors.FindAsync(id);
            return a == null ? NotFound() : View(a);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Author author)
        {
            if (id != author.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _db.Update(author);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var a = await _db.Authors.Include(x => x.Books).FirstOrDefaultAsync(m => m.Id == id);
            return a == null ? NotFound() : View(a);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _db.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
            if (author != null)
            {
                if (author.Books.Any())
                {
                    TempData["ErrorMessage"] = "Šo autoru nevar dzēst, jo viņam sistēmā ir piesaistītas grāmatas!";
                    return RedirectToAction(nameof(Index));
                }
                _db.Authors.Remove(author);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}