using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using TaskMaster.Controllers;
using TaskMaster.Core.Contracts;
using TaskMaster.Core.Models.Notification;
using TaskMaster.Core.Models.Task;
using TaskMaster.Core.Models.User;
using Xunit;
using Assert = Xunit.Assert;
using Task = System.Threading.Tasks.Task;

namespace TaskMaster.IntegrationTests
{
    public class UserControllerTests
    {
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<SignInManager<IdentityUser>> _signInManagerMock;
        private readonly Mock<IStatisticsService> _statisticsServiceMock;
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly UserController _userController;
        private static readonly string userId = "bd68a836-f988-4dea-af8d-ad33664480af";

        public UserControllerTests()
        {
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                new Mock<IUserStore<IdentityUser>>().Object, null, null, null, null, null, null, null, null
            );

            _signInManagerMock = new Mock<SignInManager<IdentityUser>>(
                _userManagerMock.Object, new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object, null, null, null, null
            );

            _statisticsServiceMock = new Mock<IStatisticsService>();
            _taskServiceMock = new Mock<ITaskService>();
            _notificationServiceMock = new Mock<INotificationService>();

            _userController = new UserController(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _statisticsServiceMock.Object,
                _taskServiceMock.Object,
                _notificationServiceMock.Object
            );
        }

        [Fact]
        public async Task Test_DashboardReturnsViewWithModel()
        {
            var expectedModel = new StatisticsModel();

            _statisticsServiceMock
                .Setup(service => service.GetStatisticsAsync(It.IsAny<string>()))
                .ReturnsAsync(expectedModel);

            _userController.ControllerContext = CreateControllerContext();

            var result = await _userController.Dashboard() as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(expectedModel, result.Model);
            _statisticsServiceMock.Verify(service => service.GetStatisticsAsync(userId), Times.Once);
        }

        [Fact]
        public async Task Test_MyTasksReturnsViewWithModel()
        {
            int testCurrentPage = 1;
            var expectedModel = new TaskPageModel();

            _taskServiceMock
                .Setup(service => service.GetTasksForPageAsync(It.IsAny<string>(), testCurrentPage))
                .ReturnsAsync(expectedModel);

            _userController.ControllerContext = CreateControllerContext();

            var result = await _userController.MyTasks(testCurrentPage) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(expectedModel, result.Model);
            _taskServiceMock.Verify(service => service.GetTasksForPageAsync(userId, testCurrentPage), Times.Once);
        }

        [Fact]
        public void Test_NewTaskGetReturnsViewWithNewModel()
        {
            var result = _userController.NewTask() as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            var model = result.Model as TaskFormModel;
            Assert.NotNull(model);
            Assert.Equal(DateTime.Now.Date, model.DueTime.Date);
        }

