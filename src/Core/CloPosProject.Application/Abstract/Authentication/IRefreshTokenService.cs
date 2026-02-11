using CloPosProject.Application.DTOs.Authentication;
using CloPosProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Abstract.Authentication
{
    public interface IRefreshTokenService
    {
        string GenerateRefreshToken();
        Task<RefreshToken> CreateAsync(User user);
        Task<AuthResponseDto> RefreshTokenAsync(string token);

    }
}
