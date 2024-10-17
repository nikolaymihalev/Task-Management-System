namespace TaskMaster.Core.Models.Task
{
    /// <summary>
    /// Model for Task information
    /// </summary>
    public class TaskInfoModel
    {
        /// <summary>
        /// Unique identifier for the task
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title or name of the task
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the task
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The deadline for completing the task
        /// </summary>
        public string DueTime { get; set; } = string.Empty;

        /// <summary>
        /// The time when the task was completed
        /// </summary>
        public string CompletedTime { get; set; } = string.Empty;

        /// <summary>
        /// Priority level of the task (Low, Medium, High)
        /// </summary>
        public string Priority { get; set; } = string.Empty;

        /// <summary>
        /// Current status of the task (To Do, In Progress, Completed)
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key that links the task to a specific user
        /// </summary>
        public string UserId { get; set; } = string.Empty;
    }
}
