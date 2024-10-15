namespace TaskMaster.Core.Models.Comment
{
    /// <summary>
    /// Model for Comment information
    /// </summary>
    public class CommentInfoModel
    {
        /// <summary>
        /// Unique identifier for the comment
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The content or message of the comment
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// The date when the comment was posted
        /// </summary>
        public string DateSent { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key linking the comment to a specific task
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// Foreign key linking the comment to the user who posted it
        /// </summary>
        public string UserId { get; set; } = string.Empty;
    }
}
