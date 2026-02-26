using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Table
{
    public record ActivateTableCommand(Guid Id) : IRequest<SimpleResponse<string>>;
}
