using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Table
{
    public record DeactivateTableCommand(Guid Id) : IRequest<SimpleResponse<string>>;
}
