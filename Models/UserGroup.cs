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
        public int Member_ID {get;set;}
        
        [Key]
        [Required]
        public int Group_ID {get;set;}

        public Group Group {get;set;}
        public User User {get;set;}
        /* *************** */

        //navigation properties:
        public ICollection<SharedTaskAssignment> SharedTaskAssignments {get;set;}

    }
}