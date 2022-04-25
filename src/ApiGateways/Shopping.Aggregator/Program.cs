using Shopping.Aggregator.Services;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Common.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

// Add services to the container.

builder.Services.AddTransient<LoggingDelegatingHandler>();

builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
                 c.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]))
                 .AddHttpMessageHandler<LoggingDelegatingHandler>()
                 .AddPolicyHandler(GetRetryPolicy())
                 .AddPolicyHandler(GetCircuitBreakerPolicy());

builder.Services.AddHttpClient<IBasketService, BasketService>(c =>
                 c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BasketUrl"]))
                 .AddHttpMessageHandler<LoggingDelegatingHandler>()
                 .AddPolicyHandler(GetRetryPolicy())
                 .AddPolicyHandler(GetCircuitBreakerPolicy());

builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
                 c.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderingUrl"]))
                 .AddHttpMessageHandler<LoggingDelegatingHandler>()
                 .AddPolicyHandler(GetRetryPolicy())
                 .AddPolicyHandler(GetCircuitBreakerPolicy());

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

app.UseAuthorization();

app.MapControllers();

app.Run();


static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    // In this case will wait for
    //  2 ^ 1 = 2 seconds then
    //  2 ^ 2 = 4 seconds then
    //  2 ^ 3 = 8 seconds then
    //  2 ^ 4 = 16 seconds then
    //  2 ^ 5 = 32 seconds

    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(
            retryCount: 5,
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (exception, retryCount, context) =>
            {
                Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
            });
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30)
        );
}
