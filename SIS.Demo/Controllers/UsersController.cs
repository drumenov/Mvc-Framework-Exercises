using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes;
using SIS.Framework.Controllers.Base;
using SIS.Demo.Services.Contracts;
using SIS.Demo.ViewModels;

namespace SIS.Demo.Controllers
{
    public class UsersController : Controller
    {
        private IUsersService usersService;

        public UsersController(IUsersService usersService) {
            this.usersService = usersService;
        }

        public IActionResult Login() => this.View();

        [HttpPost]
        public IActionResult Login(LoginViewModel model) {

            IActionResult result = this.RedirectToAction("/users/login");

            if (this.ModelState.IsValid.HasValue && ModelState.IsValid.Value && usersService.ExistsByUsernameAndPassword(model.Email, model.Password)) {
                this.FillViewModel(nameof(model.Email), model.Email);
                result = this.View("IndexLoggedIn");
            }
            return result;
        }

        public IActionResult Register() => this.View();

        [HttpPost]
        public IActionResult Register(RegisterViewModel model) {
            IActionResult result = this.RedirectToAction("/users/register/");
            if (this.ModelState.IsValid.HasValue && this.ModelState.IsValid.Value) {
                if (this.usersService.TryRegisterUser(model)) {
                    this.FillViewModel(nameof(model.Email), model.Email);
                    result = this.View("IndexLoggedIn");
                }
            }
            return result;
        }

        public IActionResult Logout() {
            return this.RedirectToAction("/");
        }

        private void FillViewModel(string key, object value) {
            this.ViewModel[key.ToLower()] = value;
        }
    }
}

