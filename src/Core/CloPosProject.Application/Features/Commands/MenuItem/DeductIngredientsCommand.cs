using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.MenuItem
{
    public record DeductIngredientsCommand(Guid MenuItemId, int Quantity) : IRequest<SimpleResponse<string>>;
}
