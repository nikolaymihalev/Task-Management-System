using TaskMaster.Core.Models.Notification;

namespace TaskMaster.Core.Contracts
{
    public interface INotificationService
    {
        Task<NotificationPageModel> GetNotificationsForPageAsync(string userId, int currentPage = 1);
        Task<IEnumerable<NotificationInfoModel>> GetAllNotificationsAsync(string userId);
        Task AddAsync(NotificationFormModel model);
        Task<NotificationInfoModel> GetNotificationByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
