using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Map("/wp-json/bm/v1/velobasar", (HttpContext context) =>
{
    Console.WriteLine("============================================================");
    Console.WriteLine($"Start of {context.Request.Method} request");

    Console.WriteLine("Headers:");
    foreach (KeyValuePair<string, StringValues> header in context.Request.Headers)
    {
        Console.WriteLine($"  {header.Key}:");
        foreach (string? value in header.Value)
        {
            Console.WriteLine($"    {value ?? "null"}");
        }
    }

    Console.WriteLine();
    Console.WriteLine("Body:");
    using StreamReader bodyReader = new(context.Request.Body);    
    string body = bodyReader.ReadToEnd();
    Console.WriteLine(body);

    Console.WriteLine("End of request");
    Console.WriteLine("============================================================");
})
.WithOpenApi();

app.Run();
