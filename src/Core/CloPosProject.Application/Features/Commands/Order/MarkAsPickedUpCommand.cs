using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Order
{
    public record MarkAsPickedUpCommand(Guid OrderId) : IRequest<SimpleResponse<string>>;
}
