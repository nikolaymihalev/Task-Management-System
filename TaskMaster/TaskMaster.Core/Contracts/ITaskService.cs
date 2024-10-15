using TaskMaster.Core.Models.Task;

namespace TaskMaster.Core.Contracts
{
    public interface ITaskService
    {
        Task<TaskPageModel> GetTasksForPageAsync(string userId, int currentPage = 1);
        Task<IEnumerable<TaskInfoModel>> GetAllTasksAsync(string userId);
        Task AddAsync(TaskFormModel model);
        Task EditAsync(TaskFormModel model);
        Task<TaskInfoModel> GetTaskByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
