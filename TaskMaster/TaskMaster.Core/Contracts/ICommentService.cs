using TaskMaster.Core.Models.Comment;

namespace TaskMaster.Core.Contracts
{
    /// <summary>
    /// Defines methods for retrieving, adding, editing, and deleting comments
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// Retrieves all comments associated with a specific task
        /// </summary>
        /// <param name="id">The ID of the task whose comments are being retrieved</param>
        /// <returns>An IEnumerable<CommentInfoModel> containing the comments related to the task</returns>
        Task<IEnumerable<CommentInfoModel>> GetCommentsPerTaskAsync(int id);

        /// <summary>
        /// Adds a new comment to the system based on the provided comment details
        /// </summary>
        /// <param name="model">A CommentFormModel containing the details of the comment to be added</param>
        Task AddAsync(CommentFormModel model);

        /// <summary>
        /// Edits an existing comment based on the provided details
        /// </summary>
        /// <param name="model">A CommentFormModel containing the updated details of the comment</param>
        Task EditAsync(CommentFormModel model);

        /// <summary>
        /// Retrieves the details of a specific comment by its ID
        /// </summary>
        /// <param name="id">The ID of the comment to be retrieved</param>
        /// <returns>A CommentInfoModel containing the comment's details</returns>
        Task<CommentInfoModel> GetCommentByIdAsync(int id);

        /// <summary>
        /// Deletes a comment from the system by its ID
        /// </summary>
        /// <param name="id">The ID of the comment to be deleted</param>
        Task DeleteAsync(int id);
    }
}
