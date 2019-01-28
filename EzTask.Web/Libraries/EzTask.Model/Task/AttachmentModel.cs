using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Model
{
    public class AttachmentModel : BaseModel
    {
        public int AttatchmentId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public DateTime AddedDate { get; set; }
        public AccountModel User { get; set; }
        public TaskItemModel Task { get; set; }
        public byte[] FileData { get; set; }

        public AttachmentModel()
        {
            User = new AccountModel();
            Task = new TaskItemModel();
        }
    }
}
