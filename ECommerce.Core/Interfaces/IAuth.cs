using ECommerce.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Interfaces
{
    public interface IAuth
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO);
        Task<string> LoginAsync(LoginDTO loginDTO);
        Task SendEmail(string email, string code, string component, string subject, string message);
        Task<bool> SendEmailForForgetPassword(string email);
        Task<string> ResetPassword(ResetPasswordDTO resetPassword);
        Task<bool> ActiveAccount(ActiveAccountDTO accountDTO);

    }
}
