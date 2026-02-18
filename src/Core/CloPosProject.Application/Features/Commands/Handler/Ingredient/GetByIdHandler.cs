using CloPosProject.Application.Abstract.Ingredient;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Ingredient;
using CloPosProject.Application.Features.Queries.Ingredient;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Commands.Handler.Ingredient
{
    public class GetByIdHandler : IRequestHandler<GetIngredientByIdQuery, SimpleResponse<IngredientResponseDto>>
    {
        private readonly IIngredientService _service;
        public GetByIdHandler(IIngredientService service) => _service = service;
        public async Task<SimpleResponse<IngredientResponseDto>> Handle(GetIngredientByIdQuery request, CancellationToken cancellationToken)
            => await _service.GetByIdAsync(request.Id);
    }
}
