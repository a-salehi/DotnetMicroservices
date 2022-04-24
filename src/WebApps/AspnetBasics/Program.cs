using AspnetBasics.Services;
using Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

builder.Services.AddTransient<LoggingDelegatingHandler>();

builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
                c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]))
                .AddHttpMessageHandler<LoggingDelegatingHandler>();

builder.Services.AddHttpClient<IBasketService, BasketService>(c =>
                c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]))
                .AddHttpMessageHandler<LoggingDelegatingHandler>();

builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
                c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]))
                .AddHttpMessageHandler<LoggingDelegatingHandler>();

builder.Services.AddRazorPages();

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});

app.Run();
