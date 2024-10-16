using System.ComponentModel.DataAnnotations;
using TaskMaster.Core.Constants;

namespace TaskMaster.Core.Models.User
{
    /// <summary>
    /// ViewModel for user login.
    /// Contains the user's email and password required for authentication during the login process.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// The user's email address used for login authentication
        /// </summary>
        [Required(ErrorMessage = Messages.RequireErrorMessage)]
        public string Email { get; set; } = null!;

        /// <summary>
        /// The user's password used for login authentication
        /// </summary>
        [Required(ErrorMessage = Messages.RequireErrorMessage)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
