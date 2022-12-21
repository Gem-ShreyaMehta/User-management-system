using Microsoft.AspNetCore.Mvc;

namespace LoginPageViaRepositoryPattern.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Add()
        {
            return View(Add);
        }
    }
}
