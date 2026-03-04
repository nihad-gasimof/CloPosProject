using CloPosProject.Application.Abstract.Authentication;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Authentication;
using CloPosProject.Application.Features.Queries.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.User
{
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, SimpleResponse<List<GetUserDto>>>
    {
        private readonly IAuthService _authService;

        public GetAllUserQueryHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<SimpleResponse<List<GetUserDto>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            return _authService.GetAllUser();
        }
    }
}
