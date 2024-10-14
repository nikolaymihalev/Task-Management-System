using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskMaster.Infrastructure.Models
{
    /// <summary>
    /// Represents a comment added to a task
    /// Comments allow users to discuss task progress, feedback, or issues
    /// </summary>
    [Comment("Represents a comment added to a task")]
    public class Comment
    {
        /// <summary>
        /// Unique identifier for the comment
        /// </summary>
        [Key]
        [Comment("Unique identifier for the comment")]
        public int Id { get; set; }

        /// <summary>
        /// The content or message of the comment
        /// </summary>
        [Required]
        [Comment("The content of the comment")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// The date when the comment was posted
        /// </summary>
        [Required]
        [Comment("The date when the comment was posted")]
        public DateTime DateSent { get; set; }

        /// <summary>
        /// Foreign key linking the comment to a specific task
        /// </summary>
        [Required]
        [Comment("Task identifier")]
        public int TaskId { get; set; }

        /// <summary>
        /// Foreign key linking the comment to the user who posted it
        /// </summary>
        [Required]
        [Comment("User identifier")]
        public string UserId { get; set; } = string.Empty;


        [ForeignKey(nameof(TaskId))]
        public Task Task { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;
    }
}
