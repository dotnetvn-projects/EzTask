using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzTask.Entity.Data
{
    public class ToDoItem : Entity<ToDoItem>
    {
        public Guid ManagedCode { get; set; }
        public string Title { get; set; }
        public short Priority { get; set; }
        public short Status { get; set; }
        public DateTime CompleteOn { get; set; }
        public int Owner { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedDate { get; set; }

        [ForeignKey("Owner")]
        public Account Account { get; set; }
    }
}
