namespace LibraryExamApp.Services
{
    public interface ILibraryQueryService
    {
        // 1. Filtrēšana un kārtošana: Dārgākas grāmatas par noteiktu cenu, sakārtotas dilstoši
        Task<List<BookFilterDto>> GetBooksFilteredAndSortedAsync(decimal minPrice);

        // 2. Saistīto datu apvienošana: Aizņēmumi ar Grāmatas un Lasītāja datiem (3 tabulas)
        Task<List<LoanDetailsDto>> GetActiveLoansWithDetailsAsync();

        // 3. Grupēšana: Grāmatu skaits un vidējā cena pa kategorijām
        Task<List<CategoryGroupStatsDto>> GetCategoryStatsAsync();

        // 4. Agregāts / Nosacījums: Pārbauda, vai kādam lasītājam ir soda nauda, un saskaita kavētājus
        Task<LoanAggregateResultDto> GetLoanAggregatesAsync();

        // 5. Brīvā izvēle: Autoru reitings pēc izlasīto/paņemto grāmatu popularitātes
        Task<List<AuthorPopularityDto>> GetTopPopularAuthorsAsync();
    }

    public record BookFilterDto(int Id, string Title, decimal Price, int PublishYear, string AuthorName);
    public record LoanDetailsDto(int LoanId, string BookTitle, string MemberName, DateTime DueDate, bool IsReturned, decimal Fine);
    public record CategoryGroupStatsDto(string CategoryName, int BookCount, decimal AvgPrice);
    public record LoanAggregateResultDto(bool HasAnyUnpaidFines, int TotalOverdueCount, decimal MaxFine);
    public record AuthorPopularityDto(string AuthorName, int TotalTimesBorrowed);
}