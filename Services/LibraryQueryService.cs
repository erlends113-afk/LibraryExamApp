using LibraryExamApp.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryExamApp.Services
{
    public class LibraryQueryService : ILibraryQueryService
    {
        private readonly AppDbContext _db;

        public LibraryQueryService(AppDbContext db)
        {
            _db = db;
        }

        // 1. Where + OrderByDescending (Filtrēšana un kārtošana)
        public async Task<List<BookFilterDto>> GetBooksFilteredAndSortedAsync(decimal minPrice)
        {
            return await _db.Books
                .Where(b => b.Price >= minPrice)
                .OrderByDescending(b => b.Price)
                .Select(b => new BookFilterDto(b.Id, b.Title, b.Price, b.PublishYear, b.Author!.FullName))
                .ToListAsync();
        }

        // 2. Include / Navigācija no 3 tabulām (Aizņēmums + Grāmata + Lasītājs)
        public async Task<List<LoanDetailsDto>> GetActiveLoansWithDetailsAsync()
        {
            return await _db.BookLoans
                .Where(l => !l.IsReturned)
                .OrderBy(l => l.DueDate)
                .Select(l => new LoanDetailsDto(
                    l.Id,
                    l.Book!.Title,
                    l.Member!.FullName,
                    l.DueDate,
                    l.IsReturned,
                    l.FineAmount))
                .ToListAsync();
        }

        // 3. Īstais GroupBy kopā ar Count un Average (Grupēšana)
        public async Task<List<CategoryGroupStatsDto>> GetCategoryStatsAsync()
        {
            var data = await _db.Books
                .GroupBy(b => b.Category!.Title)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    BookCount = g.Count(),
                    AvgPrice = g.Average(b => b.Price)
                })
                .OrderByDescending(x => x.BookCount)
                .ToListAsync();

            // DTO izveide un noapaļošana atmiņā
            return data.Select(x => new CategoryGroupStatsDto(
                x.CategoryName,
                x.BookCount,
                Math.Round(x.AvgPrice, 2)
            )).ToList();
        }

        // 4. Any + Count + Max (Agregāti ar nosacījumu)
        public async Task<LoanAggregateResultDto> GetLoanAggregatesAsync()
        {
            var hasUnpaid = await _db.BookLoans.AnyAsync(l => !l.IsReturned && l.FineAmount > 0);
            var overdueCount = await _db.BookLoans.CountAsync(l => !l.IsReturned && l.DueDate < DateTime.Today);
            var maxFine = await _db.BookLoans.MaxAsync(l => (decimal?)l.FineAmount) ?? 0m;

            return new LoanAggregateResultDto(hasUnpaid, overdueCount, maxFine);
        }

        // 5. Brīvā izvēle: Autoru reitings pēc aizņemto grāmatu skaita
        public async Task<List<AuthorPopularityDto>> GetTopPopularAuthorsAsync()
        {
            var data = await _db.Authors
                .Select(a => new
                {
                    a.FullName,
                    LoanCount = a.Books.SelectMany(b => b.Loans).Count()
                })
                .OrderByDescending(x => x.LoanCount)
                .ToListAsync();

            return data.Select(x => new AuthorPopularityDto(x.FullName, x.LoanCount)).ToList();
        }
    }
}