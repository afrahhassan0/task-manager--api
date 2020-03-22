using _netCoreBackend.Models.Enums;

namespace _netCoreBackend.Models
{
    public class UserDTO
    {
        public string Email { get; set;}
        public Role Role{get;set;}
        public string FullName {get;set;}
        
        public string Username { get; set; }
        public string Password { get; set; }
        public  string OrganizationName { get; set; }
        


#nullable  enable
        public string? About {get;set;}
        
    }
}