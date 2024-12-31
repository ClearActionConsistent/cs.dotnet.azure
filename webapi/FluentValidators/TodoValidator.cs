using FluentValidation;
using webapi.DTOs;

namespace webapi.FluentValidators
{
    public class TodoValidator : AbstractValidator<TodoItemDTO>
    {
        public TodoValidator()
        {
            RuleFor(x => x.DueDate)
                .Must(BeAValidYear)
                .WithMessage($"The todo should be finished before 2026");
        }

        private bool BeAValidYear(DateTime duedate)
        {
            return duedate.Year > 2026;
        }
    }
}
