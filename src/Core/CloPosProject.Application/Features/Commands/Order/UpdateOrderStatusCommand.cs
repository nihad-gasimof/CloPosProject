using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Domain.Enums;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Order
{
    public record UpdateOrderStatusCommand(Guid OrderId, OrderStatus NewStatus) : IRequest<SimpleResponse<string>>;
}
