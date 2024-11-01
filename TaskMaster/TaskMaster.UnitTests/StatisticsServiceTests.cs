namespace TaskMaster.UnitTests
{
    [TestFixture]
    public class StatisticsServiceTests
    {
        private ApplicationDbContext context;
        private IRepository repository;
        private static string userId = Guid.NewGuid().ToString();
        private IStatisticsService statisticsService;
        private Infrastructure.Models.Task task;
        private Infrastructure.Models.Task task2;
        private Infrastructure.Models.Task task3;
        private Infrastructure.Models.Task task4;

        [SetUp]
        public void SetUp() 
        {
            task = new TaskMaster.Infrastructure.Models.Task()
            {
                Id = 1,
                Title = "Test Title",
                Description = "Test Description",
                DueTime = new DateTime(2024, 12, 13),
                Priority = "Medium",
                Status = "ToDo",
                UserId = userId
            };

            task2 = new TaskMaster.Infrastructure.Models.Task()
            {
                Id = 2,
                Title = "Test Title 2",
                Description = "Test Description 2",
                DueTime = new DateTime(2024, 10, 14),
                Priority = "Low",
                Status = "Completed",
                CompletedTime = new DateTime(2024, 5, 2),
                UserId = userId
            };

            task3 = new TaskMaster.Infrastructure.Models.Task()
            {
                Id = 3,
                Title = "Test Title 3",
                Description = "Test Description 3",
                DueTime = new DateTime(2024, 5, 18),
                Priority = "High",
                Status = "Completed",
                CompletedTime = new DateTime(2024, 8, 20),
                UserId = userId
            };

            task4 = new TaskMaster.Infrastructure.Models.Task()
            {
                Id = 4,
                Title = "Test Title 4",
                Description = "Test Description 4",
                DueTime = new DateTime(2024, 6, 25),
                Priority = "High",
                Status = "InProgress",
                UserId = userId
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TaskMasterDb")
                .Options;

            this.context = new ApplicationDbContext(options);

            this.repository = new Repository(this.context);

            this.repository.AddAsync(task);
            this.repository.AddAsync(task2);
            this.repository.AddAsync(task3);
            this.repository.AddAsync(task4);

            this.repository.SaveChangesAsync();

            statisticsService = new StatisticsService(this.repository);
        }

        [Test]
        public async Task Test_GetStatisticsShouldReturnEmptyModel() 
        {
            int exCount = 0;

            var model = await statisticsService.GetStatisticsAsync(" ");

            Assert.IsTrue(exCount == model.TaskCompletionRate);
            Assert.IsTrue(exCount == model.PendingTasksCount);
            Assert.IsTrue(exCount == model.CompletedTasksCount);
            Assert.IsTrue(exCount == model.TasksCompletedBeforeDeadline);
            Assert.IsTrue(exCount == model.TasksCompletedAfterDeadline);
            Assert.IsTrue(exCount == model.OverdueTasksCount);
            Assert.IsEmpty(model.TasksByPriority);
            Assert.IsEmpty(model.TasksByStatus);
            Assert.IsEmpty(model.TasksCompletedThisYear);
            Assert.IsEmpty(model.TasksAllTime);
        }

        [TearDown]
        public void TearDown() 
        {
            this.context.Database.EnsureDeleted();
            this.context.Dispose();
        }
    }
}
