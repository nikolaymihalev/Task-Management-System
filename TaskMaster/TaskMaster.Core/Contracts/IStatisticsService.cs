using TaskMaster.Core.Models.User;

namespace TaskMaster.Core.Contracts
{
    public interface IStatisticsService
    {
        Task<StatisticsModel> GetStatisticsAsync(string userId);
    }
}
