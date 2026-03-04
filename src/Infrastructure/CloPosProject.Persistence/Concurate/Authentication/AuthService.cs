using CloPosProject.Application.Abstract.Authentication;
using CloPosProject.Application.Abstract.Email;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Authentication;
using CloPosProject.Domain.Entities;
using CloPosProject.Domain.Enums;
using CloPosProject.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
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
        private readonly IEmailService _mailService;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<User> _signinmanager;
        public AuthService(IJwtGenerator jwtGenerator, UserManager<User> userManager, SignInManager<User> signinmanager, ApplicationDbContext context, IEmailService mailService)
        {
            _jwtGenerator = jwtGenerator;
            _userManager = userManager;
            _signinmanager = signinmanager;
            _context = context;
            _mailService = mailService;
        }

        public async Task<SimpleResponse<string>> AssignRoleAsync(Guid Id,Roles role)
        {
           var user= await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return new SimpleResponse<string>("İstifadəçi tapılmadı.");
            }
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
          await   _userManager.AddToRoleAsync(user,role.ToString() );
            return new SimpleResponse<string>("Userin Rolu Ugurla deyisdirildi",$"Role:{role},Id:{Id}");
        }

        public async  Task<SimpleResponse<List<GetUserDto>>> GetAllUser()
        {
            var users = _userManager.Users.ToList();
            if (!users.Any())
            {
                return new SimpleResponse<List<GetUserDto>>( "No users found.", new List<GetUserDto>());
            }
            var userDtos = new List<GetUserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user) ?? new List<string>();

                var dto = new GetUserDto
                {
                    Id = user.Id.ToString(),
                    Name = user.UserName??"Yoxdur",
                    Surname=user.Surname,
                    Mail = user.Email??"Yoxdur",
                    Role = roles.FirstOrDefault()??"Rol teyin olunmayib"
                };

                userDtos.Add(dto);
            }
            return new SimpleResponse<List<GetUserDto>>(userDtos);
        }

        public async Task<Response<AuthResponseDto>> LoginAsync(LoginDto loginDto)
            {
            if(string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return Response<AuthResponseDto>.Fail("Email və şifrə daxil edilməlidir.", 400);
            }
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (!user.EmailConfirmed)
            {
                var token1 = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token1));
                var confirmationLink = $"https://localhost:7046/api/Auth/ConfirmEmail?userid={user.Id}&token={encodedToken}";
                var body = $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='UTF-8'>
<style>
    body {{
        font-family: Arial, sans-serif;
        background-color: #f4f6f9;
        padding: 20px;
    }}
    .container {{
        max-width: 600px;
        margin: auto;
        background: #ffffff;
        border-radius: 8px;
        padding: 30px;
        box-shadow: 0 4px 12px rgba(0,0,0,0.05);
    }}
    .logo {{
        font-size: 24px;
        font-weight: bold;
        color: #2c3e50;
        text-align: center;
        margin-bottom: 20px;
    }}
    .btn {{
        display: inline-block;
        padding: 12px 20px;
        margin-top: 20px;
        background-color: #27ae60;
        color: #ffffff !important;
        text-decoration: none;
        border-radius: 6px;
        font-weight: bold;
    }}
    .footer {{
        margin-top: 30px;
        font-size: 12px;
        color: #888;
        text-align: center;
    }}
</style>
</head>
<body>
<div class='container'>
    <div class='logo'>CloPOS</div>
    <h2>Hesabınızı təsdiqləyin</h2>
    <p>Salam <strong>{user.UserName}</strong>,</p>
    <p>CloPOS platformasında qeydiyyatınız uğurla tamamlandı.</p>
    <p>Zəhmət olmasa aşağıdakı düyməyə klik edərək email ünvanınızı təsdiqləyin:</p>
    <div style='text-align:center;'>
        <a href='{confirmationLink}' class='btn'>Emaili Təsdiqlə</a>
    </div>
    <p>Əgər bu qeydiyyatı siz etməmisinizsə, bu emaili nəzərə almayın.</p>
    <div class='footer'>
        © {DateTime.Now.Year} CloPOS. Bütün hüquqlar qorunur.
    </div>
</div>
</body>
</html>";
                var mailsent = await _mailService.SendEmailAsync(user.Email, "Email Tesdiqleme", body);
                return Response<AuthResponseDto>.Fail("Email tesdiqlenmeyib,tesdiqleme linki emailinize yonlendirildi.", 400);
            }
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

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var confirmationLink =$"https://localhost:7046/api/Auth/ConfirmEmail?userid={user.Id}&token={encodedToken}";
            var body = $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='UTF-8'>
<style>
    body {{
        font-family: Arial, sans-serif;
        background-color: #f4f6f9;
        padding: 20px;
    }}
    .container {{
        max-width: 600px;
        margin: auto;
        background: #ffffff;
        border-radius: 8px;
        padding: 30px;
        box-shadow: 0 4px 12px rgba(0,0,0,0.05);
    }}
    .logo {{
        font-size: 24px;
        font-weight: bold;
        color: #2c3e50;
        text-align: center;
        margin-bottom: 20px;
    }}
    .btn {{
        display: inline-block;
        padding: 12px 20px;
        margin-top: 20px;
        background-color: #27ae60;
        color: #ffffff !important;
        text-decoration: none;
        border-radius: 6px;
        font-weight: bold;
    }}
    .footer {{
        margin-top: 30px;
        font-size: 12px;
        color: #888;
        text-align: center;
    }}
</style>
</head>
<body>
<div class='container'>
    <div class='logo'>CloPOS</div>
    <h2>Hesabınızı təsdiqləyin</h2>
    <p>Salam <strong>{user.UserName}</strong>,</p>
    <p>CloPOS platformasında qeydiyyatınız uğurla tamamlandı.</p>
    <p>Zəhmət olmasa aşağıdakı düyməyə klik edərək email ünvanınızı təsdiqləyin:</p>
    <div style='text-align:center;'>
        <a href='{confirmationLink}' class='btn'>Emaili Təsdiqlə</a>
    </div>
    <p>Əgər bu qeydiyyatı siz etməmisinizsə, bu emaili nəzərə almayın.</p>
    <div class='footer'>
        © {DateTime.Now.Year} CloPOS. Bütün hüquqlar qorunur.
    </div>
</div>
</body>
</html>";
           var mailsent=  await _mailService.SendEmailAsync(user.Email, "Email Tesdiqleme", body);
            if (!mailsent)
            {
                await _userManager.DeleteAsync(user);
                return  Response<string>.Fail("Mail göndərilmədi, qeydiyyat ləğv edildi.", 400);
            }
           
          

                return Response<string>.Success(user.Id, 201);
            }

        public async Task<Response<string>> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Response<string>.Fail("İstifadəçi tapılmadı.", 404);
            }

            try
            {
                byte[] decodedTokenBytes = WebEncoders.Base64UrlDecode(token);
                string actualToken = Encoding.UTF8.GetString(decodedTokenBytes);

                var result = await _userManager.ConfirmEmailAsync(user, actualToken);

                if (result.Succeeded)
                {
                    return Response<string>.Success("Email uğurla təsdiqləndi.", 200);
                }

                var errors = result.Errors.Select(e => e.Description);
                return Response<string>.Fail(errors, 400);
            }
            catch (Exception)
            {
                return Response<string>.Fail("Token formatı səhvdir.", 400);
            }
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

