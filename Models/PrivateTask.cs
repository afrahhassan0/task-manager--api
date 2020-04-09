
using System.Text.Json.Serialization;

namespace _netCoreBackend.Models
{
    public class PrivateTask:Task
    {
        public PrivateTask() {}
     
        //Foreign key and reference navigation property:
        [JsonIgnore]
        public string OwnerId {get;set;}
        [JsonIgnore]
        public Credentials OwnerAccount {get;set;}        
        
    }
}