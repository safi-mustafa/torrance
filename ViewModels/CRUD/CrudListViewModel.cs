using Pagination;
using System.Text.Json;
using ViewModels.DataTable;

namespace ViewModels.CRUD
{
    public class CrudListViewModel
    {
        public CrudListViewModel()
        {
            SearchViewPath = "_Search";
        }
        public string Title { get; set; }
        public List<DataTableViewModel> DatatableColumns { get; set; }
        public IBaseSearchModel Filters { get; set; }
        public string DatatableColumnsJson
        {
            get
            {
                return JsonSerializer.Serialize(DatatableColumns);
            }
        }
        public string DataUrl { get; set; }
        public string SearchViewPath { get; set; }

        public string DataTableHeaderHtml { get; set; }
        public bool HideCreateButton { get; set; } = false;
        public bool HideSearchFiltersButton { get; set; } = false;
        public string CreateButtonAction { get; set; } = "Create";
        public string CreateButtonTitle { get; set; } = "";
        public bool DisableSearch { get; set; } = true;
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
    }
}
