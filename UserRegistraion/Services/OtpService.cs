using Microsoft.EntityFrameworkCore;
using UserRegistraion.Context;
using UserRegistraion.Interfaces;
using UserRegistraion.Entities;

namespace UserRegistraion.Services
{
    public class OtpService : IOtpService
    {
        private readonly TenteraDbContext _context;

        public OtpService(TenteraDbContext context)
        {
            _context = context;
        }
      
        // This method sends an OTP to a user for verification purposes. It takes a User object as input and returns a User object if the OTP is sent successfully, otherwise it returns null.        /// </summary>
        public async Task<User?> SendOtp(User user)
        {
            if (user == null)
            {
                return null;
            }
            else
            {
                var verificationData = await _context.Otps.SingleOrDefaultAsync(a => a.UserId == user.Id);
                if (verificationData == null)
                {
                    verificationData = new Otp
                    {
                        UserId = user.Id,
                        User = user,
                        Id = Guid.NewGuid(),
                        MobilePin = GeneratePin(),
                        EmailPin = null,
                        EmailVerified = false,
                        MobileVerified = false
                    };
                    _context.Otps.Add(verificationData);
                }
                else
                {
                    verificationData.MobilePin = GeneratePin();
                    verificationData.MobileVerified = false;
                    verificationData.EmailVerified = false;
                    _context.Otps.Update(verificationData);
                }
                var response = await _context.SaveChangesAsync();

                if (response > -1)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        /// Verifies a mobile OTP. True if the OTP is valid, false otherwise.
        public async Task<bool> VerifyMobileOtp(Guid userId, string pinCode)
        {
            var verificationData = await _context.Otps.SingleOrDefaultAsync(a => a.UserId == userId);
            if (verificationData == null)
            {
                return false;
            }

            if (verificationData.MobilePin == pinCode)
            {
                verificationData.MobileVerified = true;
                verificationData.EmailPin = GeneratePin();
                verificationData.MobilePin = null;

                _context.Otps.Update(verificationData);

                await _context.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }
        }
/// Verifies a email OTP. True if the OTP is valid, false otherwise.
        public async Task<bool> VerifyEmailOtp(Guid userId, string pinCode)
        {
            var verificationData = await _context.Otps.SingleOrDefaultAsync(a => a.UserId == userId);
            if (verificationData == null)
            {
                return false;
            }

            if (verificationData.EmailPin == pinCode)
            {
                verificationData.EmailVerified = true;
                verificationData.EmailPin = null;

                _context.Otps.Update(verificationData);

                await _context.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }
        }

        /// Generates a random pin number between 1000 and 9999. The generated number is used as a one time password.
        private string GeneratePin()
        {
            Random random = new Random();
            return random.Next(1000, 10000).ToString();// Generates a random number between 1000 and 9999
        }
    }
}
