using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserRegistraion.Context;
using UserRegistraion.Entities;
using UserRegistraion.Interfaces;
using UserRegistraion.Services;
using UserRegistraion.ViewModels;

namespace UserRegistraion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtpController : ControllerBase
    {

        private readonly IOtpService _otpService;

        public OtpController(IOtpService otpService)
        {
            _otpService = otpService;
        }

        [HttpPost("verify-mobile-otp")]
        public async Task<ActionResult<User?>> VerifyMobile([FromBody] PinVerificationRequest request)
        {
            if (request is null)
            {
                return BadRequest("Request is null"); 
            }

            var isValidPin = await _otpService.VerifyMobileOtp(request.UserId, request.PinCode);

            return isValidPin
                ? Ok(new PinVerificationResponse { UserId = request.UserId, EmailVerified = false, MobileVerified = true })
                : Unauthorized("Invalid OTP");
        }

        [HttpPost("verify-email-otp")]
        public async Task<ActionResult<User?>> VerifyEmailOtp([FromBody] PinVerificationRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request is null");
            }

            var isValidPin = await _otpService.VerifyEmailOtp(request.UserId, request.PinCode);

            if (isValidPin)
            {
                return Ok(new PinVerificationResponse { UserId = request.UserId, EmailVerified = false, MobileVerified = true });
            }
            else
            {
                return Unauthorized("Invalid OTP");
            }
        }
    }
}
