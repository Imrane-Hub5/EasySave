var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureHttpJsonOptions(o =>
    o.SerializerOptions.PropertyNameCaseInsensitive = true);

var app = builder.Build();

string logDir = Path.Combine(AppContext.BaseDirectory, "Logs");
Directory.CreateDirectory(logDir);

app.MapGet("/health", () => Results.Ok("OK"));

app.MapPost("/log", async (HttpRequest request) =>
{
    try
    {
        request.EnableBuffering();
        using StreamReader reader = new StreamReader(request.Body, leaveOpen: true);
        string body = await reader.ReadToEndAsync();

        string logFile = Path.Combine(logDir, DateTime.Now.ToString("yyyy-MM-dd") + ".json");
        await File.AppendAllTextAsync(logFile, body + Environment.NewLine);

        return Results.Ok("Log received");
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Run("http://0.0.0.0:5000");
