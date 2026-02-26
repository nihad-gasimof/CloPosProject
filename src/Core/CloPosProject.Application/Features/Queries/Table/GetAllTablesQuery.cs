using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Table;
using MediatR;
using System.Collections.Generic;

namespace CloPosProject.Application.Features.Queries.Table
{
    public record GetAllTablesQuery(bool? IsActive, CloPosProject.Domain.Enums.TableStatus? Status) : IRequest<SimpleResponse<List<TableSummaryResponse>>>;
}
