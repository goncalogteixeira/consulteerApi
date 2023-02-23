using FluentValidation;

namespace WebApiTemplate.Api.Application.Features.Todos.Commands.DataBaseCrud.UpdateTodoDatabase
{
    public class UpdateTodoDatabaseCommandValidator : AbstractValidator<UpdateTodoDatabaseCommand>
    {
        public UpdateTodoDatabaseCommandValidator()
        {
            RuleFor(p => p.Id).GreaterThan(0);

            RuleFor(p => p.UserId).GreaterThan(0);

            RuleFor(p => p.Title).NotEmpty().MaximumLength(200);

            RuleFor(p => p.Body).NotEmpty().MaximumLength(1000);
        }
    }
}
