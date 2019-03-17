using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EzTask.Entity.Data
{
    public class ToDoItem: Entity<ToDoItem>
    {
        public string Title { get; set; }
        public short Status { get; set; }
        public DateTime CompleteOn { get; set; }
        public int Owner { get; set; }

        [ForeignKey("Owner")]
        public Account Account { get; set; }
    }
}
