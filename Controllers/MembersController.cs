using LibraryExamApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace LibraryExamApp.Controllers {
    public class MembersController : Controller {
        private readonly AppDbContext _db;
        public MembersController(AppDbContext db) => _db = db;
        public async Task<IActionResult> Index() => View(await _db.Members.ToListAsync());
    }
}