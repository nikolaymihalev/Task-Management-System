using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using TaskMaster.Controllers;
using TaskMaster.Core.Contracts;
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

            _userController.ControllerContext = CreateController();

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

            _userController.ControllerContext = CreateController();

            var result = await _userController.MyTasks(testCurrentPage) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(expectedModel, result.Model);
            _taskServiceMock.Verify(service => service.GetTasksForPageAsync(userId, testCurrentPage), Times.Once);
        }

        private ControllerContext CreateController()
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
