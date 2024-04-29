using BlazorAuthFromScratch.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents();

builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("TdtsCookie")
    .AddCookie("TdtsCookie")
    .AddGoogle(options =>
    {
        builder.Configuration.GetSection("Authentication:Google").Bind(options);
        options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
    }).
    AddMicrosoftAccount(options =>
    {
        builder.Configuration.GetSection("Authentication:Microsoft").Bind(options);
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>();
app.MapIdentityEndpoints();

app.Run();
