﻿using System.ComponentModel.DataAnnotations;

namespace Helper.Attributes
{
    public class RequiredMultiSelect2Attribute : ValidationAttribute
    {
        public string ErrorPropertyName { get; set; }
        public string IsValidationEnabledPropertyName { get; set; }
        public RequiredMultiSelect2Attribute(string errorPropertyName = "", string isValidationEnabledPropertyName = "") : base()
        {
            ErrorPropertyName = errorPropertyName;
            IsValidationEnabledPropertyName = isValidationEnabledPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            object instance = validationContext.ObjectInstance;
            if (string.IsNullOrEmpty(ErrorPropertyName) == false &&
                string.IsNullOrEmpty(IsValidationEnabledPropertyName) == false)
            {
                var errorPropertyValue = instance.GetType().GetProperty(ErrorPropertyName)?.GetValue(instance, null);
                var isValidationEnabled = instance.GetType().GetProperty(IsValidationEnabledPropertyName)?.GetValue(instance, null);
                if (isValidationEnabled != null && bool.Parse(isValidationEnabled.ToString()))
                {
                    if ((value as IList<object>)?.Count() < 1)
                    {
                        return new ValidationResult(errorPropertyValue?.ToString());
                    }
                }
            }

            return ValidationResult.Success;
        }
        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }


    }
}
