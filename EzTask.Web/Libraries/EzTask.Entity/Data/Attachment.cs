using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzTask.Entity.Data
{
    public class Attachment : Entity<Attachment>
    {
        public int TaskId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileData { get; set; }
        public DateTime AddedDate { get; set; }
        public int AddedUser { get; set; }
        public TaskItem Task { get; set; }

        [ForeignKey("AddedUser")]
        public Account User { get; set; }
    }
}
