using FluentValidation;

namespace WebApiTemplate.Api.Application.Features.Todos.Commands.DataBaseCrud.UpdateBatchTodo
{
    public class UpdateBatchTodoCommandValidator : AbstractValidator<UpdateBatchTodoCommand>
    {
        public UpdateBatchTodoCommandValidator()
        {
            RuleForEach(cmd => cmd.Items).ChildRules(items => items.RuleFor(todo => todo.Id).GreaterThan(0));

            RuleForEach(cmd => cmd.Items).ChildRules(items => items.RuleFor(todo => todo.UserId).GreaterThan(0));

            RuleForEach(cmd => cmd.Items).ChildRules(items => items.RuleFor(todo => todo.Title).NotEmpty().MaximumLength(200));

            RuleForEach(cmd => cmd.Items).ChildRules(items => items.RuleFor(todo => todo.Body).NotEmpty().MaximumLength(1000));
        }
    }
}
