using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using _netCoreBackend.Models.Enums;
using _netCoreBackend.Models.Objects;

namespace _netCoreBackend.Models
{
    public abstract class Task
    {
        [Key]
        [Required]
        public int SharedTaskId {get;set;}
        public string Title{get;set;}

        [Column(TypeName = "varchar(15)")]
        public string CreatedDate {get;set;}

        #nullable enable
        [MaxLength(500)]
        public string? Description{get;set;}
        public Status? Status {get;set;}

        [Column(TypeName = "jsonb")]
        public List<Checklist>? Checklists{get;set;}
    }
}