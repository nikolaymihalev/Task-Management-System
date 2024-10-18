namespace TaskMaster.Core.Models.User
{
    /// <summary>
    /// Model for User's tasks statistics
    /// </summary>
    public class StatisticsModel
    {
        /// <summary>
        /// Percentage of tasks completed out of the total tasks created.
        /// </summary>
        public double TaskCompletionRate { get; set; }

        /// <summary>
        /// Number of tasks still pending.
        /// </summary>
        public int PendingTasksCount { get; set; }

        /// <summary>
        /// Number of tasks that are completed.
        /// </summary>
        public int CompletedTasksCount { get; set; }

        /// <summary>
        /// Number of tasks completed before the deadline.
        /// </summary>
        public int TasksCompletedBeforeDeadline { get; set; }

        /// <summary>
        /// Number of tasks completed after the deadline.
        /// </summary>
        public int TasksCompletedAfterDeadline { get; set; }

        /// <summary>
        /// Number of tasks overdue (past their deadline without being completed).
        /// </summary>
        public int OverdueTasksCount { get; set; }

        /// <summary>
        /// Number of tasks by priority level (High, Medium, Low).
        /// </summary>
        public Dictionary<string, int> TasksByPriority { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// Number of tasks by status (ToDo, InProgress, Completed).
        /// </summary>
        public Dictionary<string, int> TasksByStatus { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// Number of tasks completed for the past year.
        /// </summary>
        public Dictionary<string, int> TasksCompletedThisYear { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// Number of tasks for all time.
        /// </summary>
        public Dictionary<string, int> TasksAllTime { get; set; } = new Dictionary<string, int>();

    }
}