        [Fact]
        public async Task Test_NewTaskPostValidModelRedirectsToMyTasks()
        {
            var model = new TaskFormModel();
            _taskServiceMock.Setup(service => service.AddAsync(model)).Returns(Task.CompletedTask);
            _notificationServiceMock.Setup(service => service.AddAsync(It.IsAny<NotificationFormModel>())).Returns(Task.CompletedTask);

            _userController.ControllerContext = CreateControllerContext();

            var result = await _userController.NewTask(model) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(_userController.MyTasks), result.ActionName);
            Assert.Equal(userId, model.UserId);
            Assert.Equal(0, model.Status);
            _taskServiceMock.Verify(service => service.AddAsync(model), Times.Once);
            _notificationServiceMock.Verify(service => service.AddAsync(It.Is<NotificationFormModel>(n => n.UserId == userId && n.Message == "Added new task!")), Times.Once);
        }

        [Fact]
        public async Task Test_NewTaskPostInvalidModelReturnsViewWithModel()
        {
            var model = new TaskFormModel();

            _userController.ModelState.AddModelError("Title", "Title is required");

            var result = await _userController.NewTask(model) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(model, result.Model);
        }

        [Fact]
        public async Task Test_NewTaskPostExceptionReturnsViewWithModel()
        {
            var model = new TaskFormModel();
            _taskServiceMock.Setup(service => service.AddAsync(model)).ThrowsAsync(new Exception());

            _userController.ControllerContext = CreateControllerContext();

            var result = await _userController.NewTask(model) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(model, result.Model);
        }

        [Fact]
        public async Task Test_NotificationsReturnsViewWithModel()
        {
            int currentPage = 1; 
            var expectedModel = new NotificationPageModel();

            _notificationServiceMock
                .Setup(service => service.GetNotificationsForPageAsync(userId, currentPage))
                .ReturnsAsync(expectedModel);

            _userController.ControllerContext = CreateControllerContext();

            var result = await _userController.Notifications(currentPage) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(expectedModel, result.Model);
            _notificationServiceMock.Verify(service => service.GetNotificationsForPageAsync(userId, currentPage), Times.Once);
        }

        [Fact]
        public async Task Test_NotificationsReturnsViewWithModelForDifferentPage()
        {
            int currentPage = 2;
            var expectedModel = new NotificationPageModel();

            _notificationServiceMock
                .Setup(service => service.GetNotificationsForPageAsync(userId, currentPage))
                .ReturnsAsync(expectedModel);

            _userController.ControllerContext = CreateControllerContext();

            var result = await _userController.Notifications(currentPage) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(expectedModel, result.Model);
            _notificationServiceMock.Verify(service => service.GetNotificationsForPageAsync(userId, currentPage), Times.Once);
        }

        [Fact]
        public async Task Test_TaskReturnsViewWithModelWhenTaskBelongsToUser()
        {
            int taskId = 1;

            var expectedModel = new TaskInfoModel { Id = taskId, UserId = userId };
            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId))
                .ReturnsAsync(expectedModel);

            _userController.ControllerContext = CreateControllerContext();

            var result = await _userController.Task(taskId) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(expectedModel, result.Model);
        }

        [Fact]
        public async Task Test_TaskThrowsArgumentExceptionWhenTaskDoesNotBelongToUser()
        {
            int taskId = 1;

            var expectedModel = new TaskInfoModel { Id = taskId, UserId = "DifferentUserId" };
            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId))
                .ReturnsAsync(expectedModel);

            _userController.ControllerContext = CreateControllerContext();

            var result = await _userController.Task(taskId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(UserController.MyTasks), result.ActionName);
        }

        [Fact]
        public async Task Test_TaskRedirectsToMyTasksWhenExceptionOccurs()
        {
            int taskId = 1;

            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId))
                .ThrowsAsync(new System.Exception("Database error"));

            _userController.ControllerContext = CreateControllerContext();

            var result = await _userController.Task(taskId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(UserController.MyTasks), result.ActionName);
        }

        [Fact]
        public async Task Test_RemoveSuccessfullyRemovesTaskAndAddsNotification()
        {
            int taskId = 1;

            var taskModel = new TaskInfoModel { Id = taskId, UserId = userId };
            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId))
                .ReturnsAsync(taskModel);
            _taskServiceMock.Setup(service => service.DeleteAsync(taskId)).Returns(Task.CompletedTask);

            _userController.ControllerContext = CreateControllerContext();

            var result = await _userController.Remove(taskId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nameof(UserController.MyTasks), result.ActionName);
            _taskServiceMock.Verify(service => service.DeleteAsync(taskId), Times.Once);
            _notificationServiceMock.Verify(service => service.AddAsync(It.IsAny<NotificationFormModel>()), Times.Once);
        }

        [Fact]
        public async Task Test_RemoveDoesNotRemoveTaskIfNotBelongsToUser()
        {
            int taskId = 1;

            var taskModel = new TaskInfoModel { Id = taskId, UserId = "DifferentUserId" };
            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId))
                .ReturnsAsync(taskModel);

            _userController.ControllerContext = CreateControllerContext();

            var result = await _userController.Remove(taskId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(UserController.MyTasks), result.ActionName);
            _taskServiceMock.Verify(service => service.DeleteAsync(It.IsAny<int>()), Times.Never);
            _notificationServiceMock.Verify(service => service.AddAsync(It.IsAny<NotificationFormModel>()), Times.Never);
        }

        [Fact]
        public async Task Test_RemoveAddsNotificationAndRedirectsToNotificationsWhenExceptionOccurs()
        {
            int taskId = 1;

            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId))
                .ThrowsAsync(new System.Exception("Database error"));

            _userController.ControllerContext = CreateControllerContext();

            var result = await _userController.Remove(taskId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(UserController.Notifications), result.ActionName);
            _notificationServiceMock.Verify(service => service.AddAsync(It.IsAny<NotificationFormModel>()), Times.Once);
        }

        private ControllerContext CreateControllerContext()
        {
            var userClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));

            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaimsPrincipal }
            };
        }
    }
}
