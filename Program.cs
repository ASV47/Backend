using Codeikoo.TodoApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default"))
);

// Allow Angular dev server
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        p => p.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
    );
});

var app = builder.Build();

// Create DB automatically (simple starter; you can switch to migrations later)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", p =>
        p.WithOrigins(
            "http://localhost:4200",
            "https://asv47.github.io"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    // Railway expects the app to listen on this PORT
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}



app.UseCors("CorsPolicy");

app.UseCors("cors");
app.MapGet("/healthz", () => Results.Ok("ok"));
app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.MapControllers();
app.Run();
