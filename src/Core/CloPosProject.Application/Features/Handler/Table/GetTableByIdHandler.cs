using CloPosProject.Application.Abstract.Table;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Table;
using CloPosProject.Application.Features.Queries.Table;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace CloPosProject.Application.Features.Handler.Table
{
    public class GetTableByIdHandler : IRequestHandler<GetTableByIdQuery, SimpleResponse<TableResponse>>
    {
        private readonly ITableService _service;
        public GetTableByIdHandler(ITableService service) => _service = service;
        public async Task<SimpleResponse<TableResponse>> Handle(GetTableByIdQuery request, CancellationToken cancellationToken)
            => await _service.GetByIdAsync(request.Id);
    }
}
