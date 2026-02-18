using CloPosProject.Application.Abstract.Authentication;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Authentication;
using CloPosProject.Application.Exceptions.Common;
using CloPosProject.Application.Features.Commands.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.User
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Response<AuthResponseDto>>
    {
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Response<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new NotFoundException("Request can not null");
            }
            var result = await _authService.LoginAsync(request.LoginDto);
            return result;
        }
    }
}
