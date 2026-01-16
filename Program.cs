using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Sauvio.Business.Services.Account;
using Sauvio.Business.Services.Email;
using Sauvio.Business.Services.Finance;
using SauvioData;
using SauvioData.Data;
using SauvioData.Interfaces;
using SuavioData.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ===== Add services to the container =====
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFinanceService, FinanceService>();
builder.Services.AddSingleton<DbConnectionFactory>();
builder.Services.AddScoped<IFinanceData, FinanceRepo>();
builder.Services.AddScoped<IAccountData, AccountRepo>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ===== CORS =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5500",
            "http://127.0.0.1:5500"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// ===== JWT Authentication =====
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; 
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// ===== Swagger/OpenAPI =====
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
