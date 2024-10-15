using System.ComponentModel.DataAnnotations;
using TaskMaster.Core.Constants;

namespace TaskMaster.Core.Models.Notification
{
    /// <summary>
    /// Model for adding or editing Notification
    /// </summary>
    public class NotificationFormModel
    {
        /// <summary>
        /// Unique identifier for the notification
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The message or content of the notification
        /// </summary>
        [Required(ErrorMessage = Messages.RequireErrorMessage)]
        [StringLength(Variables.NotificationMessageMaxLength, 
            MinimumLength = Variables.NotificationMessageMinLength, 
            ErrorMessage = Messages.StringLengthErrorMessage)]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// The date when the notification was sent
        /// </summary>
        public DateTime DateSent { get; set; }

        /// <summary>
        /// Foreign key linking the notification to a specific user
        /// </summary>
        public string UserId { get; set; } = string.Empty;
    }
}
