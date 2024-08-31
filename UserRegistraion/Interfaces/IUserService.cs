using UserRegistraion.Entities;
using UserRegistraion.ViewModels;

namespace UserRegistraion.Interfaces
{
    public interface IUserService
    {
        Task<User?> CreateUser(CreateUserRequest request);
        Task<bool> UserExists(int nic);
        Task<User?> GetUserById(Guid userId);
        Task<User?> GetUserByNic(int nic);
        Task<bool> ConfirmPolicies(Guid userId, bool isConfirmed);
        Task<bool> SavePasscode(Guid userId, string passCode);
        Task<bool> ConfirmPasscode(Guid userId, string passCode);
        Task<bool> SaveBiometricStaus(Guid userId, bool biometricStatus);
    }
}
