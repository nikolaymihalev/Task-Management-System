namespace TaskMaster.Core.Models.Notification
{
    /// <summary>
    /// Model for page with Notifications
    /// </summary>
    public class NotificationPageModel
    {
        /// <summary>
        /// Collection of Notifications for page
        /// </summary>
        public IEnumerable<NotificationInfoModel> Notifications { get; set; } = new List<NotificationInfoModel>();

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
