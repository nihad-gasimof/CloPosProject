using CloPosProject.Application.Abstract.Table;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Table;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Table
{
    public class DeleteTableHandler : IRequestHandler<DeleteTableCommand, SimpleResponse<string>>
    {
        private readonly ITableService _service;
        public DeleteTableHandler(ITableService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(DeleteTableCommand request, CancellationToken cancellationToken)
            => await _service.DeleteTableAsync(request.Id);
    }
}
