using System.ComponentModel.DataAnnotations;

namespace LibraryExamApp.Models.Dtos
{
    public record BookResponseDto(int Id, string Title, string ISBN, decimal Price, int PublishYear, string Author, string Category);

    public class CreateBookDto
    {
        [Required] [MaxLength(120)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string ISBN { get; set; } = string.Empty;

        [Range(0.01, 500)]
        public decimal Price { get; set; }

        public int PublishYear { get; set; }

        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
    }

    public class UpdateLoanStatusDto
    {
        public bool IsReturned { get; set; }
        public decimal FineAmount { get; set; }
    }
}