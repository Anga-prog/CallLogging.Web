using CallLogging.Data.Models;
using CallLogging.Services.DTOs;
using CallLogging.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CallLogging.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly CallLoggingContext _context;

        public UserService(CallLoggingContext context)
        {
            _context = context;
        }

        public async Task<UserInfoDto?> ValidateUserAsync(string email, string password)
        {
            var login =  await _context.PersonLogins
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Email == email && p.IsActive);

            if (login == null || string.IsNullOrEmpty(login.PasswordHash))
                return null;

            // Passwords are stored as BCrypt hashes.
            // If your existing DB uses a different algorithm, update this check.
            if (!BCrypt.Net.BCrypt.Verify(password, login.PasswordHash))
                return null;

            return new UserInfoDto
            {
                PersonId = login.PersonId,
                FullName = login.FullName,
                Email = login.Email,
                IsStaff = login.IsStaff == 1,
                SystemUserId = login.SystemUserId,
                RoleName = login.RoleName,
                IsClient = login.IsClient == 1,
                ClientProfileId = login.ClientProfileId,
                Company = login.Company
            };
        }

        public async Task UpdateLastLoginAsync(int personId)
        {
            var person = await _context.People.FindAsync(personId);
            if (person != null)
            {
                person.LastLoginAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}
