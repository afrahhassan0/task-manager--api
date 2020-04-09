namespace _netCoreBackend.Models
{
    public class GroupDTO
    {
        public int GroupId { get; set; }
        public string Title { get; set; }
        
        #nullable enable
        public string? Description { get; set; }
    }
}