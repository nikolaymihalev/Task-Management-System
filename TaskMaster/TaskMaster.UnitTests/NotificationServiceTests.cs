﻿using TaskMaster.Core.Contracts;
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

        [TearDown]
        public void TearDown()
        {
            this.context.Database.EnsureDeleted();
            this.context.Dispose();
        }
    }
}
