using CloPosProject.Application.Abstract.Ingredient;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Ingredient;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Commands.Handler.Ingredient
{
    public class UseStockHandler : IRequestHandler<UseStockCommand, SimpleResponse<string>>
    {
        private readonly IIngredientService _service;
        public UseStockHandler(IIngredientService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(UseStockCommand request, CancellationToken cancellationToken)
            => await _service.UseStockAsync(request.IngredientId, request.Quantity);
    }
}
