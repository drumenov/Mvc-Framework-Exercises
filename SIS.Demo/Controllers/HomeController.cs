using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Controllers.Base;

namespace SIS.Demo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() {
            if (!this.Request.Session.ContainsParameter("auth")) {
                return this.View();
            } else {
                return this.RedirectToAction("/users/login");
            }
        }
    }
}
