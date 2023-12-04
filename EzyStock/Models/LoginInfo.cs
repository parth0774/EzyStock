namespace EzyStock.Models
{
    public class LoginInfo
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public AccountType? AccType { get; set; }
    }
}
