using WebApi_HomeWork_3.Entitys;

namespace WebApi_HomeWork_3.Data.Abstract
{
    public interface IAutoRepository
    {

        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);


    }
}
