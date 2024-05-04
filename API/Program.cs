using API.Controllers;
using API.Extensions;
using API.Middlewares;
using Application;
using Application.AutoMapper;
using Domain.Abstractions;
using Infrastructure;
using Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ActorContext>(options =>
    options.UseInMemoryDatabase("ActorsDb"));

builder.Services.AddScoped<IActorRepository, ActorRepository>();
builder.Services.AddScoped<IActorService, ActorService>();
builder.Services.AddAutoMapper(typeof(ActorProfile));
builder.Services.AddScraper(builder.Configuration);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Actor API", Version = "v1" });
    c.ExampleFilters();
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<ActorController>();
builder.Services.AddHttpClient();
var app = builder.Build();
app.UseMiddleware<ExceptionResponseMiddleware>();
app.UseMiddleware<ExceptionLoggingMiddleware>();


using (var scope = app.Services.CreateScope())
{
    var scraper = scope.ServiceProvider.GetRequiredService<IScraper>();
    await scraper.ScrapeAsync(); 
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
