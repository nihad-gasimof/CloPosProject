using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Category
{
    public record UpdateCategoryCommand(Guid Id, string Name, string Description, int DisplayOrder) : IRequest<SimpleResponse<string>>;
}
