using CloPosProject.Application.DTOs.Authentication;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Commands
{
    public record RefreshTokenCommand(string RefreshToken):IRequest<AuthResponseDto>
    {
    }
}
