using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Table
{
    public record DeleteTableCommand(Guid Id) : IRequest<SimpleResponse<string>>;
}
