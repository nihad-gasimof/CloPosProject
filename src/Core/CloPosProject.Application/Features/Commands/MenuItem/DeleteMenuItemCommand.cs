using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.MenuItem
{
    public record DeleteMenuItemCommand(Guid Id) : IRequest<SimpleResponse<string>>;
}
