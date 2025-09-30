namespace Sauvio.Models.User
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmationToken { get; set; }
        public bool IsConfirmed { get; set; }
    }

}
