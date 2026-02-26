using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Payment;
using CloPosProject.Application.Abstract.Order;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CloPosProject.Application.Features.Commands.Order;

namespace CloPosProject.Application.Features.Handler.Order
{
    public class CreatePaymentForOrderHandler : IRequestHandler<CreatePaymentForOrderCommand, SimpleResponse<PurchaseDto>>
    {
        private readonly IOrderService _orderService;
        public CreatePaymentForOrderHandler(IOrderService orderService) => _orderService = orderService;
        public async Task<SimpleResponse<PurchaseDto>> Handle(CreatePaymentForOrderCommand request, CancellationToken cancellationToken)
            => await _orderService.CreatePaymentForOrderAsync(request.OrderId, request.RedirectUrl);
    }
}
