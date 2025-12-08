using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartTaskManager.Core.Domain;
using SmartTaskManager.Core.DTOs.Auth;
using SmartTaskManager.Core.Interfaces;
using SmartTaskManager.Infrastructure.Data;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using SmartTaskManager.Infrastructure.Helpers;

namespace SmartTaskManager.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            // Check if email already exists
            var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (exists)
            {
                return "Email already exists";
            }

            //Create new user
            var user = new User
            {
                UserId = Guid.NewGuid(),
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "User registered successfully";
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            //Check if user exists
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (user == null)
            {
                return "Invalid Credentials";
            }

            //Validate Password
            bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!valid) {
                return "Invalid Credentials";
            }


            var token = JwtHelper.GenerateToken(user);
            return token;
        }
    }
}
