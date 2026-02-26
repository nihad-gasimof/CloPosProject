using CloPosProject.Application.Abstract.Table;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Table;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Table
{
    public class CreateTableHandler : IRequestHandler<CreateTableCommand, SimpleResponse<System.Guid>>
    {
        private readonly ITableService _service;
        public CreateTableHandler(ITableService service) => _service = service;
        public async Task<SimpleResponse<System.Guid>> Handle(CreateTableCommand request, CancellationToken cancellationToken)
            => await _service.CreateTableAsync(request.TableNumber, request.Capacity, request.Location);
    }
}
