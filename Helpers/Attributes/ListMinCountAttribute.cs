using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Helpers.Attributes
{
    public class ListMinCountAttribute<T> : ValidationAttribute
    {
        private readonly int _minCount;
        private readonly string _errorMessage;

        public ListMinCountAttribute(int minCount)
        {
            _minCount = minCount;
        }
        public ListMinCountAttribute(int minCount, string errorMessage)
        {
            _minCount = minCount;
            _errorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IList<T> list && list.Count >= _minCount)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"{validationContext.DisplayName} must have at least {_minCount} record.");
        }
    }
}

