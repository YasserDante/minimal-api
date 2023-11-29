using System.Text.Json;
using Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

List<Symbol>? symbols = [];
string? text = File.ReadAllText("symbols.json");
if (text is not null)
{
    using var doc = JsonDocument.Parse(text);

    var root = doc.RootElement.GetProperty("symbols");
    symbols = JsonSerializer.Deserialize<List<Symbol>>(root);
}

app.MapGet("/symbols", () =>
{
    return symbols?.Select(_ => _.Name);
})
.WithName("Get All Symbols")
.WithOpenApi();

app.MapGet("/info", (string symbol, DateTime start, DateTime end) =>
{
    return symbols?.Where(_ => _.Name == symbol);
})
.WithName("Get Symbol Info")
.WithOpenApi();

app.Run();