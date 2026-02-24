using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Order
{
    public record VerifyAndCompletePaymentCommand(int PurchaseId) : IRequest<SimpleResponse<string>>;
}
