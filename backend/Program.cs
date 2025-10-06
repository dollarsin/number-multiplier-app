using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов приложения
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Генерируем Swagger только для удобства разработки
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Number Multiplier API", Version = "v1" });
});

// Сжатие ответов (Brotli + Gzip)
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});
builder.Services.Configure<BrotliCompressionProviderOptions>(o => o.Level = CompressionLevel.Fastest);
builder.Services.Configure<GzipCompressionProviderOptions>(o => o.Level = CompressionLevel.Fastest);

// Кэширование ответов (для идемпотентных GET)
builder.Services.AddResponseCaching();

// Простейший лимитер запросов (fixed window)
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.Window = TimeSpan.FromSeconds(1); // окно 1 секунда
        opt.PermitLimit = 10; // не более 10 запросов в секунду с IP
        opt.QueueLimit = 0;
        opt.AutoReplenishment = true;
    });
});

// CORS: разрешаем локальный фронтенд на 3000 порту
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Конвейер обработки HTTP-запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HSTS в проде и редирект на HTTPS
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseResponseCompression();
app.UseResponseCaching();
app.UseRateLimiter();

app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();

// Простой health-эндпоинт
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();