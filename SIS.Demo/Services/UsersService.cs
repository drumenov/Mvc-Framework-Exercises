using IRunesWebApp.Data;
using IRunesWebApp.Services;
using SIS.Demo.Services.Contracts;
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
            return this.dbContext.Users.Any(u => u.Username == username && u.Password == password.Hash());
        }
    }
}