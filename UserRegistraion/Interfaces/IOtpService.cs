using UserRegistraion.Entities;

namespace UserRegistraion.Interfaces
{
    public interface IOtpService
    {
        Task<User?> SendOtp(User user);
        Task<bool> VerifyMobileOtp(Guid userId, string pinCode);
        Task<bool> VerifyEmailOtp(Guid userId, string pinCode);
    }
}
