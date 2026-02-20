using CloPosProject.Application.DTOs.Order;
using FluentValidation;

namespace CloPosProject.Application.Validations.Order
{
    public class CreateDineInOrderRequestValidator : AbstractValidator<CreateDineInOrderRequest>
    {
        public CreateDineInOrderRequestValidator()
        {
            RuleFor(x => x.TableId).NotEmpty();
            RuleFor(x => x.TableNumber).NotEmpty();
            RuleFor(x => x.WaiterId).NotEmpty();
            RuleFor(x => x.Items).NotEmpty();
            RuleForEach(x => x.Items).SetValidator(new OrderItemRequestValidator());
        }
    }
}
