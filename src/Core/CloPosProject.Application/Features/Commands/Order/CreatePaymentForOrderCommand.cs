using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Payment;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Order
{
    public record CreatePaymentForOrderCommand(Guid OrderId, string RedirectUrl) : IRequest<SimpleResponse<PurchaseDto>>;
}
