
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using _netCoreBackend.Models.Enums;


namespace _netCoreBackend.Models
{
    public class User
    {   
       
        public User() {}

        //User properties
        [Key]
        [Required]
        public int UserId { get;set; }
        
        [Required]
        public string Email { get; set;}
        
        [Required]
        public Role Role{get;set;}
        [Required]
        public string FullName {get;set;}        
        
        #nullable enable
        public string? About {get;set;}

        public string? CustomBackgroundColor {get;set;}
        /* *********************** */

        //Collection Navigation Properties:
        
        
        
        public ICollection<Credentials> Credentials {get;set;}


        
    }
}