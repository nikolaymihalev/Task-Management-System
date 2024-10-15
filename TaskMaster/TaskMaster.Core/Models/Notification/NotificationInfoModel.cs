namespace TaskMaster.Core.Models.Notification
{
    /// <summary>
    /// Model for Notification information
    /// </summary>
    public class NotificationInfoModel
    {
        /// <summary>
        /// Unique identifier for the notification
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The message or content of the notification
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// The date when the notification was sent
        /// </summary>
        public string DateSent { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key linking the notification to a specific user
        /// </summary>
        public string UserId { get; set; } = string.Empty;
    }
}
