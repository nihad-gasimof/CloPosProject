using CloPosProject.Application.Abstract.Ingredient;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Ingredient;
using CloPosProject.Application.Features.Queries.Ingredient;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Ingredient
{
    public class GetLowStockHandler : IRequestHandler<GetLowStockIngredientsQuery, SimpleResponse<List<LowStockResponseDto>>>
    {
        private readonly IIngredientService _service;
        public GetLowStockHandler(IIngredientService service) => _service = service;
        public async Task<SimpleResponse<List<LowStockResponseDto>>> Handle(GetLowStockIngredientsQuery request, CancellationToken cancellationToken)
            => await _service.GetLowStockIngredientsAsync();
    }
}
