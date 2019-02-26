using System.ComponentModel.DataAnnotations;

namespace EzTask.Model
{
    public class RegisterModel : AccountModel
    {
        [Required, StringLength(maximumLength: 50, MinimumLength = 6,
            ErrorMessage = "Password Temp must be a string between 6 and 50 characters")]
        public string PasswordTemp { get; set; }

        public bool IsAcceptPolicy { get; set; }

        public RegisterModel()
        {
            IsAcceptPolicy = true;
        }
    }
}
