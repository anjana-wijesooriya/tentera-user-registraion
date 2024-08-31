using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
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
    public class UsersController : ControllerBase
    {
        private readonly TenteraDbContext _context;
        private readonly IUserService _userService;
        private readonly IOtpService _otpService;

        public UsersController(TenteraDbContext context, IUserService userService, IOtpService otpService)
        {
            _context = context;
            _userService = userService;
            _otpService = otpService;
        }

        [HttpGet("login")]
        public async Task<ActionResult> Login(int nic)
        {
            if (nic == 0)
            {
                return BadRequest("Please provide NIC no.");
            }

            var userExists = await _userService.UserExists(nic);
            if (!userExists)
            {
                return Ok(new { IsUserExists = false });
            }

            var user = await _userService.GetUserByNic(nic);
            await _otpService.SendOtp(user);

            return Ok(new { LoggedUserId = user, IsUserExists = true });

        }

        [HttpPost("create")]
        public async Task<ActionResult> PostUser([FromBody] CreateUserRequest request)
        {
            // Early return if request is null
            if (request == null) return NotFound("Request is null");

            var userExists = await _userService.UserExists(request.NIC);
            if (userExists)
            {
                return Conflict("An account with this NIC already exists.");
            }

            // Create user and store result in user variable
            var user = await _userService.CreateUser(request);


            // Return appropriate response based on user creation result
            return user != null
                ? Ok(new { CreatedUserId = user.Id })
                : BadRequest();

        }

        [HttpPost("confirm-policies")]
        public async Task<ActionResult> ConfirmPrivacyPolicies([FromBody] ConfirmPolicyRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request is null");
            }

            if (request.IsConfirmed == false)
            {
                return Unauthorized("Policy confirmation is required");
            }

            var isSuccess = await _userService.ConfirmPolicies(request.UserId, request.IsConfirmed);

            return isSuccess ? Ok() : BadRequest("Policy confirmation failed");
        }

        [HttpPost("save-pin")]
        public async Task<ActionResult> SavePasscode([FromBody] PinVerificationRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request is null");
            }
            var isPassCodeUpdated = await _userService.SavePasscode(request.UserId, request.PinCode);
            if (isPassCodeUpdated)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Fail to add the pin.");
            }
        }

        [HttpPost("confirm-pin")]
        public async Task<ActionResult> ConfirmPasscode([FromBody] PinVerificationRequest request)
        {
            if (request == null)
            {
                return NotFound();
            }
            var isPassCodeUpdated = await _userService.ConfirmPasscode(request.UserId, request.PinCode);
            if (isPassCodeUpdated)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Pin mismatch");
            }
        }

        [HttpPost("biometric")]
        public async Task<ActionResult> SaveBiometricStatus([FromBody] BiometricStatusRequest request)
        {
            if (request is null)
            {
                return BadRequest("Request cannot be null");
            }

            var isSavedData = await _userService.SaveBiometricStaus(request.UserId, request.HasEnableBiometric);

            return isSavedData ? Ok() : BadRequest("Failed to save biometric status");
        }

        // GET: api/Users/5
        [HttpGet()]
        public async Task<ActionResult> GetUser(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user is null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok(user);
        }
    }
}
