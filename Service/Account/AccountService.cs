using Microsoft.EntityFrameworkCore;
using Sauvio.Data;
using Sauvio.Dto;
using Sauvio.Models.User;
using Sauvio.Services.Email;
using BCrypt.Net;


namespace Sauvio.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _db;
        private readonly IEmailService _emailService;

        public AccountService(AppDbContext dbContext, IEmailService emailService)
        {
            _db = dbContext;
            _emailService = emailService;
        }

        public async Task<string> Register(RegisterDTO dto)
        {
            var existing = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (existing != null)
                return "Email already registered";

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var token = Guid.NewGuid().ToString();

            var newUser = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = hashedPassword,
                ConfirmationToken = token,
                IsConfirmed = false
            };

            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();

            _emailService.SendConfirmationEmail(dto.Email, token);

            return "Registration successful. Please check your email to confirm.";
        }

        public async Task<(bool Success, string Message, User? User)> Login(LoginDTO dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !user.IsConfirmed)
                return (false, "Invalid or unconfirmed credentials", null);

            bool match = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
            return match ? (true, "Login successful", user) : (false, "Invalid credentials", null);
        }

        public async Task<string> ConfirmEmail(string token)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.ConfirmationToken == token);

            if (user == null)
            {
                var alreadyConfirmed = await _db.Users
                    .FirstOrDefaultAsync(u => u.IsConfirmed && u.ConfirmationToken == null);

                if (alreadyConfirmed != null)
                    return "Email already confirmed.";
                else
                    return "Invalid token";
            }

            user.IsConfirmed = true;
            user.ConfirmationToken = null;

            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return "Email confirmed successfully!";
        }
    }
}

