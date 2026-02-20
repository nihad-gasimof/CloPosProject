using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Order
{
    public record ApplyDiscountCommand(Guid OrderId, decimal Discount) : IRequest<SimpleResponse<string>>;
}
