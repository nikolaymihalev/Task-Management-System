using Microsoft.EntityFrameworkCore;
using TaskMaster.Core.Contracts;
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

            var totalTasks = tasks.Count();
            var completedTasks = tasks.Count(t => t.Status == "Completed");
            var pendingTasks = tasks.Count(t => t.Status != "Completed");
            var overdueTasks = tasks.Count(t => t.DueTime < DateTime.Now && t.Status != "Completed");

            double completionRate = totalTasks > 0 ? (double)completedTasks / totalTasks * 100 : 0;


            return null;
        }
    }
}
