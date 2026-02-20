using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Order;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Order
{
    public record CreateDeliveryOrderCommand(CreateDeliveryOrderRequest Request) : IRequest<SimpleResponse<Guid>>;
}
