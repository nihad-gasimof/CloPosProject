using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Order;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Queries.Order
{
    public record GetOrderByIdQuery(Guid Id) : IRequest<SimpleResponse<OrderResponse>>;
}
