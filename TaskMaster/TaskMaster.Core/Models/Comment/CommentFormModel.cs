using System.ComponentModel.DataAnnotations;
using TaskMaster.Core.Constants;

namespace TaskMaster.Core.Models.Comment
{
    /// <summary>
    /// Model for adding or editing Comment
    /// </summary>
    public class CommentFormModel
    {
        /// <summary>
        /// Unique identifier for the comment
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The content or message of the comment
        /// </summary>
        [Required(ErrorMessage = Messages.RequireErrorMessage)]
        [StringLength(Variables.CommentContentMaxLength, 
            MinimumLength = Variables.CommentContentMinLength, 
            ErrorMessage = Messages.StringLengthErrorMessage)]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// The date when the comment was posted
        /// </summary>
        [Required(ErrorMessage = Messages.RequireErrorMessage)]
        public DateTime DateSent { get; set; }

        /// <summary>
        /// Foreign key linking the comment to a specific task
        /// </summary>
        [Required(ErrorMessage = Messages.RequireErrorMessage)]
        public int TaskId { get; set; }

        /// <summary>
        /// Foreign key linking the comment to the user who posted it
        /// </summary>
        public string UserId { get; set; } = string.Empty;
    }
}
