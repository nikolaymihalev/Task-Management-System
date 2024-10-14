namespace TaskMaster.Core.Enums
{
    /// <summary>
    /// Enum representing the status of a task.
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// Task is yet to be started.
        /// </summary>
        ToDo,

        /// <summary>
        /// Task is currently in progress.
        /// </summary>
        InProgress,

        /// <summary>
        /// Task has been completed.
        /// </summary>
        Completed
    }
}
