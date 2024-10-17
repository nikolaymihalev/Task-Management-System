using System.ComponentModel.DataAnnotations;
using TaskMaster.Core.Constants;

namespace TaskMaster.Core.Models.Task
{
    /// <summary>
    /// Model for adding or editing Task
    /// </summary>
    public class TaskFormModel
    {
        /// <summary>
        /// Unique identifier for the task
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title or name of the task
        /// </summary>
        [StringLength(Variables.TaskTitleMaxLength, 
            MinimumLength = Variables.TaskTitleMinLength, 
            ErrorMessage = Messages.StringLengthErrorMessage)]
        [Required(ErrorMessage = Messages.RequireErrorMessage)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the task
        /// </summary>
        [StringLength(Variables.TaskDescriptionMaxLength, 
            MinimumLength = Variables.TaskDescriptionMinLength, 
            ErrorMessage = Messages.StringLengthErrorMessage)]
        [Required(ErrorMessage = Messages.RequireErrorMessage)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The deadline for completing the task
        /// </summary>
        [Required(ErrorMessage = Messages.RequireErrorMessage)]
        public DateTime DueTime { get; set; }

        /// <summary>
        /// The time when the task was completed
        /// </summary>
        public DateTime CompletedTime { get; set; }

        /// <summary>
        /// Priority level of the task (Low, Medium, High)
        /// </summary>
        [Required(ErrorMessage = Messages.RequireErrorMessage)]
        public int Priority { get; set; }

        /// <summary>
        /// Current status of the task (To Do, In Progress, Completed)
        /// </summary>
        [Required(ErrorMessage = Messages.RequireErrorMessage)]
        public int Status { get; set; }

        /// <summary>
        /// Foreign key that links the task to a specific user
        /// </summary>
        public string UserId { get; set; } = string.Empty;
    }
}
