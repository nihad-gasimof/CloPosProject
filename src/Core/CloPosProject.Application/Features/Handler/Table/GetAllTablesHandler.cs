using CloPosProject.Application.Abstract.Table;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Table;
using CloPosProject.Application.Features.Queries.Table;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using CloPosProject.Domain.Enums;

namespace CloPosProject.Application.Features.Handler.Table
{
    public class GetAllTablesHandler : IRequestHandler<GetAllTablesQuery, SimpleResponse<List<TableSummaryResponse>>>
    {
        private readonly ITableService _service;
        public GetAllTablesHandler(ITableService service) => _service = service;
        public async Task<SimpleResponse<List<TableSummaryResponse>>> Handle(GetAllTablesQuery request, CancellationToken cancellationToken)
            => await _service.GetAllAsync(request.IsActive, request.Status);
    }
}
