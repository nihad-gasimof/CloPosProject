using CloPosProject.Application.Abstract.Ingredient;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Ingredient;
using CloPosProject.Application.Features.Queries.Ingredient;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Commands.Handler.Ingredient
{
    public class GetAllHandler : IRequestHandler<GetAllIngredientsQuery, SimpleResponse<List<IngredientResponseDto>>>
    {
        private readonly IIngredientService _service;
        public GetAllHandler(IIngredientService service) => _service = service;
        public async Task<SimpleResponse<List<IngredientResponseDto>>> Handle(GetAllIngredientsQuery request, CancellationToken cancellationToken)
            => await _service.GetAllAsync(request.IsActive);
    }
}
