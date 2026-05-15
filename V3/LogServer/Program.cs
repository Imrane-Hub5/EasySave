using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

string logDir = Path.Combine(AppContext.BaseDirectory, "Logs");
Directory.CreateDirectory(logDir);

/// <summary>
/// Receives a log entry via HTTP POST and writes it to the central log file
/// </summary>
app.MapPost("/log", async (HttpContext context) =>
{
    try
    {
        using StreamReader reader = new StreamReader(context.Request.Body);
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