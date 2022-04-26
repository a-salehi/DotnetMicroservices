using Common.Logging;
using Discount.API.Extensions;
using Discount.API.Repositories;
using Discount.API.Repositories.Interfaces;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog(SeriLogger.Configure);

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
                .AddNpgSql(builder.Configuration["DatabaseSettings:ConnectionString"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/hc", new HealthCheckOptions()
 {
     Predicate = _ => true,
     ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
 });

app.MigrateDatabase<Program>();

app.Run();
