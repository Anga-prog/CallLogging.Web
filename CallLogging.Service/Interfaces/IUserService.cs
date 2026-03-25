using CallLogging.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CallLogging.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserInfoDto?> ValidateUserAsync(string email, string password);
        Task UpdateLastLoginAsync(int personId);
    }

}
