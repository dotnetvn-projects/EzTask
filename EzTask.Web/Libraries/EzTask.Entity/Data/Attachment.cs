using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Entity.Data
{
    public class Attachment : Entity<Attachment>
    {
        public int TaskId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileUrl { get; set; }
        public byte[] FileData { get; set; }
        public DateTime AddedDate { get; set; }
        public int AccountId { get; set; }

        public TaskItem Task { get; set; }
        public Account Account { get; set; }
    }
}
