using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Order;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Order
{
    public record CreateDineInOrderCommand(CreateDineInOrderRequest Request) : IRequest<SimpleResponse<Guid>>;
}
