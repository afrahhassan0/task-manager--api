using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using NpgsqlTypes;

namespace _netCoreBackend.Models
{
    public class Credentials
    {
        public Credentials() {}

        [Key]
        [MaxLength(15)]
        public string Username {get;set;}
        
        [Required]
        [MinLength(8)]
        [MaxLength(200)]
        public string Password {get;set;}

        [Required]
        [MaxLength(50)]
        public string OrganizationName {get;set;}


        //navigation properties:
        [JsonIgnore]
        public int User_Id{get;set;}
        [JsonIgnore]
        public User User{get;set;}
        
        [JsonIgnore]
        public ICollection<Group> Groups{get;set;}
        
        [JsonIgnore]
        public ICollection<SharedTasks> SharedTasks {get;set;}

        //Memberships!
        public ICollection<UserGroup> Memberships {get;set;}
        public List<PrivateTask> PrivateTasks {get;set;}
        
    }
}

