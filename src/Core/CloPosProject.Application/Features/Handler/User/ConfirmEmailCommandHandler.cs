using CloPosProject.Application.Abstract.Authentication;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.User
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Response<string>>
    {
        private readonly IAuthService _authService;

        public ConfirmEmailCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Response<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
        return  await  _authService.ConfirmEmailAsync(request.userId, request.token);
        }
    }
}
