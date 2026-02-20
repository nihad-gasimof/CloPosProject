using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.MenuItem;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Queries.MenuItem
{
    public record GetMenuItemByIdQuery(Guid Id) : IRequest<SimpleResponse<MenuItemResponse>>;
}
