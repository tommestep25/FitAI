using FitAI.Web.Components;
using FitAI.Infrastructure;
using FitAI.Application;
using MudBlazor.Services;
using FitAI.Web.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using FitAI.Application.Common.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<
    ICurrentUserService,
    CurrentUserService>();

builder.Services.AddScoped<
    SupabaseCookieAuthenticationEvents>();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            AuthConstants.CookieScheme;

        options.DefaultSignInScheme =
            AuthConstants.CookieScheme;

        options.DefaultChallengeScheme =
            AuthConstants.CookieScheme;
    })
    .AddCookie(AuthConstants.CookieScheme, options =>
    {
        options.Cookie.Name = "FitAI.Auth";
        options.Cookie.Path = "/";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy =
            CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite =
            SameSiteMode.Lax;

        options.LoginPath = "/login";
        options.LogoutPath = "/account/logout";
        options.AccessDeniedPath = "/access-denied";

        options.ExpireTimeSpan =
            TimeSpan.FromDays(7);

        options.SlidingExpiration = true;

        options.EventsType =
            typeof(SupabaseCookieAuthenticationEvents);
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapAccountEndpoints();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();
