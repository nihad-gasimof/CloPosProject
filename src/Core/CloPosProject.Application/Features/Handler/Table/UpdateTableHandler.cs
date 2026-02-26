using CloPosProject.Application.Abstract.Table;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Table;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Table
{
    public class UpdateTableHandler : IRequestHandler<UpdateTableCommand, SimpleResponse<string>>
    {
        private readonly ITableService _service;
        public UpdateTableHandler(ITableService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(UpdateTableCommand request, CancellationToken cancellationToken)
            => await _service.UpdateTableAsync(request.Id, request.TableNumber, request.Capacity, request.Location);
    }
}
