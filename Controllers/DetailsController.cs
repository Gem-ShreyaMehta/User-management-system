using Microsoft.AspNetCore.Mvc;

namespace LoginPageViaRepositoryPattern.Controllers
{
    public class DetailsController : Controller
    {
        public IActionResult UserDetails()
        {
            return View();
        }
    }
}
