using FluentValidation;

namespace WebApiTemplate.Api.Application.Features.Todos.Commands.ExternalApi.CreateTodo
{
    public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
    {
        public CreateTodoCommandValidator()
        {
            RuleFor(p => p.UserId).NotEmpty();

            RuleFor(p => p.Title).NotEmpty();

            RuleFor(p => p.Body).NotEmpty();
        }
    }
}
