using Sauvio.Dto;
using Sauvio.Models.User;

public interface IAccountService
{
    Task<string> Register(RegisterDTO dto);
    Task<(bool Success, string Message, User? User)> Login(LoginDTO dto);
    Task<string> ConfirmEmail(string token);
}
