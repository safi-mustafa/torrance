﻿using Select2.Model;
using System.ComponentModel;
using ViewModels;

namespace ViewModels.Authentication.User
{
    public class UserBriefViewModel : BaseBriefVM
    {
        public UserBriefViewModel() : base(false, "")
        {
        }
        public UserBriefViewModel(bool isValidationEnabled) : base(isValidationEnabled)
        {
        }
        public UserBriefViewModel(bool isValidationEnabled, string errorMessage) : base(isValidationEnabled, errorMessage)
        {
        }
        public override string? Name { get; set; }
    }
    public class ApproverBriefViewModel : BaseBriefVM
    {

        public ApproverBriefViewModel() : base(true, "The Approver field is required.")
        {
        }
        public ApproverBriefViewModel(bool isValidationEnabled) : base(isValidationEnabled, "The Approver field is required.")
        {
        }
        public ApproverBriefViewModel(bool isValidationEnabled, string message) : base(isValidationEnabled, message)
        {
        }

        private string? name;

        [DisplayName("Approver")]
        public override string? Name { get => name; set => name = value; }

        public string? Email { get; set; }
    }

}
