using System.Text.Json;
using api;
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

//List<Symbol>? symbols = context.Books.ToList();
//string? text = File.ReadAllText("symbols.json");
//if (text is not null)
//{
//    using var doc = JsonDocument.Parse(text);

//    var root = doc.RootElement.GetProperty("symbols");
//    symbols = JsonSerializer.Deserialize<List<Symbol>>(root);
//}

using(var context=new APIContext())
{
    context.Books.Add(new Symbol()
    {
        Name = "BTCUSD",
        Date = DateTime.Now,
        Open = 10,
        High = 15,
        Low = 8,
        Close = 12,
        Volume = 1233
    });
    context.SaveChanges();
}
app.MapGet("/symbols", () =>
{
    string res = "OK\n";
    using (var context = new APIContext())
    {
        var syms = context.Books.ToList();
        foreach (var item in syms.Select(_ => _.Name).Distinct())
        {
            res += item + ",";
        }
    }
    res = res.Substring(0, res.Length - 1);
    return res;
    //return symbols?.Select(_ => _.Name);
})
.WithName("Get All Symbols")
.WithOpenApi();

app.MapGet("/info", (string symbol, DateTime start) =>
{
    string res = "";
    using (var context = new APIContext())
    {
        foreach (var item in context.Books.ToList().Where(_ => _.Name == symbol && _.Date > start))
        {
            res += item.Date.ToString("yyMMdd") + ",";
            res += item.Date.ToString("HHmm") + ",";
            res += item.Open.ToString() + ",";
            res += item.High.ToString() + ",";
            res += item.Low.ToString() + ",";
            res += item.Close.ToString() + ",";
            res += item.Volume.ToString();
            res += "\n";
        }
    }
    return res;
    //return symbols?.Where(_ => _.Name == symbol  && _.Date>start);
})
.WithName("Get Symbol Info")
.WithOpenApi();

//app.MapGet("/last", (string symbol, DateTime start) =>
//{
//    string res = "OK\n";
//    foreach (var item in symbols?.Where(_ => _.Name == symbol  && _.Date>start))
//    {
//        res += item.Date.ToShortDateString().Replace("/","-") + ",";
//        res += item.Date.ToLongTimeString() + ",";
//        res += item.Open.ToString() + ",";
//        res += item.High.ToString() + ",";
//        res += item.Low.ToString() + ",";
//        res += item.Close.ToString() + ",";
//        res += item.Volume.ToString();
//        res += "\n";
//    }
//    return res;
//    //return symbols?.Where(_ => _.Name == symbol  && _.Date>start);
//})
//.WithName("Get Symbol Info")
//.WithOpenApi();

app.Run();