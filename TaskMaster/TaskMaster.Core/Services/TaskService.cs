using Microsoft.EntityFrameworkCore;
using TaskMaster.Core.Constants;
using TaskMaster.Core.Contracts;
using TaskMaster.Core.Enums;
using TaskMaster.Core.Models.Task;
using TaskMaster.Infrastructure.Common;
using TaskStatus = TaskMaster.Core.Enums.TaskStatus;

namespace TaskMaster.Core.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepository repository;

        public TaskService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task<IEnumerable<TaskInfoModel>> GetAllTasksAsync(string userId)
        {
            return await repository.AllReadonly<Infrastructure.Models.Task>()
                .Where(x => x.UserId == userId)
                .Select(x => new TaskInfoModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    DueTime = x.DueTime.ToString("dd/MM/yyyy"),
                    Priority = x.Priority.ToString(),
                    Status = x.Status.ToString(),
                    UserId = userId
                })
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task<TaskInfoModel> GetTaskByIdAsync(int id)
        {
            Infrastructure.Models.Task? model = null;

            try
            {
                model = await GetByIdAsync(id);
            }
            catch (Exception)
            {
                throw new ArgumentException(Messages.OperationFailedErrorMessage);
            }

            return new TaskInfoModel()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                DueTime = model.DueTime.ToString("dd/MM/yyyy"),
                Priority = model.Priority.ToString(),
                Status = model.Status.ToString(),
                UserId = model.UserId
            };
        }

        public async Task<TaskPageModel> GetTasksForPageAsync(string userId, int currentPage = 1)
        {
            var taskPageModel = new TaskPageModel();
            int formula = (currentPage - 1) * Variables.MaxTasksPerPage;

            if (currentPage <= 1)
            {
                formula = 0;
            }

            taskPageModel.Tasks = await GetAllTasksAsync(userId);
            taskPageModel.PagesCount = Math.Ceiling(taskPageModel.Tasks.Count() / Convert.ToDouble(Variables.MaxTasksPerPage));

            taskPageModel.Tasks = taskPageModel.Tasks
               .Skip(formula)
               .Take(Variables.MaxTasksPerPage);

            taskPageModel.CurrentPage = currentPage;

            return taskPageModel;
        }

        public async Task AddAsync(TaskFormModel model)
        {
            var task = new Infrastructure.Models.Task()
            {
                Title = model.Title,
                Description = model.Description,
                DueTime = model.DueTime,
                Priority = ((TaskPriority)model.Priority).ToString(),
                Status = ((TaskStatus)model.Status).ToString(),
                UserId = model.UserId
            };

            try
            {
                await repository.AddAsync(task);
                await repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ArgumentException(Messages.OperationFailedErrorMessage);
            }
        }

        public async Task EditAsync(TaskFormModel model)
        {
            try
            {
                var task = await GetByIdAsync(model.Id);

                task.Title = model.Title;
                task.Description = model.Description;
                task.DueTime = model.DueTime;
                task.Priority = ((TaskPriority)model.Priority).ToString();
                task.Status = ((TaskStatus)model.Status).ToString();
            }
            catch (Exception)
            {
                throw new ArgumentException(Messages.OperationFailedErrorMessage);
            }            

            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var task = await GetByIdAsync(id);
            }
            catch (Exception)
            {
                throw new ArgumentException(Messages.OperationFailedErrorMessage);
            }

            await repository.DeleteAsync<Infrastructure.Models.Task>(id);
        }

        private async Task<Infrastructure.Models.Task> GetByIdAsync(int id)
        {
            var task = await repository.GetByIdAsync<Infrastructure.Models.Task>(id);

            if (task == null)
                throw new ArgumentException(Messages.DoesntExistErrorMessage);

            return task;
        }
    }
}
