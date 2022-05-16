using IdentityServer;
using IdentityServer4.Models;
using IdentityServer4.Test;
using IdentityServerHost.Quickstart.UI;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://+:7000");
builder.Services.AddControllersWithViews();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddIdentityServer(x =>
                    {
                       x.IssuerUri = "http://identityserver:7000";
                    })
                    .AddInMemoryClients(Config.Clients)
                    .AddInMemoryApiScopes(Config.ApiScopes)
                    .AddInMemoryIdentityResources(Config.IdentityResources)
                    .AddTestUsers(TestUsers.Users)
                    .AddDeveloperSigningCredential();
}
else
{
    builder.Services.AddIdentityServer()
                    .AddInMemoryClients(Config.Clients)
                    .AddInMemoryApiScopes(Config.ApiScopes)
                    .AddInMemoryIdentityResources(Config.IdentityResources)
                    .AddTestUsers(TestUsers.Users)
                    .AddDeveloperSigningCredential();
}

var app = builder.Build();

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict });
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
