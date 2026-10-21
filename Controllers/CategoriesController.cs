using LibraryExamApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace LibraryExamApp.Controllers {
    public class CategoriesController : Controller {
        private readonly AppDbContext _db;
        public CategoriesController(AppDbContext db) => _db = db;
        public async Task<IActionResult> Index() => View(await _db.Categories.Include(c => c.Books).ToListAsync());
    }
}