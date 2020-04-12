using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace _netCoreBackend.Models
{
    public class SharedTasks: Task
    {
        public SharedTasks(){}

        public int GroupId { get; set; }

        [JsonIgnore]
        public Group Group { get; set; }
        
         
    }
}