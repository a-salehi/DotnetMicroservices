using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Serilog;
using Common.Logging;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);

//builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
//builder.Logging.AddConsole();
//builder.Logging.AddDebug();

var authenticationProviderKey = "IdentityApiKey";

// NUGET - Microsoft.AspNetCore.Authentication.JwtBearer
builder.Services.AddAuthentication()
 .AddJwtBearer(authenticationProviderKey, x =>
 {
     x.RequireHttpsMetadata = false;
     x.Authority = "http://identityserver:7000"; // IDENTITY SERVER URL
                                             //x.RequireHttpsMetadata = false;
                 x.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateAudience = false
     };
 });

builder.Services.AddOcelot().AddCacheManager(settings => settings.WithDictionaryHandle());

var app = builder.Build();
//app.UseHttpsRedirection();
app.MapGet("/", () => "Hello World!");
app.UseRouting();

app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    //endpoints.MapGet("/", async context =>
    //{
    //    await context.Response.WriteAsync("Hello World!");
    //});
});

await app.UseOcelot(); 

app.Run();
