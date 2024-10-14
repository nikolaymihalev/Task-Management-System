using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskMaster.Infrastructure.Models
{
    /// <summary>
    /// Represents a notification sent to a user in the system
    /// Notifications inform users of task assignments, updates, etc
    /// </summary>
    [Comment("Represents a notification sent to a user")]
    public class Notification
    {
        /// <summary>
        /// Unique identifier for the notification
        /// </summary>
        [Key]
        [Comment("Unique identifier for the notification")]
        public int Id { get; set; }

        /// <summary>
        /// The message or content of the notification
        /// </summary>
        [Required]
        [Comment("The message of the notification")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// The date when the notification was sent
        /// </summary>
        [Comment("The date when the notification was sent")]
        public DateTime DateSent { get; set; }

        /// <summary>
        /// Foreign key linking the notification to a specific user
        /// </summary>
        [Required]
        [Comment("User identifier")]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property to the user who received the notification
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;
    }
}
