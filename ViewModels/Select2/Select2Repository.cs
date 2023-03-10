using System;
using System.Collections.Generic;
using System.Linq;
using Select2.Model;

namespace Select2
{
    public class Select2Repository
    {
        List<Select2ViewModel> GetPagedListOptions(int pageSize, int pageNumber, List<Select2ViewModel> list)
        {
            return list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public Select2PagedResultViewModel GetSelect2PagedResult(int pageSize, int pageNumber,int totalCount, List<Select2ViewModel> list)
        {
            var select2pagedResult = new Select2PagedResultViewModel();
            select2pagedResult.Results = list;// GetPagedListOptions(pageSize, pageNumber, list);
            select2pagedResult.Total = totalCount;
            return select2pagedResult;
        }

        List<Select2OptionModel<T>> GetPagedListOptions<T>(int pageSize, int pageNumber, List<Select2OptionModel<T>> list)
        {
            return list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public Select2PagedResult<T> GetSelect2PagedResult<T>(int pageSize, int pageNumber, int totalCount, List<Select2OptionModel<T>> list)
        {
            var select2pagedResult = new Select2PagedResult<T>();
            select2pagedResult.Results = list;// GetPagedListOptions(pageSize, pageNumber, list);
            select2pagedResult.Total = totalCount;
            return select2pagedResult;
        }
    }
}
