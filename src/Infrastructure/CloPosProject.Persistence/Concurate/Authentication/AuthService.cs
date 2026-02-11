using CloPosProject.Application.Abstract.Authentication;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Authentication;
using CloPosProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Persistence.Concurate.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IJwtGenerator _jwtGenerator;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User>     _signinmanager;
        private readonly IRefreshTokenService _refreshTokenService;
        public AuthService(IJwtGenerator jwtGenerator, UserManager<User> userManager, SignInManager<User> signinmanager, IRefreshTokenService refreshTokenService)
        {
            _jwtGenerator = jwtGenerator;
            _userManager = userManager;
            _signinmanager = signinmanager;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<Response<AuthResponseDto>> LoginAsync(LoginDto loginDto)
            {
            if(string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return Response<AuthResponseDto>.Fail("Email və şifrə daxil edilməlidir.", 400);
            }
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                    return Response<AuthResponseDto>.Fail("Email və ya şifrə səhvdir.", 400);

                var result = await _signinmanager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if (!result.Succeeded)
                    return Response<AuthResponseDto>.Fail("Email və ya şifrə səhvdir.", 400);
                
                var token = await _jwtGenerator.GenerateToken(user);
            var refreshtoken= await _refreshTokenService.CreateAsync(user);
             AuthResponseDto tokenDto = new AuthResponseDto
            {
                AccessToken = token,
                RefreshToken = refreshtoken.Token
            };
            return Response<AuthResponseDto>.Success(tokenDto,201);
            }

            public async Task<Response<string>> RegisterAsync(RegisterDto registerDto)
            {
                var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return Response<string>.Fail(
                        $"Email {registerDto.Email} artıq mövcuddur.",
                        statusCode: 400
                    );
                }

                
                var user = new User
                {
                    Name = registerDto.Name,
                    Surname = registerDto.Surname,
                    Email = registerDto.Email,
                    UserName = registerDto.Username,  
                    EmailConfirmed = false
                };

             
                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return Response<string>.Fail(errors, 400);
                }

                if (!await _userManager.IsInRoleAsync(user, "Member"))
                {
                    await _userManager.AddToRoleAsync(user, "Member");
                }

          

                return Response<string>.Success(user.Id, 201);
            }
        }

    }

