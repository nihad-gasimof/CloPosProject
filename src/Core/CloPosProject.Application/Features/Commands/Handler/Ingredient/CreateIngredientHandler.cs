using CloPosProject.Application.Abstract.Ingredient;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Ingredient;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Commands.Handler.Ingredient
{
    public class CreateIngredientHandler : IRequestHandler<CreateIngredientCommand, SimpleResponse<Guid>>
    {
        private readonly IIngredientService _service;
        public CreateIngredientHandler(IIngredientService service) => _service = service;
        public async Task<SimpleResponse<Guid>> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
            => await _service.CreateIngredietAsync(request.Dto);
    }
}
