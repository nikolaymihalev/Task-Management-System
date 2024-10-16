using System.ComponentModel.DataAnnotations;
using TaskMaster.Core.Constants;

namespace TaskMaster.Core.Models.User
{
    /// <summary>
    /// ViewModel for user registration.
    /// Contains the user's username, email, password, and a confirmation password field 
    /// required during the account registration process.
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// The username that the user will register with
        /// </summary>
        [Required(ErrorMessage = Messages.RequireErrorMessage)]
        [StringLength(Variables.UserNameMaxLength,
            MinimumLength = Variables.UserNameMinLength,
            ErrorMessage = Messages.StringLengthErrorMessage)]
        public string UserName { get; set; } = null!;

        /// <summary>
        /// The user's email address used for account registration
        /// </summary>
        [Required(ErrorMessage = Messages.RequireErrorMessage)]
        [EmailAddress]
        [StringLength(Variables.UserEmailMaxLength, 
            MinimumLength = Variables.UserEmailMinLength, 
            ErrorMessage = Messages.StringLengthErrorMessage)]
        public string Email { get; set; } = null!;

        /// <summary>
        /// The user's password used for account registration and future authentication
        /// </summary>
        [Required(ErrorMessage = Messages.RequireErrorMessage)]
        [StringLength(Variables.UserPassMaxLength,
            MinimumLength = Variables.UserPassMinLength,
            ErrorMessage = Messages.StringLengthErrorMessage)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        /// <summary>
        /// A field to confirm the user's password, ensuring the password is entered
        /// </summary>
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
