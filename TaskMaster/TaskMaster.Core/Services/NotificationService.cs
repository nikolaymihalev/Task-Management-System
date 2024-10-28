using Microsoft.EntityFrameworkCore;
using TaskMaster.Core.Constants;
using TaskMaster.Core.Contracts;
using TaskMaster.Core.Models.Notification;
using TaskMaster.Infrastructure.Common;
using TaskMaster.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskMaster.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository repository;

        public NotificationService(IRepository _repository)
        {
            repository = _repository;   
        }

        public async Task AddAsync(NotificationFormModel model)
        {
            var notification = new Notification()
            {
                Message = model.Message,
                DateSent = model.DateSent,
                UserId = model.UserId,
            };

            try
            {
                await repository.AddAsync(notification);
                await repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ArgumentException(Messages.OperationFailedErrorMessage);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var notification = await GetByIdAsync(id);
            }
            catch (Exception)
            {
                throw new ArgumentException(Messages.OperationFailedErrorMessage);
            }

            await repository.DeleteAsync<Notification>(id);
            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<NotificationInfoModel>> GetAllNotificationsAsync(string userId)
        {
            return await repository.AllReadonly<Notification>()
                .Where(x => x.UserId == userId)
                .Select(x => new NotificationInfoModel()
                {
                    Id = x.Id,
                    Message = x.Message,
                    DateSent = x.DateSent.ToString("dd/MM/yyyy"),
                    UserId = userId
                })
                .ToListAsync();
        }

        public async Task<NotificationPageModel> GetNotificationsForPageAsync(string userId, int currentPage = 1)
        {
            var notificationPageModel = new NotificationPageModel();
            int formula = (currentPage - 1) * Variables.MaxNotificationsPerPage;

            if (currentPage <= 1)
            {
                formula = 0;
            }

            notificationPageModel.Notifications = await GetAllNotificationsAsync(userId);
            notificationPageModel.PagesCount = Math.Ceiling(notificationPageModel.Notifications.Count() / Convert.ToDouble(Variables.MaxNotificationsPerPage));

            notificationPageModel.Notifications = notificationPageModel.Notifications
               .Skip(formula)
               .Take(Variables.MaxNotificationsPerPage);

            notificationPageModel.CurrentPage = currentPage;

            return notificationPageModel;
        }

        public async Task<NotificationInfoModel> GetNotificationByIdAsync(int id)
        {
            Notification? notification = null;
            try
            {
                notification = await GetByIdAsync(id);
            }
            catch (Exception)
            {
                throw new ArgumentException(Messages.OperationFailedErrorMessage);
            }

            return new NotificationInfoModel()
            {
                Id = notification.Id,
                Message = notification.Message,
                DateSent = notification.DateSent.ToString("dd/MM/yyyy"),
                UserId = notification.UserId
            };
        }

        private async Task<Notification> GetByIdAsync(int id) 
        {
            var notification = await repository.GetByIdAsync<Notification>(id);

            if (notification == null)
                throw new ArgumentException(Messages.DoesntExistErrorMessage);

            return notification;
        }
    }
}
