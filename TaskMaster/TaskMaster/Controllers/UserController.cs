using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskMaster.Core.Contracts;
using TaskMaster.Core.Models.Task;
using TaskMaster.Core.Models.User;

namespace TaskMaster.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IStatisticsService statisticsService;
        private readonly ITaskService taskService;

        public UserController(
            UserManager<IdentityUser> _userManager, 
            SignInManager<IdentityUser> _signInManager,
            IStatisticsService _statisticsService,
            ITaskService _taskService)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            statisticsService = _statisticsService;
            taskService = _taskService;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var model = await statisticsService.GetStatisticsAsync(User.Id());

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyTasks(int currentPage = 1)
        {
            var model = await taskService.GetTasksForPageAsync(User.Id(), currentPage);

            return View(model);
        }

        [HttpGet]
        public IActionResult NewTask()
        {
            var model = new TaskFormModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> NewTask(TaskFormModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            try
            {
                await taskService.AddAsync(model);
            }
            catch (Exception)
            {
                return View(model);
            }

            return RedirectToAction(nameof(MyTasks));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
                return RedirectToAction("Index", "Home");

            var model = new RegisterViewModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new IdentityUser()
            {
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                UserName = model.UserName,
                NormalizedUserName = model.UserName.ToUpper(),
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
                return RedirectToAction(nameof(Login));
            }

            foreach(var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
                return RedirectToAction("Index", "Home");

            var model = new LoginViewModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid login");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
