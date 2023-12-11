using Microsoft.EntityFrameworkCore;
using RedisAPI.Infra;
using RedisAPI.Infra.Caching;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ProductListDbContext>(db => db.UseInMemoryDatabase("ProductDb"));
builder.Services.AddScoped<ICachingService, CachingService>();

//Configuração do Redis
builder.Services.AddStackExchangeRedisCache(x =>
{
    x.InstanceName = "instance";
    x.Configuration = "localhost:6379";
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();