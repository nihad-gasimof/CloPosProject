using CloPosProject.Application.Abstract.Order;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Order;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Order
{
    public class ApplyDiscountHandler : IRequestHandler<ApplyDiscountCommand, SimpleResponse<string>>
    {
        private readonly IOrderService _service;
        public ApplyDiscountHandler(IOrderService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(ApplyDiscountCommand request, CancellationToken cancellationToken)
            => await _service.ApplyDiscountAsync(request.OrderId, request.Discount);
    }
}
