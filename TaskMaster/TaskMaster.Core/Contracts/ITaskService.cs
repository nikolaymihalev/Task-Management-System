using TaskMaster.Core.Models.Task;

namespace TaskMaster.Core.Contracts
{
    /// <summary>
    /// Defines methods for adding, retrieving, editing, and deleting tasks
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Retrieves a paginated list of tasks for a given user, helping manage large task sets by returning only tasks for the specified page
        /// </summary>
        /// <param name="userId">The ID of the user whose tasks are being retrieved</param>
        /// <param name="currentPage">The current page number (default is 1)</param>
        /// <returns>A TaskPageModel containing the tasks for the specified page</returns>
        Task<TaskPageModel> GetTasksForPageAsync(string userId, int currentPage = 1);

        /// <summary>
        /// Retrieves all tasks for a specific user
        /// </summary>
        /// <param name="userId">The ID of the user whose tasks are being retrieved</param>
        /// <returns>An IEnumerable<TaskInfoModel> containing all tasks for the user</returns>
        Task<IEnumerable<TaskInfoModel>> GetAllTasksAsync(string userId);

        /// <summary>
        /// Adds a new task to the system using the provided task details
        /// </summary>
        /// <param name="model">A TaskFormModel containing the details of the task to be added</param>
        Task AddAsync(TaskFormModel model);

        /// <summary>
        /// Edits an existing task based on the provided task details
        /// </summary>
        /// <param name="model">A TaskFormModel containing the updated details of the task</param>
        Task EditAsync(TaskFormModel model);

        /// <summary>
        /// Retrieves the details of a specific task by its ID
        /// </summary>
        /// <param name="id">The ID of the task to be retrieved</param>
        /// <returns>A TaskInfoModel containing the task's details</returns>
        Task<TaskInfoModel> GetTaskByIdAsync(int id);

        /// <summary>
        /// Deletes a task from the system by its ID
        /// </summary>
        /// <param name="id">The ID of the task to be deleted</param>
        Task DeleteAsync(int id);
    }
}
