using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.MenuItem;
using MediatR;
using System;
using System.Collections.Generic;

namespace CloPosProject.Application.Features.Queries.MenuItem
{
    public record GetAllMenuItemsQuery(bool? IsAvailable, Guid? CategoryId) : IRequest<SimpleResponse<List<MenuItemSummaryResponse>>>;
}
