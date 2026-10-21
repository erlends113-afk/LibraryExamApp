using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryExamApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialExamMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    RegisteredDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    ISBN = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    PublishYear = table.Column<int>(type: "INTEGER", nullable: false),
                    AuthorId = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookLoans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookId = table.Column<int>(type: "INTEGER", nullable: false),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false),
                    LoanDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsReturned = table.Column<bool>(type: "INTEGER", nullable: false),
                    FineAmount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookLoans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookLoans_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookLoans_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "BirthDate", "Email", "FullName" },
                values: new object[,]
                {
                    { 1, new DateTime(1865, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "rainis@latvija.lv", "Rainis" },
                    { 2, new DateTime(1879, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "skalbe@latvija.lv", "Kārlis Skalbe" },
                    { 3, new DateTime(1901, 10, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "caks@latvija.lv", "Aleksandrs Čaks" },
                    { 4, new DateTime(1906, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "eglitis@latvija.lv", "Anšlavs Eglītis" },
                    { 5, new DateTime(1897, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "maurina@latvija.lv", "Zenta Mauriņa" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "IsActive", "Title" },
                values: new object[,]
                {
                    { 1, "Dzejoļu krājumi", true, "Dzeja" },
                    { 2, "Tautas un autoru pasakas", true, "Pasakas" },
                    { 3, "Prozas darbi", true, "Romāns" },
                    { 4, "Lugas un dramaturģija", true, "Drāma" },
                    { 5, "Esejas un apceres", true, "Filozofija" }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "Email", "FullName", "IsActive", "Phone", "RegisteredDate" },
                values: new object[,]
                {
                    { 1, "janis@inbox.lv", "Jānis Bērziņš", true, "+37120000001", new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "liga@inbox.lv", "Līga Ozoliņa", true, "+37120000002", new DateTime(2023, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "arturs@inbox.lv", "Artūrs Kalniņš", true, "+37120000003", new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "anna@inbox.lv", "Anna Liepiņa", true, "+37120000004", new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "martins@inbox.lv", "Mārtiņš Vītols", false, "+37120000005", new DateTime(2024, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "CategoryId", "ISBN", "Price", "PublishYear", "Title" },
                values: new object[,]
                {
                    { 1, 1, 4, "978-0111111111", 12.50m, 1910, "Zelta zirgs" },
                    { 2, 1, 4, "978-0222222222", 18.00m, 1905, "Uguns un nakts" },
                    { 3, 2, 2, "978-0333333333", 8.99m, 1913, "Kaķīša dzirnavas" },
                    { 4, 3, 1, "978-0444444444", 15.20m, 1937, "Mūžības skartie" },
                    { 5, 4, 3, "978-0555555555", 14.50m, 1944, "Homo Novus" }
                });

            migrationBuilder.InsertData(
                table: "BookLoans",
                columns: new[] { "Id", "BookId", "DueDate", "FineAmount", "IsReturned", "LoanDate", "MemberId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, true, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 2, new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1.50m, false, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 3, 3, new DateTime(2026, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, false, new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 4, 4, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 3.00m, true, new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 5, 5, new DateTime(2026, 10, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, false, new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookLoans_BookId",
                table: "BookLoans",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookLoans_MemberId",
                table: "BookLoans",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookLoans");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
