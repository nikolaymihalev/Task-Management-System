using TaskMaster.Infrastructure.Models;

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

        [TearDown]
        public void TearDown()
        {
            this.context.Database.EnsureDeleted();
            this.context.Dispose();
        }
    }
}
