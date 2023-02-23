using FluentValidation;

namespace WebApiTemplate.Api.Application.Features.Todos.Queries.GetAllTodosPaged
{
    public class GetAllTodosPagedQueryValidator : AbstractValidator<GetAllTodosPagedQuery>
    {
        public GetAllTodosPagedQueryValidator()
        {
            RuleFor(p => p.PageNumber).GreaterThan(0);

            RuleFor(p => p.PageSize).GreaterThan(0);
        }
    }
}
