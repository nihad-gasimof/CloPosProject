using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Order;
using MediatR;
using System.Collections.Generic;

namespace CloPosProject.Application.Features.Queries.Order
{
    public record GetPendingTakeAwayOrdersQuery(int PageNumber, int PageSize) : IRequest<SimpleResponse<List<OrderSummaryResponse>>>;
}
