using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.MenuItem;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.MenuItem
{
    public record UpdateMenuItemCommand(Guid Id, UpdateMenuItem Dto) : IRequest<SimpleResponse<string>>;
}
