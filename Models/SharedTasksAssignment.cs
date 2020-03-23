using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _netCoreBackend.Models
{
    public class SharedTaskAssignment
    {
        public SharedTaskAssignment(){}
        
        [Key]
        [Required]
        public int SharedTaskId {get;set;}
        [Key]
        [Required]
        public string MemberShipUserUsername{get;set;}
        [Key]
        [Required]
        public int MembershipGroupId{get;set;}

        public UserGroup Membership {get;set;}
        public SharedTasks SharedTask {get;set;}
    }
}