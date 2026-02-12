using CloPosProject.Application.Abstract.Authentication;
using CloPosProject.Application.DTOs.Authentication;
using CloPosProject.Domain.Entities;
using CloPosProject.Domain.Enums;
using CloPosProject.Infrastructure.Concurate.Encrytping;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Infrastructure.Concurate.Authentication
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly TokenOptionsDto _tokenOptions;
        private readonly DateTime _expiresAt;
        public JwtGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
            _tokenOptions = _configuration.GetSection("TokenOptions").Get<TokenOptionsDto>() ?? new();
            _expiresAt = DateTime.UtcNow.AddMinutes(_tokenOptions.TokenExpiration);
        }

        public AuthResponseDto GenerateToken(List<Claim> claims)
        {
            JwtHeader jwtHeader = CreateJwtHeader();
            JwtPayload jwtPayload = CreateJwtPayload(claims);
            JwtSecurityToken jwtToken = new(jwtHeader, jwtPayload);

            return CreateAccessToken(jwtToken);

        }

        private AuthResponseDto CreateAccessToken(JwtSecurityToken jwtToken)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

            return new()
            {
                Token = jwtSecurityTokenHandler.WriteToken(jwtToken),
                ExpiredDate = _expiresAt,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpiredAt = _expiresAt.AddMinutes(15)
            };

        }

        private JwtPayload CreateJwtPayload(List<Claim> claims)
        {
            return new(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: _expiresAt
                );
        }

        private JwtHeader CreateJwtHeader()
        {
            SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            SigningCredentials signingCredentials = SigninCredentialHelper.CreateSigninCredentials(securityKey);
            JwtHeader jwtHeader = new(signingCredentials);
            return jwtHeader;
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

      
    }
}
