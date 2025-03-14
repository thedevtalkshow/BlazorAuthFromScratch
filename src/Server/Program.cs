using BlazorAuthFromScratch.Components;
using BlazorAuthFromScratch.Services;
using GraphApiLibrary;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents();

builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("TdtsCookie")
    .AddCookie("TdtsCookie")
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
        options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
    })
    .AddMicrosoftAccount(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"]!;
        options.TokenEndpoint = builder.Configuration["Authentication:Microsoft:TokenEndpoint"]!;
        options.AuthorizationEndpoint = builder.Configuration["Authentication:Microsoft:AuthorizationEndpoint"]!;
        options.Events.OnCreatingTicket = async context =>
        {
            var something = new GraphApiUserService(context.Backchannel, context.AccessToken!);
            string? photoBase64 = await something.GetUserPhoto();

            if (string.IsNullOrEmpty(photoBase64) == false)
            {
                context.Identity!.AddClaim(new Claim("urn:microsoft:picture", photoBase64));
            }
        };
    });

builder.Services.AddTransient<IClaimsTransformation, BlazorAuthFromScratchClaimsTransformation>();

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
