using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IEnumerable<SelectListItem> GetEnumSelectListWithDefaultValue<TEnum>(this IHtmlHelper htmlHelper, TEnum? defaultValue)
            where TEnum : struct
        {
            var selectList = htmlHelper.GetEnumSelectList<TEnum>().ToList();
            selectList.Insert(0, new SelectListItem { Text = "Select All", Value = "-1" });
            selectList.Single(x => x.Value == "-1").Selected = true;
            return selectList;
        }
    }
}
