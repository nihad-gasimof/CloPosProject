using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Order;
using CloPosProject.Application.Abstract.Order;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Order
{
    public class VerifyAndCompletePaymentHandler : IRequestHandler<VerifyAndCompletePaymentCommand, SimpleResponse<string>>
    {
        private readonly IOrderService _orderService;
        public VerifyAndCompletePaymentHandler(IOrderService orderService) => _orderService = orderService;
        public async Task<SimpleResponse<string>> Handle(VerifyAndCompletePaymentCommand request, CancellationToken cancellationToken)
            => await _orderService.VerifyAndCompletePaymentAsync(request.PurchaseId);
    }
}
