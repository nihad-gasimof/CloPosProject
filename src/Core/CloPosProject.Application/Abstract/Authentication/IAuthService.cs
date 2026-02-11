using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Abstract.Authentication
{
    public interface IAuthService
    {
        Task<Response<AuthResponseDto>> LoginAsync(LoginDto loginDto);
        Task<Response<string>> RegisterAsync(RegisterDto registerDto);
    }
}
