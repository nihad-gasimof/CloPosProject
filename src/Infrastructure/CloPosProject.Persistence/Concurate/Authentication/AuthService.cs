using CloPosProject.Application.Abstract.Authentication;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Authentication;
using CloPosProject.Domain.Entities;
using CloPosProject.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Persistence.Concurate.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IJwtGenerator _jwtGenerator;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User>     _signinmanager;
        public AuthService(IJwtGenerator jwtGenerator, UserManager<User> userManager, SignInManager<User> signinmanager)
        {
            _jwtGenerator = jwtGenerator;
            _userManager = userManager;
            _signinmanager = signinmanager;
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

                var result = await _signinmanager.PasswordSignInAsync(user, loginDto.Password, false,false);
                if (!result.Succeeded)
                    return Response<AuthResponseDto>.Fail("Email və ya şifrə səhvdir.", 400);
                
            var claims=(await _userManager.GetClaimsAsync(user)).ToList();
            claims.Add(new Claim("Role", Roles.Member.ToString()));
            claims.Add(new Claim("Fullname", user.Name + " " + user.Surname));
           

            var token = _jwtGenerator.GenerateToken(claims);
           
            
            return Response<AuthResponseDto>.Success(token,201);
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
        //private async Task<List<Claim>> _createClaimsAsync(User user)
        //{
        //    await _userManager.RemoveClaimsAsync(user, await _userManager.GetClaimsAsync(user));

        //    var claims = new List<Claim>
        //{
        //    new Claim("Id", user.Id.ToString()),
        //    new Claim("Email", user.Email!),
        //    new Claim("Role", Roles.Admin.ToString()),
        //    new Claim("Fullname", user.Name + " " + user.Surname),
        //};

        //    await _userManager.AddClaimsAsync(user, claims);

        //    return claims;
        //}
    }

    }

