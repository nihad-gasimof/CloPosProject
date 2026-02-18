using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Authentication;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Commands.User
{
    public record RegisterCommand(RegisterDto registerDto):IRequest<Response<string>>
    {
      
    }
}
