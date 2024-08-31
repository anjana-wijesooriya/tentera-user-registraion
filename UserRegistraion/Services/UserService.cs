using Microsoft.EntityFrameworkCore;
using System;
using UserRegistraion.Context;
using UserRegistraion.Entities;
using UserRegistraion.Interfaces;
using UserRegistraion.ViewModels;

namespace UserRegistraion.Services
{
    public class UserService : IUserService
    {
        private readonly TenteraDbContext _context;
        private readonly IOtpService _otpService;

        public UserService(TenteraDbContext context, IOtpService otpService)
        {
            _context = context;
            _otpService = otpService;
        }

        /// Retrieves a User entity by their NIC. The User entity with the given NIC, or null if no such user exists.
        public async Task<User?> GetUserByNic(int nic)
        {
            var user = await _context.Users.SingleOrDefaultAsync(a => a.NIC == nic);
            return user;
        }

        /// Retrieves a User entity by their ID. The User entity with the given ID, or null if no such user exists.
        public async Task<User?> GetUserById(Guid userId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(a => a.Id == userId);
            return user;
        }

        /// Creates a User entity. The entity is created, and its ID is set.
        /// The entity is then saved to the database. If the save is successful,
        /// an OTP is sent via the OTP service, and the User entity is returned
        /// with the OTP data. If the save fails, null is returned.
        public async Task<User?> CreateUser(CreateUserRequest request)
        {
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Mobile = request.Mobile,
                NIC = request.NIC
            };
            _context.Users.Add(newUser);
            var response = await _context.SaveChangesAsync();

            if(response > -1)
            {
                newUser = await _otpService.SendOtp(newUser);
                return newUser;
            } else
            {
                return null;
            }
        }

        /// Confirms or rejects the policies for the user with the given ID.
        /// True if the user was found and the policies were confirmed/rejected, false otherwise.
        public async Task<bool> ConfirmPolicies(Guid userId, bool isConfirmed)
        {
            var user = await GetUserById(userId);
            if (user == null)
            {
                return false;
            } else
            {
                user.HasAcceptedPolicies = isConfirmed;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return true;
            }
        }

        /// Saves a passcode for a user with the given ID.
        /// True if the passcode was saved successfully, false otherwise.
        public async Task<bool> SavePasscode(Guid userId, string passCode)
        {
            var user = await GetUserById(userId);
            if (user == null)
            {
                return false;
            }
            else
            {
                user.PassCode = passCode;
                _context.Users.Update(user);
                var response = await _context.SaveChangesAsync();

                if (response > -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// Confirms a passcode for a user with the given ID.
        /// True if the passcode matches the saved passcode, false otherwise.
        public async Task<bool> ConfirmPasscode(Guid userId, string passCode)
        {
            var user = await GetUserById(userId);
            if (user == null)
            {
                return false;
            }
            else
            {

                if (user.PassCode == passCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// Saves a biometric status for a user with the given ID.
        /// True if the biometric status was saved successfully, false otherwise.True if the biometric status was saved successfully, false otherwise
        public async Task<bool> SaveBiometricStaus(Guid userId, bool biometricStatus)
        {
            var user = await GetUserById(userId);
            if (user == null)
            {
                return false;
            }
            else
            {
                user.EnableBiometric = biometricStatus;
                _context.Users.Update(user);
                var response = await _context.SaveChangesAsync();

                if (response > -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// Checks if a user with the given NIC exists.True if the user exists, false otherwise
        public async Task<bool> UserExists(int nic)
        {
            return await _context.Users.AnyAsync(e => e.NIC == nic);
        }
    }
}
