using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.MenuItem
{
    public record SetAvailabilityCommand(Guid Id, bool IsAvailable) : IRequest<SimpleResponse<string>>;
}
