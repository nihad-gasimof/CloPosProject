using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.MenuItem;
using MediatR;
using System.Collections.Generic;

namespace CloPosProject.Application.Features.Queries.MenuItem
{
    public record GetAvailableMenuItemsQuery() : IRequest<SimpleResponse<List<MenuItemSummaryResponse>>>;
}
