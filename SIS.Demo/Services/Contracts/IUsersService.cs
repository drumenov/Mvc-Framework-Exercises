namespace SIS.Demo.Services.Contracts
{
    public interface IUsersService
    {
        bool ExistsByUsernameAndPassword(string username, string password);
    }
}