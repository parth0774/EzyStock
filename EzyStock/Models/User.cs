using System.ComponentModel.DataAnnotations.Schema;

namespace EzyStock.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhone { get; set; }
        public UserRole? Role { get; set; }
       
    }
}
