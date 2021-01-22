using System.Threading.Tasks;
using ExtraBlog.Models;

namespace ExtraBlog.Auth
{
    public interface IAuthentication //UserController -Login, Register HTTPPost
    {
        Task<User> Login(string username, string password); //JWT token, Unauthorized

        Task<User> Register(User user, string password, string passportNumber); //

        Task<bool> UserExists(string username); //true, false
    }
}
