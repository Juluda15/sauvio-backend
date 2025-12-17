using Sauvio.Business.Services.Account;
using Sauvio.Business.Services.Email;
using Sauvio.Business.Services.Finance;
using SauvioData;
using SauvioData.Data;
using SauvioData.Interfaces;
using SuavioData.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFinanceService, FinanceService>();
builder.Services.AddSingleton<DbConnectionFactory>();
builder.Services.AddScoped<IFinanceData, FinanceData>();
builder.Services.AddScoped<IAccountData, AccountData>();


builder.Services.AddControllers();
builder.Services.AddOpenApi();

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


var app = builder.Build();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
