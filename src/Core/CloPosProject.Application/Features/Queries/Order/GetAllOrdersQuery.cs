using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Order;
using CloPosProject.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;

namespace CloPosProject.Application.Features.Queries.Order
{
    public record GetAllOrdersQuery(int PageNumber, int PageSize, OrderStatus? Status, OrderType? OrderType, DateTime? Date) : IRequest<SimpleResponse<List<OrderSummaryResponse>>>;
}
