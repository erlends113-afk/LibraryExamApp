using LibraryExamApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryExamApp.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ILibraryQueryService _queries;
        public ReportsController(ILibraryQueryService queries) => _queries = queries;

        public async Task<IActionResult> Index(decimal minPrice = 10)
        {
            ViewBag.Query1 = await _queries.GetBooksFilteredAndSortedAsync(minPrice);
            ViewBag.Query2 = await _queries.GetActiveLoansWithDetailsAsync();
            ViewBag.Query3 = await _queries.GetCategoryStatsAsync();
            ViewBag.Query4 = await _queries.GetLoanAggregatesAsync();
            ViewBag.Query5 = await _queries.GetTopPopularAuthorsAsync();
            return View();
        }
    }
}