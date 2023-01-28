﻿using ViewModels.Authentication.Approver;
using ViewModels.WeldingRodRecord.Employee;

namespace ViewModels.Authentication
{
    public class TokenVM
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
    }

    public class EmployeeTokenVM : TokenVM
    {
        public EmployeeDetailViewModel UserDetail { get; set; }
    }

    public class UserTokenVM : TokenVM
    {
        public UserDetailViewModel UserDetail { get; set; }
    }
}
