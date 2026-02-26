using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Table;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Queries.Table
{
    public record GetTableByIdQuery(Guid Id) : IRequest<SimpleResponse<TableResponse>>;
}
