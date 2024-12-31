using System.ComponentModel.DataAnnotations;
using Humanizer.Localisation;
using webapi.DTOs;

namespace webapi.CustomAttibutes.ValidationAttributes
{
    public class FinishBeforYearAtribute: ValidationAttribute
    {
        private int _year;

        public FinishBeforYearAtribute(int Year)
        {
            _year = Year;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            TodoItemDTO todo = (TodoItemDTO)validationContext.ObjectInstance;

            if (todo.DueDate.Year > _year)
            {
                return new ValidationResult($"The todo should be finished before {_year}");
            }

            return ValidationResult.Success;
        }
    }
}
