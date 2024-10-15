namespace TaskMaster.Core.Models.Task
{
    /// <summary>
    /// Model for page with Tasks
    /// </summary>
    public class TaskPageModel
    {
        /// <summary>
        /// Collection of Tasks for page
        /// </summary>
        public IEnumerable<TaskInfoModel> Tasks { get; set; } = new List<TaskInfoModel>();

        /// <summary>
        /// Pages count
        /// </summary>
        public double PagesCount { get; set; }

        /// <summary>
        /// Current page
        /// </summary>
        public int CurrentPage { get; set; }
    }
}
