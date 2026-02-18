using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.MenuItem;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace CloPosProject.Application.Features.Commands.MenuItem
{
    public record CreateMenuItemCommand(CreateMenuItem Dto) : IRequest<SimpleResponse<Guid>>;
}
