using LibraryExamApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryExamApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<BookLoan> BookLoans => Set<BookLoan>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // FluentAPI: Relācijas un Kaskādes dzēšana
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict); // Aizliedz dzēst autoru, ja tam ir grāmatas!

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookLoan>()
                .HasOne(bl => bl.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(bl => bl.BookId)
                .OnDelete(DeleteBehavior.Cascade); // Izdzēšot grāmatu, izdzēšas aizņēmumu ieraksti

            modelBuilder.Entity<BookLoan>()
                .HasOne(bl => bl.Member)
                .WithMany(m => m.Loans)
                .HasForeignKey(bl => bl.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            // SĀKOTNĒJIE DATI (vismaz 5 ieraksti galvenajās tabulās)
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, FullName = "Rainis", Email = "rainis@latvija.lv", BirthDate = new DateTime(1865, 9, 11) },
                new Author { Id = 2, FullName = "Kārlis Skalbe", Email = "skalbe@latvija.lv", BirthDate = new DateTime(1879, 11, 7) },
                new Author { Id = 3, FullName = "Aleksandrs Čaks", Email = "caks@latvija.lv", BirthDate = new DateTime(1901, 10, 27) },
                new Author { Id = 4, FullName = "Anšlavs Eglītis", Email = "eglitis@latvija.lv", BirthDate = new DateTime(1906, 10, 14) },
                new Author { Id = 5, FullName = "Zenta Mauriņa", Email = "maurina@latvija.lv", BirthDate = new DateTime(1897, 12, 15) }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Title = "Dzeja", Description = "Dzejoļu krājumi", IsActive = true },
                new Category { Id = 2, Title = "Pasakas", Description = "Tautas un autoru pasakas", IsActive = true },
                new Category { Id = 3, Title = "Romāns", Description = "Prozas darbi", IsActive = true },
                new Category { Id = 4, Title = "Drāma", Description = "Lugas un dramaturģija", IsActive = true },
                new Category { Id = 5, Title = "Filozofija", Description = "Esejas un apceres", IsActive = true }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Zelta zirgs", ISBN = "978-0111111111", Price = 12.50m, PublishYear = 1910, AuthorId = 1, CategoryId = 4 },
                new Book { Id = 2, Title = "Uguns un nakts", ISBN = "978-0222222222", Price = 18.00m, PublishYear = 1905, AuthorId = 1, CategoryId = 4 },
                new Book { Id = 3, Title = "Kaķīša dzirnavas", ISBN = "978-0333333333", Price = 8.99m, PublishYear = 1913, AuthorId = 2, CategoryId = 2 },
                new Book { Id = 4, Title = "Mūžības skartie", ISBN = "978-0444444444", Price = 15.20m, PublishYear = 1937, AuthorId = 3, CategoryId = 1 },
                new Book { Id = 5, Title = "Homo Novus", ISBN = "978-0555555555", Price = 14.50m, PublishYear = 1944, AuthorId = 4, CategoryId = 3 }
            );

            modelBuilder.Entity<Member>().HasData(
                new Member { Id = 1, FullName = "Jānis Bērziņš", Email = "janis@inbox.lv", Phone = "+37120000001", RegisteredDate = new DateTime(2023, 1, 15), IsActive = true },
                new Member { Id = 2, FullName = "Līga Ozoliņa", Email = "liga@inbox.lv", Phone = "+37120000002", RegisteredDate = new DateTime(2023, 3, 22), IsActive = true },
                new Member { Id = 3, FullName = "Artūrs Kalniņš", Email = "arturs@inbox.lv", Phone = "+37120000003", RegisteredDate = new DateTime(2024, 2, 10), IsActive = true },
                new Member { Id = 4, FullName = "Anna Liepiņa", Email = "anna@inbox.lv", Phone = "+37120000004", RegisteredDate = new DateTime(2024, 5, 1), IsActive = true },
                new Member { Id = 5, FullName = "Mārtiņš Vītols", Email = "martins@inbox.lv", Phone = "+37120000005", RegisteredDate = new DateTime(2024, 6, 12), IsActive = false }
            );

            modelBuilder.Entity<BookLoan>().HasData(
                new BookLoan { Id = 1, BookId = 1, MemberId = 1, LoanDate = new DateTime(2026, 9, 1), DueDate = new DateTime(2026, 9, 15), IsReturned = true, FineAmount = 0m },
                new BookLoan { Id = 2, BookId = 2, MemberId = 2, LoanDate = new DateTime(2026, 10, 1), DueDate = new DateTime(2026, 10, 15), IsReturned = false, FineAmount = 1.50m },
                new BookLoan { Id = 3, BookId = 3, MemberId = 3, LoanDate = new DateTime(2026, 10, 5), DueDate = new DateTime(2026, 10, 19), IsReturned = false, FineAmount = 0m },
                new BookLoan { Id = 4, BookId = 4, MemberId = 4, LoanDate = new DateTime(2026, 8, 10), DueDate = new DateTime(2026, 8, 24), IsReturned = true, FineAmount = 3.00m },
                new BookLoan { Id = 5, BookId = 5, MemberId = 1, LoanDate = new DateTime(2026, 10, 10), DueDate = new DateTime(2026, 10, 24), IsReturned = false, FineAmount = 0m }
            );
        }
    }
}