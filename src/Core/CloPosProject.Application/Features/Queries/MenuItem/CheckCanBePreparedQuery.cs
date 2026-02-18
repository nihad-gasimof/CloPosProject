using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Queries.MenuItem
{
    public record CheckCanBePreparedQuery(Guid MenuItemId) : IRequest<SimpleResponse<bool>>;
}
