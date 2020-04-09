using System.ComponentModel.DataAnnotations;
using _netCoreBackend.Models.Enums;

namespace _netCoreBackend.Models
{
    public class UserDTO
    {
        [Required]
        public string Email { get; set;}
    
        public string FullName {get;set;}
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        #nullable  enable
        public string? About {get;set;}
        public string? OrganizationName { get; set; }
        public string? BackgroundColor { get; set; }
        public Role? Role { get; set; }
        public string? JobTitle {get;set;}
        public long? PhoneNumber {get;set;}

    }
}