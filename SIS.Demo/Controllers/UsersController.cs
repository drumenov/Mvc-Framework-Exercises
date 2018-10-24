using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes;
using SIS.Framework.Controllers.Base;
using SIS.Demo.Services.Contracts;
using SIS.Demo.ViewModels;
using SIS.Framework.Security.Contracts;
using SIS.Framework.Security;
using SIS.Demo.Models;

namespace SIS.Demo.Controllers
{
    public class UsersController : Controller
    {
        private IUsersService usersService;

        public UsersController(IUsersService usersService) {
            this.usersService = usersService;
        }

        public IActionResult Login() {
            if (!this.Request.Session.ContainsParameter("auth")) {
                return this.View();
            } else {
                this.FillViewModel(nameof(this.Identity.Email), this.Identity.Email);
                return this.View("Index");
            }
            
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model) {
            if (this.ModelState.IsValid.HasValue && ModelState.IsValid.Value && usersService.ExistsByUsernameAndPassword(model.Email, model.Password)) {
                this.FillViewModel(nameof(model.Email), model.Email);
                User user = this.usersService.GetUserByEmail(model.Email);
                IIdentity identity = new IdentityUser();
                identity.Email = model.Email;
                identity.Username = user.Username;
                this.SignIn(identity);
                return this.View("Index");
            } else {
                return this.Login();
            }
        }

        public IActionResult Register() => this.View();

        [HttpPost]
        public IActionResult Register(RegisterViewModel model) {
            IActionResult result = this.RedirectToAction("/users/register/");
            if (this.ModelState.IsValid.HasValue && this.ModelState.IsValid.Value) {
                if (this.usersService.TryRegisterUser(model)) {
                    this.FillViewModel(nameof(model.Email), model.Email);
                    result = this.RedirectToAction("/Home/IndexLoggedIn");
                }
            }
            return result;
        }

        public IActionResult Logout() {
            this.SignOut();
            return this.RedirectToAction("/");
        }

        private void FillViewModel(string key, object value) {
            this.ViewModel[key] = value;
        }
    }
}

