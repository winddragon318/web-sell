using MyWebApp.Shared.Models;
namespace MyWebApp.API.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task<bool> EmailExistsAsync(string email);
    }
}