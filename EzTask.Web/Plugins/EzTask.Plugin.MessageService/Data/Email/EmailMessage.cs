using EzTask.Interface.SharedData;

namespace EzTask.Plugin.MessageService.Data.Email
{
    public class EmailMessage : IMessage
    {
        public string To { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
