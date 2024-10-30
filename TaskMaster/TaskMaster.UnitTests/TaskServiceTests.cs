using Microsoft.IdentityModel.Tokens;
using TaskMaster.Core.Constants;
using TaskMaster.Core.Contracts;
using TaskMaster.Core.Services;

namespace TaskMaster.UnitTests
{
    [TestFixture]
    public class TaskServiceTests
    {
        private ApplicationDbContext context;
        private IRepository repository;
        private TaskMaster.Infrastructure.Models.Task task;
        private TaskMaster.Infrastructure.Models.Task task2;
        private static string userId = Guid.NewGuid().ToString();
        private ITaskService taskService;

        [SetUp]
        public void SetUp()
        {
            task = new TaskMaster.Infrastructure.Models.Task()
            {
                Id = 1,
                Title = "Test Title",
                Description = "Test Description",
                DueTime = new DateTime(2026, 12, 13),
                Priority = "High",
                Status = "ToDo",
                UserId = userId
            };

            task2 = new TaskMaster.Infrastructure.Models.Task()
            {
                Id = 2,
                Title = "Test Title 2",
                Description = "Test Description 2",
                DueTime = new DateTime(2024, 10, 14),
                Priority = "Medium",
                Status = "Completed",
                CompletedTime = new DateTime(2025, 12, 10),
                UserId = userId
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TaskMasterDb")
                .Options;

            this.context = new ApplicationDbContext(options);

            this.repository = new Repository(this.context);

            this.repository.AddAsync(task);
            this.repository.AddAsync(task2);

            this.repository.SaveChangesAsync();

            taskService = new TaskService(this.repository);
        }

        [Test]
        public async Task Test_GetAllTasksAsyncShouldReturnList()
        {
            int exListCount = 2;
            
            var list = await taskService.GetAllTasksAsync(userId);

            Assert.IsTrue(exListCount == list.Count());
            Assert.True(list.SequenceEqual(list.OrderByDescending(x => x.Id)));
        }

        [Test]
        public async Task Test_GetAllTasksAsyncShouldReturnEmptyList()
        {
            int exListCount = 0;

            var list = await taskService.GetAllTasksAsync(" ");

            Assert.IsTrue(exListCount == list.Count());
            Assert.True(list.IsNullOrEmpty());
        }

        [Test]
        public async Task Test_GetTaskByIdAsyncShouldReturnModel()
        {
            string exTitle = "Test Title";
            string exDescription = "Test Description";
            string exDueTime = "13.12.2026";
            string exPriority = "High";
            string exStatus = "ToDo";
            string exCompletedTime = "-";

            var actualTask = await taskService.GetTaskByIdAsync(task.Id);

            Assert.IsNotNull(actualTask);
            Assert.IsTrue(exTitle == actualTask.Title);
            Assert.IsTrue(exDescription == actualTask.Description);
            Assert.IsTrue(exDueTime == actualTask.DueTime); 
            Assert.IsTrue(exPriority == actualTask.Priority);
            Assert.IsTrue(exStatus == actualTask.Status);
            Assert.IsTrue(exCompletedTime == actualTask.CompletedTime);
            Assert.IsTrue(userId == actualTask.UserId);
        }

        [Test]
        public void Test_GetTaskByIdAsyncShouldThrowException()
        {
            string exException = Messages.OperationFailedErrorMessage;

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await taskService.GetTaskByIdAsync(50));

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
