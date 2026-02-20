using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.MenuItem;
using MediatR;
using System;
using System.Collections.Generic;

namespace CloPosProject.Application.Features.Commands.MenuItem
{
    public record UpdateMenuItemIngredientsCommand(Guid MenuItemId, List<MenuItemIngredientRequest> Ingredients) : IRequest<SimpleResponse<string>>;
}
