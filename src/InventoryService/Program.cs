using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog configuration
builder.Logging.ClearProviders(); // remove default logging providers

using var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("ApplicationName", "Inventory")
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.AddSerilog(logger);

// Build the app
var app = builder.Build();
app.UseHttpsRedirection();

app.MapGet("/reserve-items", () => true)
.WithName("ReserveItems");

await app.RunAsync();
