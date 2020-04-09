using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace _netCoreBackend.Models
{   [DisplayColumn("Membership")]
    public class UserGroup
    {
        //Usergroup properties:
        public UserGroup(){}
        [Key]
        [Required]
        public string MemberUsername {get;set;}
        
        [Key]
        [Required]
        public int GroupID {get;set;}

        public Group Group {get;set;}
        public Credentials UserAccount {get;set;}
        /* *************** */
    }
}