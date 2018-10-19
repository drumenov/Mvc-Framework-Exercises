using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes;
using SIS.Framework.Controllers.Base;
using SIS.Demo.Services.Contracts;

namespace IRunesWebApp.Controllers
{
    public class UsersController : Controller {
        private IUsersService usersService;

        public UsersController(IUsersService usersService) {
            this.usersService = usersService;
        }

        public IActionResult Login() => this.View();

        [HttpPost]
        public IActionResult Login(LoginViewModel model) {
            if (!ModelState.IsValid.HasValue || !ModelState.IsValid.Value) {
                this.RedirectToAction("/users/login");
            }
            return null;
        }
    }    
}

