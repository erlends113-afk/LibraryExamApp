using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryExamApp.Models.Entities
{
    // 1. Tabula: Autori
    [Table("Authors")]
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vārds ir obligāts!")]
        [MaxLength(80, ErrorMessage = "Maksimums 80 simboli")]
        [Display(Name = "Pilns vārds")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Nederīgs e-pasta formāts")]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Dzimšanas datums")]
        public DateTime BirthDate { get; set; }

        public List<Book> Books { get; set; } = new();
    }

    // 2. Tabula: Kategorijas
    [Table("Categories")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nosaukums ir obligāts!")]
        [MaxLength(50)]
        [Display(Name = "Kategorijas nosaukums")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(200)]
        [Display(Name = "Apraksts")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Aktīva")]
        public bool IsActive { get; set; } = true;

        public List<Book> Books { get; set; } = new();
    }

    // 3. Tabula: Grāmatas
    [Table("Books")]
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nosaukums ir obligāts!")]
        [MaxLength(120)]
        [Display(Name = "Grāmatas nosaukums")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[0-9]{3}-[0-9]{10}$", ErrorMessage = "Formāts: 978-0123456789")]
        public string ISBN { get; set; } = string.Empty;

        [Range(0.01, 500.0, ErrorMessage = "Cenai jābūt no 0.01 līdz 500.00")]
        [Column(TypeName = "TEXT")]
        [Display(Name = "Cena (€)")]
        public decimal Price { get; set; }

        [Display(Name = "Izdošanas gads")]
        [Range(1800, 2030)]
        public int PublishYear { get; set; }

        // Ārējā atslēga 1
        [Required(ErrorMessage = "Jānorāda autors")]
        [Display(Name = "Autors")]
        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        // Ārējā atslēga 2
        [Required(ErrorMessage = "Jānorāda kategorija")]
        [Display(Name = "Kategorija")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<BookLoan> Loans { get; set; } = new();
    }

    // 4. Tabula: Lasītāji / Biedri
    [Table("Members")]
    public class Member
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vārds un uzvārds ir obligāts!")]
        [MaxLength(100)]
        [Display(Name = "Lasītājs")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Tālrunis")]
        public string Phone { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Reģistrēšanās datums")]
        public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Ir aktīvs biedrs")]
        public bool IsActive { get; set; } = true;

        public List<BookLoan> Loans { get; set; } = new();
    }

    // 5. Tabula: Saistošā tabula - Aizņēmumi (ar papildus datiem: datumiem, statusu, sodu)
    [Table("BookLoans")]
    public class BookLoan
    {
        [Key]
        public int Id { get; set; }

        // Ārējā atslēga 3
        [Required(ErrorMessage = "Jānorāda grāmata")]
        [Display(Name = "Grāmata")]
        public int BookId { get; set; }
        public Book? Book { get; set; }

        // Ārējā atslēga 4
        [Required(ErrorMessage = "Jānorāda lasītājs")]
        [Display(Name = "Lasītājs")]
        public int MemberId { get; set; }
        public Member? Member { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Paņemts")]
        public DateTime LoanDate { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        [Display(Name = "Termiņš")]
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(14);

        [Display(Name = "Nodots")]
        public bool IsReturned { get; set; } = false;

        [Display(Name = "Soda nauda (€)")]
        [Range(0, 100)]
        public decimal FineAmount { get; set; } = 0.0m;
    }
}