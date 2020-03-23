using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace _netCoreBackend.Models
{
    public class SharedTasks: Task
    {
        public SharedTasks(){}

        [Column(TypeName = "varchar(15)")]
        public string Deadline {get;set;}

        #nullable enable
        public string?[] AdminComments {get;set;}
        /* ************** */
        
        [JsonIgnore]
        public string AdminUsername {get;set;}
        [JsonIgnore]
        public Credentials AdminAccount {get;set;}

        //navigation properties:
        public ICollection<SharedTaskAssignment> SharedTaskAssignments {get;set;}
         
    }
}