using IRunesWebApp.Models;
using IRunesWebApp.Services;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace IRunesWebApp.Controllers
{
    public class UsersController : Controller
    {
	private IUsersService usersService;
	
	public UsersController(IUsersService usersService){
		this.usersService = usersService;
	}

	public IActionResult Login() => this.View();
		
	    [HttpPost]
	    public IActionResult Login(LoginViewModel model) {
            
            if (!this.IsAuthenticated(request)) {
			if(!ModelState.IsValid.HasValue || !ModelState.IsValid.Value){
				this.RedirectToAction("/users/login");
			}

                if (!this.usersService.ExistsByUsernameAndPassword(model.Username, model.Password)) {
                    return new this.RedirectToAction("/users/login");
                }
                IHttpResponse response = new RedirectResult("/");

                this.SignInUser(username, response, request, user);
                return response;
            }
            else {
                return new this.RedirectToAction("/");
            }
        }

        public IHttpResponse Register(IHttpRequest request) {
            if(request.RequestMethod == HttpRequestMethod.Get && !this.IsAuthenticated(request)) {
                return this.View();
            }
            if (!this.IsAuthenticated(request)) {
                string username = request.FormData["username"].ToString().Trim();
                string hashedPassword = request.FormData["password"].ToString().Hash();
                string confirmedPasswordHash = request.FormData["confirmPassword"].ToString().Hash();

                username = WebUtility.UrlDecode(username);
                //if(String.IsNullOrWhiteSpace(username) || username.Length < 4) {
                //    return new BadRequestResult("Please, provide a valid username with length of at leaset 4 characters.", HttpResponseStatusCode.BadRequest);
                //}
                //if(this.IRunesDbContext.Users.Any(u => u.Username == username)) {
                //    return new BadRequestResult("User with the same username already exists.", HttpResponseStatusCode.BadRequest);
                //}

                if (hashedPassword != confirmedPasswordHash) {
                    return new BadRequestResult("Passwords do not match.", HttpResponseStatusCode.BadRequest);
                }

                User user = new User {
                    Email = username,
                    HashedPassword = hashedPassword,
                    Username = username
                };

                try {
                    this.IRunesDbContext.Users.Add(user);
                    this.IRunesDbContext.SaveChanges();
                }
                catch (Exception e) {
                    return new BadRequestResult(e.Message, HttpResponseStatusCode.InternalServerError);
                }
                RedirectResult response = new RedirectResult("/");
                this.SignInUser(username, response, request, user);

                return response;
            } else {
                return new RedirectResult("/");
            }
        }
    }
}
