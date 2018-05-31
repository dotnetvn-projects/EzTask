using System;

namespace EzTask.Entity.Data
{
    public class AccountInfo : Entity<AccountInfo>
    {
        public int AccountId { get; set; }
        public string JobTitle { get; set; }
        public string Education { get; set; }
        public Account Account { get; set; }
        public byte[] DisplayImage { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Comment { get; set; }
        public string Introduce { get; set; }
        public byte[] Document { get; set; }
        public bool IsPublished { get; set; }

        public override void Update(AccountInfo entity)
        {
            var displayImage = DisplayImage;
            base.Update(entity);
            DisplayImage = displayImage;
        }
    }
}
