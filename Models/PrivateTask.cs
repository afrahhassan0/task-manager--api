
using System.Text.Json.Serialization;

namespace _netCoreBackend.Models
{
    public class PrivateTask:Task
    {
        public PrivateTask() {}
     
        //Foreign key and reference navigation property:
        [JsonIgnore]
        public int OwnerId {get;set;}
        [JsonIgnore]
        public User User {get;set;}        
        
    }
}