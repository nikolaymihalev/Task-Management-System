using Microsoft.EntityFrameworkCore;
using TaskMaster.Core.Constants;
using TaskMaster.Core.Contracts;
using TaskMaster.Core.Models.Comment;
using TaskMaster.Infrastructure.Common;
using TaskMaster.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskMaster.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository repository;

        public CommentService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task AddAsync(CommentFormModel model)
        {
            var comment = new Comment()
            {
                Content = model.Content,
                DateSent = model.DateSent,
                TaskId = model.TaskId,
                UserId = model.UserId,
            };

            try
            {
                await repository.AddAsync(comment);
                await repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ArgumentException(Messages.OperationFailedErrorMessage);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var comment = await repository.GetByIdAsync<Comment>(id);

            if (comment == null)
                throw new ArgumentException(Messages.DoesntExistErrorMessage);

            await repository.DeleteAsync<Comment>(id);
        }

        public async Task EditAsync(CommentFormModel model)
        {
            var comment = await repository.GetByIdAsync<Comment>(model.Id);

            if (comment == null)
                throw new ArgumentException(Messages.DoesntExistErrorMessage);

            comment.Content = model.Content;

            await repository.SaveChangesAsync();
        }

        public async Task<CommentInfoModel> GetCommentByIdAsync(int id)
        {
            var comment = await repository.GetByIdAsync<Comment>(id);

            if (comment == null)
                throw new ArgumentException(Messages.DoesntExistErrorMessage);

            return new CommentInfoModel()
            {
                Id = id,
                Content = comment.Content,
                DateSent = comment.DateSent.ToString("dd/MM/yyyy"),
                TaskId = comment.TaskId,
                UserId = comment.UserId
            };
        }

        public async Task<IEnumerable<CommentInfoModel>> GetCommentsPerTaskAsync(int id)
        {
            return await repository.AllReadonly<Comment>()
                .Where(x => x.TaskId == id)
                .Select(x => new CommentInfoModel()
                {
                    Id = x.Id,
                    Content = x.Content,
                    DateSent = x.DateSent.ToString("dd/MM/yyyy"),
                    TaskId = x.TaskId,
                    UserId = x.UserId
                })
                .ToListAsync();
        }
    }
}
