namespace Sauvio.Dto
{
    public class ChangePasswordDTO
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; } = string.Empty;
    }
}
