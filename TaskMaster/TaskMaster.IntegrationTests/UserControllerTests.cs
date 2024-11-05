using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using TaskMaster.Controllers;
using TaskMaster.Core.Constants;
using TaskMaster.Core.Contracts;
using TaskMaster.Core.Enums;
using TaskMaster.Core.Models.Notification;
using TaskMaster.Core.Models.Task;
using TaskMaster.Core.Models.User;
using Xunit;
using Assert = Xunit.Assert;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
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

        [Fact]
        public async Task Test_RemoveNotificationSuccessfullyDeletesNotification()
        {
            int notificationId = 1;
            _notificationServiceMock.Setup(service => service.DeleteAsync(notificationId)).Returns(Task.CompletedTask);

            var result = await _userController.RemoveNotification(notificationId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(UserController.Notifications), result.ActionName);
            _notificationServiceMock.Verify(service => service.DeleteAsync(notificationId), Times.Once);
        }

        [Fact]
        public async Task Test_RemoveNotificationAddsFailureNotificationAndRedirectsWhenExceptionOccurs()
        {
            int notificationId = 1;
            _userController.ControllerContext = CreateControllerContext();

            _notificationServiceMock.Setup(service => service.DeleteAsync(notificationId))
                .ThrowsAsync(new Exception("Deletion error"));

            var result = await _userController.RemoveNotification(notificationId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(UserController.Notifications), result.ActionName);
            _notificationServiceMock.Verify(service => service.AddAsync(It.Is<NotificationFormModel>(
                n => n.Message == Messages.OperationFailedErrorMessage &&
                     n.UserId == userId
            )), Times.Once);
        }

        [Fact]
        public async Task Test_EditGetReturnsViewWithModelWhenTaskExistsAndBelongsToUser()
        {
            int taskId = 1;
            _userController.ControllerContext = CreateControllerContext();

            var task = new TaskInfoModel
            {
                Id = taskId,
                UserId = userId,
                Title = "Test Task",
                Description = "Task description",
                Status = "ToDo",
                Priority = "High",
                DueTime = DateTime.Now.ToString()
            };

            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId))
                .ReturnsAsync(task);

            // Act
            var result = await _userController.Edit(taskId) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<TaskFormModel>(result.Model);
            var model = result.Model as TaskFormModel;
            Assert.Equal(taskId, model.Id);
            Assert.Equal(task.Title, model.Title);
            Assert.Equal(task.Description, model.Description);
            Assert.Equal((int)Enum.Parse(typeof(Core.Enums.TaskStatus), task.Status), model.Status);
            Assert.Equal((int)Enum.Parse(typeof(TaskPriority), task.Priority), model.Priority);
        }

        [Fact]
        public async Task Test_EditGetRedirectsToMyTasksWhenExceptionOccurs()
        {
            int taskId = 1;
            _userController.ControllerContext = CreateControllerContext();

            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _userController.Edit(taskId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(UserController.MyTasks), result.ActionName);
        }

        [Fact]
        public async Task Test_EditGetReturnsUnauthorizedWhenTaskDoesNotBelongToUser()
        {
            int taskId = 1;
            _userController.ControllerContext = CreateControllerContext();

            var task = new TaskInfoModel
            {
                Id = taskId,
                UserId = "OtherUserId"
            };

            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId))
                .ReturnsAsync(task);

            var result = await _userController.Edit(taskId);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Test_EditPostSuccessfullyEditsTaskWhenModelStateIsValid()
        {
            int taskId = 1;
            _userController.ControllerContext = CreateControllerContext();

            var task = new TaskInfoModel
            {
                Id = taskId,
                UserId = userId
            };

            var model = new TaskFormModel
            {
                Id = taskId,
                Title = "Updated Task",
                Description = "Updated Description",
                Status = 1,
                Priority = 1
            };

            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId))
                .ReturnsAsync(task);

            _taskServiceMock.Setup(service => service.EditAsync(model)).Returns(Task.CompletedTask);

            var result = await _userController.Edit(model, taskId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(UserController.MyTasks), result.ActionName);
            _taskServiceMock.Verify(service => service.EditAsync(model), Times.Once);
        }

        [Fact]
        public async Task Test_EditPostRedirectsToMyTasksWhenExceptionOccurs()
        {
            int taskId = 1;
            _userController.ControllerContext = CreateControllerContext();

            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId))
                .ThrowsAsync(new Exception("Database error"));

            var model = new TaskFormModel { Id = taskId };

            var result = await _userController.Edit(model, taskId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(UserController.MyTasks), result.ActionName);
        }

        [Fact]
        public async Task Test_EditPostReturnsUnauthorizedWhenTaskDoesNotBelongToUser()
        {
            int taskId = 1;
            _userController.ControllerContext = CreateControllerContext();

            var task = new TaskInfoModel
            {
                Id = taskId,
                UserId = "OtherUserId"
            };

            var model = new TaskFormModel { Id = taskId };

            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId))
                .ReturnsAsync(task);

            var result = await _userController.Edit(model, taskId);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Test_EditPostReturnsViewWithModelWhenModelStateIsInvalid()
        {
            int taskId = 1;
            _userController.ControllerContext = CreateControllerContext();

            var model = new TaskFormModel
            {
                Id = taskId,
                Title = "Invalid Task",
                UserId = userId
            };

            _userController.ModelState.AddModelError("Error", "Invalid model");

            var result = await _userController.Edit(model, taskId) as ViewResult;

            Assert.Null(result);
        }

        [Fact]
        public async Task Test_UpdateGetReturnsViewWithModelWhenTaskExistsAndBelongsToUser()
        {
            int taskId = 1;
            _userController.ControllerContext = CreateControllerContext();

            var task = new TaskInfoModel
            {
                Id = taskId,
                UserId = userId,
                Status = "ToDo",
                Priority = "High"
            };

            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId))
                .ReturnsAsync(task);

            var result = await _userController.Update(taskId) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<TaskFormModel>(result.Model);
            var model = result.Model as TaskFormModel;
            Assert.Equal(taskId, model.Id);
            Assert.Equal((int)Enum.Parse(typeof(Core.Enums.TaskStatus), task.Status), model.Status);
            Assert.Equal((int)Enum.Parse(typeof(TaskPriority), task.Priority), model.Priority);
        }

        [Fact]
        public async Task Test_UpdateGetRedirectsToMyTasksWhenExceptionOccurs()
        {
            int taskId = 1;
            _userController.ControllerContext = CreateControllerContext();

            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _userController.Update(taskId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(UserController.MyTasks), result.ActionName);
        }

        [Fact]
        public async Task Test_UpdateGetReturnsUnauthorizedWhenTaskDoesNotBelongToUser()
        {
            int taskId = 1;
            _userController.ControllerContext = CreateControllerContext();

            var task = new TaskInfoModel
            {
                Id = taskId,
                UserId = "OtherUserId"
            };

            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId))
                .ReturnsAsync(task);

            var result = await _userController.Update(taskId);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Test_UpdatePostSuccessfullyUpdatesTaskWhenTaskBelongsToUser()
        {
            int taskId = 1;
            _userController.ControllerContext = CreateControllerContext();

            var task = new TaskInfoModel
            {
                Id = taskId,
                UserId = userId
            };

            var model = new TaskFormModel
            {
                Id = taskId,
                Status = 1,
                Priority = 1
            };

            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId))
                .ReturnsAsync(task);

            _taskServiceMock.Setup(service => service.UpdateAsync(model)).Returns(Task.CompletedTask);

            var result = await _userController.Update(model, taskId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(UserController.MyTasks), result.ActionName);
            _taskServiceMock.Verify(service => service.UpdateAsync(model), Times.Once);
        }

        [Fact]
        public async Task Test_UpdatePostRedirectsToMyTasksWhenExceptionOccurs()
        {
            int taskId = 1;
            _userController.ControllerContext = CreateControllerContext();

            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId))
                .ThrowsAsync(new Exception("Database error"));

            var model = new TaskFormModel { Id = taskId };

            var result = await _userController.Update(model, taskId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(UserController.MyTasks), result.ActionName);
        }

        [Fact]
        public async Task Test_UpdatePostReturnsUnauthorizedWhenTaskDoesNotBelongToUser()
        {
            int taskId = 1;
            _userController.ControllerContext = CreateControllerContext();

            var task = new TaskInfoModel
            {
                Id = taskId,
                UserId = "OtherUserId"
            };

            var model = new TaskFormModel { Id = taskId };

            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId))
                .ReturnsAsync(task);

            var result = await _userController.Update(model, taskId);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void Test_RegisterGetReturnsViewWithModelWhenUserIsNotAuthenticated()
        {
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            var result = _userController.Register() as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<RegisterViewModel>(result.Model);
        }

        [Fact]
        public async Task Test_RegisterPostReturnsViewWithModelWhenModelStateIsInvalid()
        {
            var model = new RegisterViewModel();
            _userController.ModelState.AddModelError("Email", "Required");

            var result = await _userController.Register(model) as ViewResult;

            Assert.NotNull(result);
            Assert.Same(model, result.Model);
        }        

        [Fact]
        public async Task Test_RegisterPostReturnsViewWithErrorsWhenRegistrationFails()
        {
            var model = new RegisterViewModel
            {
                Email = "test@example.com",
                UserName = "testuser",
                Password = "Test@123"
            };

            var identityError = new IdentityError { Description = "Password too weak" };
            var failedResult = IdentityResult.Failed(identityError);

            _userManagerMock.Setup(manager => manager.CreateAsync(It.IsAny<IdentityUser>(), model.Password))
                .ReturnsAsync(failedResult);

            var result = await _userController.Register(model) as ViewResult;

            Assert.NotNull(result);
            Assert.Same(model, result.Model);
            Assert.True(_userController.ModelState.ContainsKey(""));
            Assert.Equal("Password too weak", _userController.ModelState[""].Errors[0].ErrorMessage);
            _userManagerMock.Verify(manager => manager.CreateAsync(It.IsAny<IdentityUser>(), model.Password), Times.Once);
        }

        [Fact]
        public void Test_LoginGetReturnsViewWithModelWhenUserIsNotAuthenticated()
        {
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            var result = _userController.Login() as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<LoginViewModel>(result.Model);
        }

        [Fact]
        public async Task Test_LoginPostReturnsViewWithModelWhenModelStateIsInvalid()
        {
            var model = new LoginViewModel();
            _userController.ModelState.AddModelError("Email", "Required");

            var result = await _userController.Login(model) as ViewResult;

            Assert.NotNull(result);
            Assert.Same(model, result.Model);
        }

        [Fact]
        public async Task Test_LoginPostRedirectsToHomeWhenLoginIsSuccessful()
        {
            var model = new LoginViewModel
            {
                Email = "test@example.com",
                Password = "Test@123"
            };
            var user = new IdentityUser { Email = model.Email };

            _userManagerMock.Setup(manager => manager.FindByEmailAsync(model.Email))
                .ReturnsAsync(user);

            _signInManagerMock.Setup(manager => manager.PasswordSignInAsync(user, model.Password, false, false))
                .ReturnsAsync(SignInResult.Success);

            var result = await _userController.Login(model) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
            _userManagerMock.Verify(manager => manager.FindByEmailAsync(model.Email), Times.Once);
            _signInManagerMock.Verify(manager => manager.PasswordSignInAsync(user, model.Password, false, false), Times.Once);
        }

        [Fact]
        public async Task Test_LoginPostReturnsViewWithErrorWhenLoginFails()
        {
            var model = new LoginViewModel
            {
                Email = "test@example.com",
                Password = "WrongPassword"
            };
            var user = new IdentityUser { Email = model.Email };

            _userManagerMock.Setup(manager => manager.FindByEmailAsync(model.Email))
                .ReturnsAsync(user);

            _signInManagerMock.Setup(manager => manager.PasswordSignInAsync(user, model.Password, false, false))
                .ReturnsAsync(SignInResult.Failed);

            var result = await _userController.Login(model) as ViewResult;

            Assert.NotNull(result);
            Assert.Same(model, result.Model);
            Assert.True(_userController.ModelState.ContainsKey(""));
            Assert.Equal("Invalid login", _userController.ModelState[""].Errors[0].ErrorMessage);
            _userManagerMock.Verify(manager => manager.FindByEmailAsync(model.Email), Times.Once);
            _signInManagerMock.Verify(manager => manager.PasswordSignInAsync(user, model.Password, false, false), Times.Once);
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
