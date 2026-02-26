using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Table
{
    public record UpdateTableCommand(Guid Id, string TableNumber, int Capacity, string Location) : IRequest<SimpleResponse<string>>;
}
