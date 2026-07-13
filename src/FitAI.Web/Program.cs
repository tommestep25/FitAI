using FitAI.Web.Components;
using FitAI.Infrastructure;
using FitAI.Application;
using MudBlazor.Services;
using FitAI.Web.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using FitAI.Application.Common.Security;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddHttpContextAccessor();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<
    ICurrentUserService,
    CurrentUserService>();

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
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy =
            CookieSecurePolicy.Always;

        options.Cookie.SameSite =
            SameSiteMode.Lax;

        options.LoginPath = "/login";
        options.LogoutPath = "/account/logout";
        options.AccessDeniedPath = "/access-denied";

        options.SlidingExpiration = true;
        options.ExpireTimeSpan =
            TimeSpan.FromDays(7);

        // API/Form endpoint ไม่ควรถูก redirect เป็น HTML
        options.Events =
            new CookieAuthenticationEvents
            {
                OnRedirectToLogin = context =>
                {
                    if (context.Request.Path
                        .StartsWithSegments("/account"))
                    {
                        context.Response.StatusCode =
                            StatusCodes.Status401Unauthorized;

                        return Task.CompletedTask;
                    }

                    context.Response.Redirect(
                        context.RedirectUri);

                    return Task.CompletedTask;
                }
            };
    });


builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

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
