using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Helpers.ModelHelpers
{
    public static class ModelStateHelper
    {
        public static void RemoveModelStateErrors<TModel>(ModelStateDictionary ModelState, TModel model, string propertyKey)
        {
            var property = typeof(TModel).GetProperty(propertyKey, BindingFlags.Public | BindingFlags.Instance);

            if (property != null)
            {
                RemoveModelStateErrorsRecursive(ModelState, model, propertyKey);
            }

        }
        public static void RemoveModelStateErrorsRecursive<TModel>(ModelStateDictionary ModelState, TModel model, string propertyKey)
        {
            try
            {
                var property = typeof(TModel).GetProperty(propertyKey, BindingFlags.Public | BindingFlags.Instance);

                if (property != null)
                {
                    var propertyValue = property.GetValue(model);
                    var propertyType = property.PropertyType;
                    if (propertyValue != null)
                    {
                        if (typeof(IEnumerable<object>).IsAssignableFrom(propertyType))
                        {
                            var enumerable = propertyValue as IEnumerable<object>;
                            if (enumerable != null)
                            {
                                var index = 0;
                                foreach (var item in enumerable)
                                {
                                    var itemPropertyKey = $"{propertyKey}[{index}]";
                                    RemoveModelStateErrorsRecursive(ModelState, item, itemPropertyKey);
                                    index++;
                                }
                            }
                        }
                        else if (IsComplexType(propertyType))
                        {
                            var complexErrors = ModelState.Keys
                                .Where(key => key.StartsWith(propertyKey + "."))
                                .ToList();

                            foreach (var errorKey in complexErrors)
                            {
                                ModelState.ClearValidationState(errorKey);
                                ModelState.Remove(errorKey);
                            }

                            // Recursively handle child properties
                            var properties = propertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                            foreach (var childProperty in properties)
                            {
                                var childPropertyKey = childProperty.Name;
                                var childPropertyType = childProperty.PropertyType;
                                if (IsComplexType(childPropertyType))
                                    RemoveModelStateErrorsRecursive(ModelState, propertyValue, childPropertyKey);
                                else
                                {
                                    var childPropertyValue = childProperty.GetValue(propertyValue);
                                    var childPropertyKeyWithPrefix = propertyKey + "." + childPropertyKey;
                                    ModelState.ClearValidationState(childPropertyKeyWithPrefix);
                                    ModelState.Remove(childPropertyKeyWithPrefix);
                                }
                            }
                        }
                        else
                        {
                            ModelState.ClearValidationState(propertyKey);
                            ModelState.Remove(propertyKey);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception if needed
            }
        }

        //public static void RemoveModelStateErrorsRecursive<TModel>(ModelStateDictionary ModelState, TModel model, string propertyKey)
        //{
        //    try
        //    {
        //        var property = typeof(TModel).GetProperty(propertyKey, BindingFlags.Public | BindingFlags.Instance);

        //        if (property != null)
        //        {
        //            var propertyValue = property.GetValue(model);
        //            var propertyType = property.PropertyType;
        //            if (propertyValue != null)
        //            {

        //                if (IsComplexType(propertyType))
        //                {
        //                    var complexErrors = ModelState.Keys
        //                        .Where(key => key.StartsWith(propertyKey + "."))
        //                        .ToList();

        //                    foreach (var errorKey in complexErrors)
        //                    {
        //                        ModelState.ClearValidationState(errorKey);
        //                        ModelState.Remove(errorKey);
        //                    }

        //                    // Recursively handle child properties
        //                    var properties = propertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //                    foreach (var childProperty in properties)
        //                    {
        //                        var childPropertyKey = childProperty.Name;
        //                        var childPropertyType = childProperty.PropertyType;
        //                        if (IsComplexType(childPropertyType))
        //                            RemoveModelStateErrorsRecursive(ModelState, childProperty, childPropertyKey);
        //                        else
        //                        {
        //                            ModelState.ClearValidationState(childPropertyKey);
        //                            ModelState.Remove(childPropertyKey);
        //                        }

        //                    }
        //                }
        //                else if (typeof(IEnumerable<object>).IsAssignableFrom(propertyType))
        //                {
        //                    var enumerable = propertyValue as IEnumerable<object>;
        //                    if (enumerable != null)
        //                    {
        //                        var index = 0;
        //                        foreach (var item in enumerable)
        //                        {
        //                            var itemPropertyKey = $"{propertyKey}[{index}]";
        //                            RemoveModelStateErrorsRecursive(ModelState, item, itemPropertyKey);
        //                            index++;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    ModelState.ClearValidationState(propertyKey);
        //                    ModelState.Remove(propertyKey);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        private static bool IsComplexType(Type type)
        {
            return type.IsClass && !type.IsPrimitive && type != typeof(string);
        }
    }
}

