using TaskMaster.Core.Models.User;

namespace TaskMaster.Core.Contracts
{
    /// <summary>
    /// Provides methods to calculate and retrieve user statistics
    /// </summary>
    public interface IStatisticsService
    {
        /// <summary>
        /// Retrieves the statistics data for a specific user
        /// </summary>
        /// <param name="userId">The ID of the user whose statistics are being retrieved</param>
        /// <returns>A StatisticsModel containing the statistics data for a specific user</returns>
        Task<StatisticsModel> GetStatisticsAsync(string userId);
    }
}
