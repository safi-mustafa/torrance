﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pagination
{
    public class PaginationMeta
    {
        public int CurrentPage { get; set; }
        public int PerPage { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
    }
    public class PaginatedResultModel<M>
    {
        public PaginatedResultModel()
        {
            Items = new List<M>();
            _links = new List<string>();
            _meta = new PaginationMeta();
        }
        public List<M> Items { get; set; }
        public List<string> _links { get; set; }
        public PaginationMeta _meta { get; set; }
    }

    public interface IBaseSearchModel
    {
        bool CalculateTotal { get; set; }
        int CurrentPage { get; set; }
        bool DisablePagination { get; set; }
        int Draw { get; set; }
        string OrderByColumn { get; set; }
        PaginationOrderCatalog OrderDir { get; set; }
        int PerPage { get; set; }
        DataTableSearchViewModel Search { get; set; }
    }

    public class BaseSearchModel : IBaseSearchModel
    {
        public BaseSearchModel()
        {
            Search = new DataTableSearchViewModel();
        }
        public BaseSearchModel(int currentPage, int perPage, bool disablePagination)
        {
            CurrentPage = currentPage;
            PerPage = perPage;
            DisablePagination = disablePagination;
        }
        protected int defaultPerPage { get; set; } = 15;
        public int PerPage { get; set; } = 15;
        public bool CalculateTotal { get; set; } = true;
        public int CurrentPage { get; set; } = 1;
        public bool DisablePagination { get; set; } = false;
        public virtual string OrderByColumn { get; set; }
        public PaginationOrderCatalog OrderDir { get; set; } = PaginationOrderCatalog.Asc;
        public List<SortExpression> SortExpressions { get; set; }
        [IgnoreDataMember]
        public int Draw { get; set; }
        public DataTableSearchViewModel Search { get; set; }
    }

    public class DataTableSearchViewModel
    {
        public string value { get; set; }
        public string regex { get; set; }
    }

    public class SortExpression
    {
        public string Column { get; set; }
        public string Direction { get; set; }
    }
}
