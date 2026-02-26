using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Category
{
    public record DeleteCategoryCommand(Guid Id) : IRequest<SimpleResponse<string>>;
}
