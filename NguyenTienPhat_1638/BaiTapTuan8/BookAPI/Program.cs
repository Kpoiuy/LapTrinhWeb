using BookAPI.Data;
using BookAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:9999");

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BookDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var contentRoot = Path.Combine(app.Environment.ContentRootPath, "Content");
var imageRoot = Path.Combine(contentRoot, "ImageBooks");
Directory.CreateDirectory(imageRoot);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(contentRoot),
    RequestPath = "/Content"
});

app.UseAuthorization();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=BooksMvc}/{action=Index}/{id?}");
app.MapGet("/health", () => Results.Ok(new { ok = true, time = DateTime.UtcNow }));

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BookDbContext>();
    dbContext.Database.EnsureCreated();
    ImageSeedWriter.EnsureSeedImages(imageRoot);
    BookRuntimeSeeder.EnsureSeedData(dbContext);
}

app.Run();
