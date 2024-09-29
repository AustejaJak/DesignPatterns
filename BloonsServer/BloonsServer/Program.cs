var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();

app.UseStaticFiles();

app.MapGet("/server/{*operations}", (string? operations) =>
{
    return $"Operations: {operations}";
});


app.Run();
