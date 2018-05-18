using System;

namespace EzTask.Interfaces.Models
{
    public interface IAccountInfoModel : IModel
    {
        int AccountInfoId { get; set; }
        string JobTitle { get; set; }
        string Education { get; set; }
        byte[] DisplayImage { get; set; }
        string Email { get; set; }
        string Address1 { get; set; }
        string Address2 { get; set; }
        string PhoneNumber { get; set; }
        DateTime? BirthDay { get; set; }
        string Comment { get; set; }
        string Introduce { get; set; }
        byte[] Document { get; set; }
        string Skills { get; set; }
        bool IsPublished { get; set; }
    }
}
