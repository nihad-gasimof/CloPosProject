using CloPosProject.Application.Abstract.Ingredient;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Ingredient;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Ingredient
{
    public class UpdateIngredientHandler : IRequestHandler<UpdateIngredientCommand, SimpleResponse<string>>
    {
        private readonly IIngredientService _service;
        public UpdateIngredientHandler(IIngredientService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
            => await _service.UpdateIngredientAsync(request.Id, request.Dto);
    }
}
