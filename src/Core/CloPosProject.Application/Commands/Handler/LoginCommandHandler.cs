using CloPosProject.Application.Abstract.Authentication;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Exceptions.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Commands.Handler
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Response<string>>
    {
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Response<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
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
