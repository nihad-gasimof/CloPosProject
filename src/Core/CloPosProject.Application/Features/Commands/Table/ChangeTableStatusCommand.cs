using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Domain.Enums;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Table
{
    public record ChangeTableStatusCommand(Guid Id, TableStatus Status) : IRequest<SimpleResponse<string>>;
}
