using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskMaster.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
    }
}
