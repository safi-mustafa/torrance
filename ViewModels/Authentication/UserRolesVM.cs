﻿using Pagination;

namespace ViewModels.Authentication
{
    public class UserRolesVM
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
    }
    public class UserRoleVM
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
    }
    public class UserRolesSearchVM : BaseSearchModel
    {
        public string? Name { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
