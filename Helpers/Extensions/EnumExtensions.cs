using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
        }
        public static string GetDisplayName<T>(this T enumValue) where T : IComparable, IFormattable, IConvertible
        {
            try
            {
                var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
                if (fieldInfo == null)
                    return "";
                var displayAttributes = (DisplayAttribute[])fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false);

                return displayAttributes.Length > 0 ? displayAttributes[0].Name : enumValue.ToString();
            }
            catch
            {
                return "";
            }

        }

        // Extension method to get display name of enum value
        public static string GetDisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DisplayAttribute>();
            return attribute?.GetName() ?? value.ToString();
        }
    }
}
