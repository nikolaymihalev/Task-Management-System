using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskMaster.Infrastructure.Models
{
    /// <summary>
    /// Represents a task assigned to a user. 
    /// Each task has a title, description, due date, priority, and status.
    /// </summary>
    [Comment("Represents a task assigned to a user")]    
    public class Task
    {
        /// <summary>
        /// Unique identifier for the task
        /// </summary>
        [Key]
        [Comment("Identifier for the task")]
        public int Id { get; set; }

        /// <summary>
        /// Title or name of the task
        /// </summary>
        [Required]
        [Comment("Title or name of the task")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the task
        /// </summary>
        [Required]
        [Comment("Detailed description of the task")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The deadline for completing the task
        /// </summary>
        [Required]
        [Comment("The deadline for completing the task")]
        public DateTime DueTime { get; set; }

        /// <summary>
        /// The time when the task was completed
        /// </summary>
        [Comment("The time when the task was completed")]
        public DateTime CompletedTime { get; set; }

        /// <summary>
        /// Priority level of the task (Low, Medium, High)
        /// </summary>
        [Required]
        [Comment("Priority level of the task")]
        public string Priority { get; set; } = string.Empty;

        /// <summary>
        /// Current status of the task (To Do, In Progress, Completed)
        /// </summary>
        [Required]
        [Comment("Current status of the task")]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key that links the task to a specific user
        /// </summary>
        [Comment("User identifier")]
        [Required]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property to the user who owns the task
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;

        /// <summary>
        /// Collection of comments assigned to the task
        /// </summary>
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
