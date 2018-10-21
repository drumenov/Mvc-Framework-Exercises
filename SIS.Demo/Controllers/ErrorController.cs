using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Controllers.Base;

namespace SIS.Demo.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index() {
            return View();
        }
    }
}
