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
            string exPriority = "High";
            string exStatus = "ToDo";
            string exCompletedTime = "-";

            var actualTask = await taskService.GetTaskByIdAsync(task.Id);

            Assert.IsNotNull(actualTask);
            Assert.IsTrue(exTitle == actualTask.Title);
            Assert.IsTrue(exDescription == actualTask.Description);
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

        [Test]
        public async Task Test_GetTasksForPageShouldReturnList()
        {
            int exCurrentPage = 1;
            int exPagesCount = 1;
            int exListCount = 2;

            var taskPageModel = await taskService.GetTasksForPageAsync(userId);

            Assert.IsTrue(exCurrentPage == taskPageModel.CurrentPage);
            Assert.IsTrue(exPagesCount == taskPageModel.PagesCount);
            Assert.IsTrue(exListCount == taskPageModel.Tasks.Count());
            Assert.False(taskPageModel.Tasks.IsNullOrEmpty());
        }

        [Test]
        public async Task Test_GetTasksForPageShouldReturnNull()
        {
            int exCurrentPage = 2;
            int exPagesCount = 1;
            int exListCount = 0;

            var taskPageModel = await taskService.GetTasksForPageAsync(userId, 2);

            Assert.IsTrue(exCurrentPage == taskPageModel.CurrentPage);
            Assert.IsTrue(exPagesCount == taskPageModel.PagesCount);
            Assert.IsTrue(exListCount == taskPageModel.Tasks.Count());
            Assert.True(taskPageModel.Tasks.IsNullOrEmpty());
        }

        [Test]
        public async Task Test_AddAsyncShouldAddModel()
        {
            int exTasksCount = 3;
            string exTitle = "Test Title 3";
            string exDescription = "Test Description 3";
            string exPriority = "Low";
            string exStatus = "InProgress";
            string exCompletedTime = "-";

            var taskForAdd = new TaskFormModel()
            {
                Id = 3,
                Title = "Test Title 3",
                Description = "Test Description 3",
                DueTime = new DateTime(2023, 10, 14),
                Priority = 0,
                Status = 1,
                UserId = userId
            };

            await taskService.AddAsync(taskForAdd);

            var allTasks = await taskService.GetAllTasksAsync(userId);
            var addedTask = await taskService.GetTaskByIdAsync(taskForAdd.Id);

            Assert.IsTrue(exTasksCount == allTasks.Count());
            Assert.IsNotNull(addedTask);
            Assert.IsTrue(exTitle == addedTask.Title);
            Assert.IsTrue(exDescription == addedTask.Description);
            Assert.IsTrue(exPriority == addedTask.Priority);
            Assert.IsTrue(exStatus == addedTask.Status); 
            Assert.IsTrue(exCompletedTime == addedTask.CompletedTime);
        }

        [Test]
        public void Test_AddAsyncShouldThrowException()
        {
            var exception = Assert.ThrowsAsync<NullReferenceException>(async () => await taskService.AddAsync(null));

            Assert.IsNotNull(exception.Message);
        }

        [Test]
        public async Task Test_EditAsyncShouldEditModel()
        {
            string exTitle = "Test Title 5";
            string exDescription = "Test Description 5";

            var taskForEdit = new TaskFormModel()
            {
                Id = task2.Id,
                Title = exTitle,
                Description = exDescription,
                DueTime = new DateTime(2024, 10, 15)
            };

            await taskService.EditAsync(taskForEdit);

            var actualTask = await taskService.GetTaskByIdAsync(task2.Id);

            Assert.IsNotNull(actualTask);
            Assert.IsTrue(exTitle == actualTask.Title);
            Assert.IsTrue(exDescription == actualTask.Description);
        }

        [Test]
        public void Test_EditAsyncShouldThrowException()
        {
            string exException = Messages.OperationFailedErrorMessage;

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await taskService.EditAsync(null));

            Assert.IsTrue(exException == exception.Message);
        }

        [Test]
        public async Task Test_DeleteAsyncShouldDeleteModel()
        {
            int exCount = 1;

            await taskService.DeleteAsync(task2.Id);
            await repository.SaveChangesAsync();

            var tasks = await taskService.GetAllTasksAsync(userId);

            Assert.IsTrue(exCount == tasks.Count());
        }

        [Test]
        public void Test_DeleteAsyncShouldThrowException()
        {
            string exException = Messages.OperationFailedErrorMessage;

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await taskService.DeleteAsync(50));

            Assert.IsTrue(exException == exception.Message);
        }

        [Test]
        public async Task Test_UpdateAsyncShouldUpdateModel()
        {
            string exPriority = "Low";
            string exStatus = "InProgress";

            var taskForUpdate = new TaskFormModel()
            {
                Id = task.Id,
                Priority = 0,
                Status = 1,
            };

            await taskService.UpdateAsync(taskForUpdate);

            var actualTask = await taskService.GetTaskByIdAsync(task.Id);

            Assert.IsNotNull(actualTask);
            Assert.IsTrue(exPriority == actualTask.Priority);
            Assert.IsTrue(exStatus == actualTask.Status);
        }

        [Test]
        public void Test_UpdateAsyncShouldThrowException()
        {
            string exException = Messages.OperationFailedErrorMessage;

            var invalidTask = new TaskFormModel() { Id = 50 }; 

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await taskService.UpdateAsync(invalidTask));

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
