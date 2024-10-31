using System.Threading.Tasks;
using TaskMaster.Core.Contracts;
using TaskMaster.Core.Models.Notification;
using TaskMaster.Core.Services;
using TaskMaster.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskMaster.UnitTests
{
    [TestFixture]
    public class NotificationServiceTests
    {
        private ApplicationDbContext context;
        private IRepository repository;
        private INotificationService notificationService;
        private Notification not;
        private Notification not2;
        private static string userId = Guid.NewGuid().ToString();

        [SetUp]
        public void SetUp()
        {
            not = new Notification()
            {
                Id = 1,
                Message = "Test Message",
                DateSent = new DateTime(2024, 10, 11),
                UserId = userId,
            };

            not2 = new Notification()
            {
                Id = 2,
                Message = "Test Message 2",
                DateSent = new DateTime(2024, 11, 12),
                UserId = userId,
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "TaskMasterDb")
               .Options;

            this.context = new ApplicationDbContext(options);

            this.repository = new Repository(this.context);

            this.repository.AddAsync(not);
            this.repository.AddAsync(not2);

            this.repository.SaveChangesAsync();

            notificationService = new NotificationService(this.repository);
        }

        [Test]
        public async Task Test_AddAsyncShouldAddModel()
        {
            int exNotificationsCount = 3;
            string exMessage = "Test Message 3";
            string exSentTime = "30.10.2024";

            var notification = new NotificationFormModel()
            {
                Id = 3,
                Message = "Test Message 3",
                DateSent = new DateTime(2024, 10, 30),
                UserId = userId,
            };

            await notificationService.AddAsync(notification);

            var allNotifications = await notificationService.GetAllNotificationsAsync(userId);
            var addedNotification = await notificationService.GetNotificationByIdAsync(notification.Id);

            Assert.IsTrue(exNotificationsCount == allNotifications.Count());
            Assert.IsNotNull(addedNotification);
            Assert.IsTrue(exMessage == addedNotification.Message);
            Assert.IsTrue(exSentTime == addedNotification.DateSent);
        }

        [Test]
        public void Test_AddAsyncShouldThrowException()
        {
            var exception = Assert.ThrowsAsync<NullReferenceException>(async () => await notificationService.AddAsync(null));

            Assert.IsNotNull(exception.Message);
        }

        [Test]
        public async Task Test_DeleteAsyncShouldDeleteModel()
        {
            int exCount = 1;

            await notificationService.DeleteAsync(not2.Id);
            await repository.SaveChangesAsync();

            var notifications = await notificationService.GetAllNotificationsAsync(userId);

            Assert.IsTrue(exCount == notifications.Count());
        }

        [Test]
        public void Test_DeleteAsyncShouldThrowException()
        {
            string exException = Messages.OperationFailedErrorMessage;

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await notificationService.DeleteAsync(50));

            Assert.IsTrue(exException == exception.Message);
        }

        [Test]
        public async Task Test_GetAllNotificationsAsyncShouldReturnList()
        {
            int exListCount = 2;

            var list = await notificationService.GetAllNotificationsAsync(userId);

            Assert.IsTrue(exListCount == list.Count());
            Assert.True(list.SequenceEqual(list.OrderByDescending(x => x.Id)));
        }

        [Test]
        public async Task Test_GetAllNotificationsAsyncShouldReturnEmptyList()
        {
            int exListCount = 0;

            var list = await notificationService.GetAllNotificationsAsync(" ");

            Assert.IsTrue(exListCount == list.Count());
            Assert.True(list.IsNullOrEmpty());
        }

        [Test]
        public async Task Test_GetNotificationsForPageShouldReturnList()
        {
            int exCurrentPage = 1;
            int exPagesCount = 1;
            int exListCount = 2;

            var notificationPageModel = await notificationService.GetNotificationsForPageAsync(userId);

            Assert.IsTrue(exCurrentPage == notificationPageModel.CurrentPage);
            Assert.IsTrue(exPagesCount == notificationPageModel.PagesCount);
            Assert.IsTrue(exListCount == notificationPageModel.Notifications.Count());
            Assert.False(notificationPageModel.Notifications.IsNullOrEmpty());
        }

        [Test]
        public async Task Test_GetNotificationsForPageShouldReturnNull()
        {
            int exCurrentPage = 2;
            int exPagesCount = 1;
            int exListCount = 0;

            var notificationPageModel = await notificationService.GetNotificationsForPageAsync(userId, 2);

            Assert.IsTrue(exCurrentPage == notificationPageModel.CurrentPage);
            Assert.IsTrue(exPagesCount == notificationPageModel.PagesCount);
            Assert.IsTrue(exListCount == notificationPageModel.Notifications.Count());
            Assert.True(notificationPageModel.Notifications.IsNullOrEmpty());
        }

        [Test]
        public async Task Test_GetNotificationByIdAsyncShouldReturnModel()
        {
            string exMessage = "Test Message 2";
            string exSentTime = "12.11.2024";

            var actualNotification = await notificationService.GetNotificationByIdAsync(not2.Id);

            Assert.IsNotNull(actualNotification);
            Assert.IsTrue(exMessage == actualNotification.Message);
            Assert.IsTrue(exSentTime == actualNotification.DateSent);
            Assert.IsTrue(userId == actualNotification.UserId);
        }

        [Test]
        public void Test_GetNotificationByIdAsyncShouldThrowException()
        {
            string exException = Messages.OperationFailedErrorMessage;

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await notificationService.GetNotificationByIdAsync(50));

            Assert.IsTrue(exException == exception.Message);
        }

        [TearDown]
        public void TearDown()
        {
            this.context.Database.EnsureDeleted();
            this.context.Dispose();
        }
    }
}
