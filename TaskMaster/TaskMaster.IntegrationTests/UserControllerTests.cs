using Microsoft.AspNetCore.Identity;
using Moq;
using TaskMaster.Controllers;

namespace TaskMaster.IntegrationTests
{
    public class UserControllerTests
    {
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<SignInManager<IdentityUser>> _signInManagerMock;
        private readonly UserController _userController;
    }
}
