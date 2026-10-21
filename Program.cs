using LibraryExamApp.Data;
using LibraryExamApp.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 1. Reģistrē MVC un JSON iestatījumus
builder.Services.AddControllersWithViews()
    .AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// 2. Swagger / OpenAPI dokumentācija
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. EF Core SQLite
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=exam_library.db"));

// 4. Centralizētais LINQ Serviss
builder.Services.AddScoped<ILibraryQueryService, LibraryQueryService>();

var app = builder.Build();

// Automātiskā migrācija un DB izveide palaižot
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // Izpilda EF Core migrācijas automātiski!
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Swagger UI iespējošana (pieejama pie /swagger)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bibliotēkas Eksāmena API v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Books}/{action=Index}/{id?}");

app.Run();