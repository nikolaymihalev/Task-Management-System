using TaskMaster.Core.Contracts;
using TaskMaster.Core.Services;
using Task = TaskMaster.Infrastructure.Models.Task;

namespace TaskMaster.UnitTests
{
    [TestFixture]
    public class TaskServiceTests
    {
        private ApplicationDbContext context;
        private IRepository repository;
        private Task task;
        private static string userId = Guid.NewGuid().ToString();
        private ITaskService taskService;

        [SetUp]
        public void SetUp()
        {
            task = new Task()
            {
                Id = 1,
                Title = "Test Title",
                Description = "Test Description",
                DueTime = new DateTime(2026, 12, 13),
                Priority = "High",
                Status = "Low",
                UserId = userId
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TaskMasterDb")
                .Options;

            this.context = new ApplicationDbContext(options);

            this.repository = new Repository(this.context);

            this.repository.AddAsync(task);

            this.repository.SaveChangesAsync();

            taskService = new TaskService(this.repository);
        }

        [TearDown]
        public void TearDown()
        {
            this.context.Database.EnsureDeleted();
            this.context.Dispose();
        }
    }
}
