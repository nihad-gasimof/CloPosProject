using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Category
{
    public record CreateCategoryCommand(string Name, string Description, int DisplayOrder) : IRequest<SimpleResponse<Guid>>;
}
