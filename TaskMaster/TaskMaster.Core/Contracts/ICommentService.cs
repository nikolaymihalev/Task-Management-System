using TaskMaster.Core.Models.Comment;

namespace TaskMaster.Core.Contracts
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentInfoModel>> GetCommentsPerTaskAsync(int id);
        Task AddAsync(CommentFormModel model);
        Task EditAsync(CommentFormModel model);
        Task<CommentInfoModel> GetCommentByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
