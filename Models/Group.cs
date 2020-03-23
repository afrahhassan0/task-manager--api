using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace _netCoreBackend.Models
{
    public class Group
    {
        //group properties:
        public Group(){}

        [Key]
        [Required]
        public int GroupId {get;set;}
        
        [Required]
        [MaxLength(25)]
        public string title{get;set;}
        [Required]
        [Column(TypeName="varchar(10)")]
        public string CreatedDate {get;set;}
        #nullable enable
        [MaxLength(500)]
        public string? Description {get;set;}
        /* *********************** */

        //foreign key and navigation properties
        [Required]
        [JsonIgnore]
        public string AdminUsername {get;set;}
        [JsonIgnore]
        public Credentials AdminAccount {get;set;} 


        //Membership:
        [JsonIgnore]
        public ICollection<UserGroup> Memberships {get;set;}
    }

}