using CloPosProject.Application.Abstract.Table;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Table;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Table
{
    public class ChangeTableStatusHandler : IRequestHandler<ChangeTableStatusCommand, SimpleResponse<string>>
    {
        private readonly ITableService _service;
        public ChangeTableStatusHandler(ITableService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(ChangeTableStatusCommand request, CancellationToken cancellationToken)
            => await _service.ChangeTableStatusAsync(request.Id, request.Status);
    }
}
