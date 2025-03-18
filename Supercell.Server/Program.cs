var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.WebHost.UseUrls("http://127.0.0.1:8181");

app.Use(async (context, next) =>
{
    Console.WriteLine($"[{DateTime.Now}] {context.Request.Method} {context.Request.Path}");
    await next();
});

app.UseRouting();

app.MapGet("/", () => "Hello World!");

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/supercell/{file}", async context =>
    {
        var fileName = context.Request.RouteValues["file"]?.ToString();
        if (string.IsNullOrEmpty(fileName))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("File name is required.");
            return;
        }

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

        if (File.Exists(filePath))
        {
            var fileBytes = await File.ReadAllBytesAsync(filePath);
            context.Response.ContentType = "application/octet-stream";
            await context.Response.Body.WriteAsync(fileBytes);
        }
        else
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("File not found.");
        }
    });
});

app.Run();
