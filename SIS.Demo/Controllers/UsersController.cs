using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes;
using SIS.Framework.Controllers.Base;
using SIS.Demo.Services.Contracts;
using SIS.Demo.ViewModels;

namespace SIS.Demo.Controllers
{
    public class UsersController : Controller {
        

        private IUsersService usersService;

        public UsersController(IUsersService usersService) {
            this.usersService = usersService;
        }

        public IActionResult Login() => this.View();

        [HttpPost]
        public IActionResult Login(LoginViewModel model) {

            IActionResult result = this.RedirectToAction("/users/login");

            if (!ModelState.IsValid.HasValue || !ModelState.IsValid.Value) {
                result = this.RedirectToAction("/users/login");
            }
            if(usersService.ExistsByUsernameAndPassword(model.Username, model.Password)) {
                result = this.RedirectToAction("/");
            }
            return result;
        }

        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model) {
            IActionResult result = this.RedirectToAction("/users/register/");
            if(this.ModelState.IsValid.HasValue && this.ModelState.IsValid.Value) {
                if (this.usersService.TryRegisterUser(model)) {
                    this.ViewModel[nameof(model.Email).ToLower()] = model.Email;
                    result = this.View("IndexLoggedIn");
                }
            }
            return result;
        }
    }    
}

