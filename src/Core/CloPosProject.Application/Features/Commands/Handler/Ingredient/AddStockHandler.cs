using CloPosProject.Application.Abstract.Ingredient;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Ingredient;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Commands.Handler.Ingredient
{
    public class AddStockHandler : IRequestHandler<AddStockCommand, SimpleResponse<string>>
    {
        private readonly IIngredientService _service;
        public AddStockHandler(IIngredientService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(AddStockCommand request, CancellationToken cancellationToken)
            => await _service.AddStockAsync(request.IngredientId, request.Quantity, request.UnitPrice);
    }
}
