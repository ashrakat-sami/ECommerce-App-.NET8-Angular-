using ECommerce.Core.DTOs;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;
using ECommerce.Core.Sharing;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class AuthRepository : IAuth
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IGenerateToken generateToken;
        public AuthRepository(UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken generateToken)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.generateToken = generateToken;
        }
        // Method for register
        public async Task<string> RegisterAsync(RegisterDTO registerDTO)
        {
            // check if the user not exists
            if (registerDTO == null)
            {
                return null;
            }
            // Check if the user already exists
            if (await userManager.FindByNameAsync(registerDTO.UserName) != null)
            {
                return "This username already exists";
            }
            // Check if the email already exists
            if (await userManager.FindByEmailAsync(registerDTO.Email) != null)
            {
                return "This email already exists";
            }
            // Create a new user
            AppUser user = new AppUser()
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                DsiplayName = registerDTO.DisplayName,
            };
            // Create the user
            var result = await userManager.CreateAsync(user, registerDTO.Password);
            // Check if the user is not created successfully
            if (result.Succeeded  is not true)
            {
                // Return the error message (the first error in the index)
                return result.Errors.ToList()[0].Description;
            }
            // Generate the token, this code for email activation
            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            // Send active email
            await SendEmail(user.Email, token, "active", "Active your account", "Please active your account");
            return "done";
        }
        // Method for send email
        public async Task SendEmail(string email, string code, string component, string subject, string message)
        {
            var result = new EmailDTO(
                email,
                "osulaiman710@gmail.com",
                subject,
                EmailStringBody.send(email, code, component, message));
            await emailService.SendEmail(result);
        }
        // Method for login
        public async Task<string>LoginAsync(LoginDTO login)
        {
            // Check the login process if the login false
            if (login == null)
            {
                return null;
            }
            var finduser = await userManager.FindByEmailAsync(login.Email);
            if(!finduser.EmailConfirmed)
            {
                string token = await userManager.GenerateEmailConfirmationTokenAsync(finduser);
                await SendEmail(finduser.Email, token, "active", "Active your account", "Please active your account");
                return "Please, confirm your email first, you `ve received an activation code to your email";

            }
            // if the login true
            var result = await signInManager.CheckPasswordSignInAsync(finduser, login.Password, true);
            if (result.Succeeded)
            {
                return generateToken.GetAndCreateToken(finduser);
            }
            return "Login failed, something went wrong";

        }
        // Method for forgot password
        public async Task<bool> SendEmailForForgetPassword(string email)
        {
            // Check if the email is not null
            var findUser = await userManager.FindByEmailAsync(email);
            if (email == null)
            {
                return false;
            }
            // Check if the email is not confirmed
            // Generate the token
            string token = await userManager.GeneratePasswordResetTokenAsync(findUser);
            // Send the email
            await SendEmail(findUser.Email, token, "Reset-password", "Reset your password", "Please reset your password");
            return true;
        }
        // Method for reset password
        public async Task<string> ResetPassword(ResetPasswordDTO resetPassword)
        {
            // Check if the reset password is not null
            var findUSer = await userManager.FindByEmailAsync(resetPassword.Email);
            if (findUSer is null)
            {
                return null;
            }
            // Check if the token is not null
            var result = await userManager.ResetPasswordAsync(findUSer, resetPassword.Token, resetPassword.Password);
            if (result.Succeeded)
            {
                return "Password reset successfully";
            }
            // Return the error message (the first error in the index)
            return result.Errors.ToList()[0].Description;
        }
        // Method for active account
        public async Task<bool> ActiveAccount(ActiveAccountDTO accountDTO)
        {
            // Check if the active account is not null
            var findUser = await userManager.FindByEmailAsync(accountDTO.Email);
            if (findUser is null)
            {
                return false;
            }
            // Check if the token is not null
            var result = await userManager.ConfirmEmailAsync(findUser, accountDTO.Token);
            if (result.Succeeded)
            {
                return true;
            }
            // if not succeeded, resend the email
            var token = await userManager.GenerateEmailConfirmationTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "active", "Active your account", "Please active your account");
            return false;
        }
    }
}
