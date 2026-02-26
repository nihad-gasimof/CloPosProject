using CloPosProject.Application.Abstract.Table;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Table;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Table
{
    public class ActivateTableHandler : IRequestHandler<ActivateTableCommand, SimpleResponse<string>>
    {
        private readonly ITableService _service;
        public ActivateTableHandler(ITableService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(ActivateTableCommand request, CancellationToken cancellationToken)
            => await _service.ActivateTableAsync(request.Id);
    }
}
