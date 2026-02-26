using CloPosProject.Application.Abstract.Table;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Table;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Table
{
    public class DeactivateTableHandler : IRequestHandler<DeactivateTableCommand, SimpleResponse<string>>
    {
        private readonly ITableService _service;
        public DeactivateTableHandler(ITableService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(DeactivateTableCommand request, CancellationToken cancellationToken)
            => await _service.DeactivateTableAsync(request.Id);
    }
}
