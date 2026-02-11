using CloPosProject.Application.Abstract.Authentication;
using CloPosProject.Application.DTOs.Authentication;
using CloPosProject.Application.Exceptions.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Commands.Handler
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshTokenCommandHandler(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {

            if (request == null) throw new NullException("Request dont be null");
            var result = await _refreshTokenService.RefreshTokenAsync(request.RefreshToken);
            
            return result;
        }
    }
}
