using WebAppRazor.DTOs;

namespace WebAppRazor.Services
{
    public interface IAuthService
    {
        Task<Token?> GenerateTokenAsync(string userName, string password);
    }
}