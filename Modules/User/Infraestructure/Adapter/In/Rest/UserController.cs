using Microsoft.AspNetCore.Mvc;

namespace liminal_api.Modules.User.Infraestructure.Adapter.In.Rest
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
