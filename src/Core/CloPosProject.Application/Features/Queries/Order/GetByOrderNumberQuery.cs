using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Order;
using MediatR;

namespace CloPosProject.Application.Features.Queries.Order
{
    public record GetOrderByOrderNumberQuery(string OrderNumber) : IRequest<SimpleResponse<OrderResponse>>;
}
