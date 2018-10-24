using SIS.Demo.Data;
using SIS.Demo.Models;
using SIS.Demo.Services.Contracts;
using SIS.Demo.ViewModels;
using SIS.Framework.Services;
using System.Linq;

namespace SIS.Demo.Services
{
    public class UsersService : IUsersService
    {
        private readonly IRunesDbContext dbContext;

        public UsersService(IRunesDbContext dbContext) {
            this.dbContext = dbContext;
        }

        public bool ExistsByUsernameAndPassword(string username, string password) {
            return this.dbContext.Users.Any(u => u.Username == username && u.HashedPassword == password);
        }

        public bool TryRegisterUser(RegisterViewModel model) {
            bool result = false;
            if(this.dbContext.Users.Any(u => u.Username == model.Email)) {
                return result;
            }
            else {
                User newUser = new User {
                    Username = model.Email,
                    Email = model.Email,
                    HashedPassword = model.Password
                };
                try {
                    this.dbContext.Users.Add(newUser);
                    this.dbContext.SaveChanges();
                    return true;
                }
                catch {
                    return false;
                }
            }
        }

        public User GetUserByEmail(string email) {
            return this.dbContext.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}