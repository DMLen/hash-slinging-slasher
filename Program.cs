using Microsoft.EntityFrameworkCore;
using hash_slinging_slasher.Database;
using hash_slinging_slasher.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ImageSearchContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

//create db on first run if it doesnt already exist
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ImageSearchContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

//test endpoint, retrieve all image records as json
app.MapGet("/images", async (ImageSearchContext db) =>
{
    return await db.ImageRecords.ToListAsync();
});

//add test record to db
app.MapPost("/images/test", async (ImageSearchContext db) =>
{
    var testImage = new ImageRecord
    {
        Hash = (ulong)Random.Shared.NextInt64(), //image hash
        FileName = "test.png", //name of the image file
        OriginURL = "www.google.com/test.jpg", //where the image was first found
        CreatedAt = DateTime.UtcNow //timestamp
    };
    
    db.ImageRecords.Add(testImage);
    await db.SaveChangesAsync();
    
    return new { Message = "Test image was added.", Data = testImage };
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
