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
    public class AssignRoleCommandHandler : IRequestHandler<AssignedRoleCommand, SimpleResponse<string>>
    {
        private readonly IAuthService _authService;

        public AssignRoleCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<SimpleResponse<string>> Handle(AssignedRoleCommand request, CancellationToken cancellationToken)
        {
            return _authService.AssignRoleAsync(request.Id, request.role);
        }
    }
}
