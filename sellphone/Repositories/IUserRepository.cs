using MyWebApp.Models;
namespace MyWebApp.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task<bool> EmailExistsAsync(string email);
    }
}