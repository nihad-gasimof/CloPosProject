using CloPosProject.Application.Abstract.Authentication;
using CloPosProject.Application.DTOs.Authentication;
using CloPosProject.Application.Exceptions.InvalidToken;
using CloPosProject.Domain.Entities;
using CloPosProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Infrastructure.Concurate.Authentication
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IJwtGenerator _jwtGenarator;
        private readonly ApplicationDbContext _context;

        public RefreshTokenService(IJwtGenerator jwtGenarator, ApplicationDbContext context)
        {
            _jwtGenarator = jwtGenarator;
            _context = context;
        }
        public string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public async Task<RefreshToken> CreateAsync(User user)
        {
            var token = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                UserId = user.Id,
                Expires= DateTime.UtcNow.AddDays(7),
                
            };
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();
            return token;
        }


        public async Task<AuthResponseDto> RefreshTokenAsync(string token)
        {
            var stored = await _context.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(x => x.Token == token);
            if (stored==null || stored.IsRevoked || stored.Expires<DateTime.UtcNow)
            {
                throw new InvalidTokenException();
            }
            stored.IsRevoked = true;
            var newAccessToken = await _jwtGenarator.GenerateToken(stored.User);
            var refreshToken = await CreateAsync(stored.User);
            await _context.SaveChangesAsync();
            var dto=new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = refreshToken.Token
            };
            return dto;
        }
    }
}
