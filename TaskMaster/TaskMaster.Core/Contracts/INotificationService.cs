using TaskMaster.Core.Models.Notification;

namespace TaskMaster.Core.Contracts
{
    /// <summary>
    /// Defines methods for retrieving, adding, and deleting notifications
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Retrieves a paginated list of notifications for a specific user
        /// </summary>
        /// <param name="userId">The ID of the user whose notifications are being retrieved</param>
        /// <param name="currentPage">The page number for pagination (default is 1)</param>
        /// <returns>A NotificationPageModel containing the notifications for the specified page</returns>
        Task<NotificationPageModel> GetNotificationsForPageAsync(string userId, int currentPage = 1);

        /// <summary>
        /// Retrieves all notifications for a specific user
        /// </summary>
        /// <param name="userId">The ID of the user whose notifications are being retrieved</param>
        /// <returns>An IEnumerable<NotificationInfoModel> containing all notifications for the user</returns>
        Task<IEnumerable<NotificationInfoModel>> GetAllNotificationsAsync(string userId);

        /// <summary>
        /// Adds a new notification to the system using the provided notification details
        /// </summary>
        /// <param name="model">A NotificationFormModel containing the details of the notification to be added</param>
        Task AddAsync(NotificationFormModel model);

        /// <summary>
        /// Retrieves the details of a specific notification by its ID
        /// </summary>
        /// <param name="id">The ID of the notification to be retrieved</param>
        /// <returns>A NotificationInfoModel containing the notification's details</returns>
        Task<NotificationInfoModel> GetNotificationByIdAsync(int id);

        /// <summary>
        /// Deletes a notification from the system by its ID
        /// </summary>
        /// <param name="id">The ID of the notification to be deleted</param>
        Task DeleteAsync(int id);
    }
}
