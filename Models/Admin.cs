using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace _netCoreBackend.Models
{
    public class Admin:User
    {
        [Required]
        [MaxLength(50)]
        public string JobTitle {get;set;}
        public long? PhoneNumber {get;set;}

        /* *************** */
        
        public ICollection<Group> Groups{get;set;}
        public ICollection<SharedTasks> SharedTasks {get;set;}
    }

}