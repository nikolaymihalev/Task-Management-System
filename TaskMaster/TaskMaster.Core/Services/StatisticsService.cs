using Microsoft.EntityFrameworkCore;
using TaskMaster.Core.Contracts;
using TaskMaster.Core.Enums;
using TaskMaster.Core.Models.User;
using TaskMaster.Infrastructure.Common;
using Task = TaskMaster.Infrastructure.Models.Task;

namespace TaskMaster.Core.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IRepository repository;

        public StatisticsService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task<StatisticsModel> GetStatisticsAsync(string userId)
        {
            var tasks = await repository.AllReadonly<Task>()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if (tasks.Any()) 
            {
                int totalTasksCount = tasks.Count();
                int completedTasksCount = tasks.Count(t => t.Status == "Completed");
                int pendingTasksCount = tasks.Count(t => t.Status != "Completed");
                int overdueTasksCount = tasks.Count(t => t.DueTime < DateTime.Now && t.Status != "Completed");

                double completionRate = totalTasksCount > 0 ? (double)completedTasksCount / totalTasksCount * 100 : 0;

                int tasksComBeforeDeadCount = 0;
                int tasksComAfterDeadCount = 0;

                var completedTasks = tasks.Where(x => x.Status == "Completed");

                foreach (var task in completedTasks)
                {
                    if (task.CompletedTime <= task.DueTime)
                        tasksComBeforeDeadCount++;
                    else
                        tasksComAfterDeadCount++;
                }

                Dictionary<string, int> tasksByPriority = new Dictionary<string, int>();
                var enumPriorityNames = Enum.GetNames(typeof(TaskPriority));

                Dictionary<string, int> tasksByStatus = new Dictionary<string, int>();
                var enumStatusNames = Enum.GetNames(typeof(Enums.TaskStatus));

                foreach (var enumValue in enumPriorityNames)
                {
                    tasksByPriority[enumValue] = 0;
                }

                foreach (var enumValue in enumStatusNames)
                {
                    tasksByStatus[enumValue] = 0;
                }

                foreach (var task in tasks)
                {
                    if(tasksByPriority.Keys.Contains(task.Priority))
                        tasksByPriority[task.Priority]++;
                }

                foreach (var task in tasks)
                {
                    if (tasksByStatus.Keys.Contains(task.Status))
                        tasksByStatus[task.Status]++;
                }

                int yearNow = DateTime.Now.Year;
                string[] monthsArray = new string[] {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};

                Dictionary<string, int> taskCompletedPastYear = new Dictionary<string, int>();
                Dictionary<string, int> taskCompletedForAllTime = new Dictionary<string, int>();

                foreach(var month in monthsArray)
                {
                    if (!taskCompletedPastYear.Keys.Contains(month))
                        taskCompletedPastYear[month] = 0;

                    if (!taskCompletedForAllTime.Keys.Contains(month))
                        taskCompletedForAllTime[month] = 0;
                }


                foreach(var task in completedTasks)
                {
                    string currentMonth = monthsArray[task.CompletedTime.Month + 1];

                    if (task.CompletedTime.Year == yearNow)
                    {
                        taskCompletedPastYear[currentMonth]++;
                    }
                    else
                    {
                        taskCompletedForAllTime[currentMonth]++;
                    }
                }

                return new StatisticsModel()
                {
                    TaskCompletionRate = completionRate,
                    PendingTasksCount = pendingTasksCount,
                    CompletedTasksCount = completedTasksCount,
                    TasksCompletedBeforeDeadline = tasksComBeforeDeadCount,
                    TasksCompletedAfterDeadline = tasksComAfterDeadCount,
                    OverdueTasksCount = overdueTasksCount,
                    TasksByPriority = tasksByPriority,
                    TasksByStatus = tasksByStatus,
                    TasksCompletedPastYear = taskCompletedPastYear,
                    TasksCompletedAllTime = taskCompletedForAllTime
                };
            }

            return new StatisticsModel();
        }
    }
}
