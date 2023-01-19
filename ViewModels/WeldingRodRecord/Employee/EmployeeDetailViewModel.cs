﻿using ViewModels.Shared;
using Pagination;
using Select2.Model;
using Enums;
using System.ComponentModel;
using ViewModels.CRUD;
using ViewModels.Authentication;

namespace ViewModels.WeldingRodRecord.Employee
{
    public class EmployeeDetailViewModel : BaseCrudViewModel, ISelect2Data
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public string Name
        {
            get
            {
                return FirstName + " " + LastName;
            }
            set { }
        }
        [DisplayName("Employee ID")]
        public string EmployeeId { get; set; }

        public long? Telephone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        public string? City { get; set; }
        public string? State { get; set; }
        [DisplayName("Zip Code")]
        public string? ZipCode { get; set; }
        [DisplayName("Social Security")]
        public string? SocialSecurity { get; set; }
        [DisplayName("Driver's License")]
        public string? DriverLicense { get; set; }
        [DisplayName("Bank Account")]
        public long? BankAccount { get; set; }
        [DisplayName("Routing Number")]
        public long? RoutingNumber { get; set; }
        [DisplayName("Emergency Contact Name")]
        public string? EmergencyContactName { get; set; }
        [DisplayName("Emergency Contact Number")]
        public long? EmergencyContactNumber { get; set; }
        [DisplayName("Date of Hire")]
        public DateTime DateOfHire { get; set; }
        [DisplayName("Termination Date")]
        public DateTime TerminationDate { get; set; }

        public string FormattedTerminationDate
        {
            get
            {
                return TerminationDate.Date.ToString("MM/dd/yyyy");
            }
        }
        public string FormattedDateOfHire
        {
            get
            {
                return DateOfHire.Date.ToString("MM/dd/yyyy");
            }
        }


        [DisplayName("Approver Name")]
        public UserBriefViewModel Approver { get; set; } = new UserBriefViewModel();
    }
    
}