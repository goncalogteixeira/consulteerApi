using FluentValidation;

namespace WebApiTemplate.Api.Application.Features.Todos.Commands.DataBaseCrud.CreateTodoDatabase
{
    public class CreateTodoDatabaseCommandValidator : AbstractValidator<CreateTodoDatabaseCommand>
    {
        public CreateTodoDatabaseCommandValidator()
        {
            RuleFor(p => p.UserId).GreaterThan(0);

            RuleFor(p => p.Title).NotEmpty().MaximumLength(200);

            RuleFor(p => p.Body).NotEmpty().MaximumLength(1000);
        }
    }
}
