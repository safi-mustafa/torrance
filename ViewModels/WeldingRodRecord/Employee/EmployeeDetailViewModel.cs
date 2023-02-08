using ViewModels.Shared;
using Pagination;
using Select2.Model;
using Enums;
using System.ComponentModel;
using ViewModels.CRUD;
using ViewModels.Authentication;
using ViewModels.Common.Contractor;
using ViewModels.Common.Company;

namespace ViewModels.WeldingRodRecord.Employee
{
    public class EmployeeDetailViewModel : BaseCrudViewModel, ISelect2Data
    {
        public long? Id { get; set; }
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
        [DisplayName("Access Code")]
        public string EmployeeId { get; set; }

        public string Role { get; set; }

        //public long? Telephone { get; set; }
        public string? Email { get; set; }
        //public string? Address { get; set; }

        //public string? City { get; set; }
        //public string? State { get; set; }
        //[DisplayName("Zip Code")]
        //public string? ZipCode { get; set; }
        //[DisplayName("Social Security")]
        //public string? SocialSecurity { get; set; }
        //[DisplayName("Driver's License")]
        //public string? DriverLicense { get; set; }
        //[DisplayName("Bank Account")]
        //public long? BankAccount { get; set; }
        //[DisplayName("Routing Number")]
        //public long? RoutingNumber { get; set; }
        //[DisplayName("Emergency Contact Name")]
        //public string? EmergencyContactName { get; set; }
        //[DisplayName("Emergency Contact Number")]
        //public long? EmergencyContactNumber { get; set; }
        //[DisplayName("Date of Hire")]
        //public DateTime DateOfHire { get; set; }
        //[DisplayName("Termination Date")]
        //public DateTime TerminationDate { get; set; }

        //public string FormattedTerminationDate
        //{
        //    get
        //    {
        //        return TerminationDate.Date.ToString("MM/dd/yyyy");
        //    }
        //}
        //public string FormattedDateOfHire
        //{
        //    get
        //    {
        //        return DateOfHire.Date.ToString("MM/dd/yyyy");
        //    }
        //}
        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();
        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel();
        public ApproverStatus IsApprover { get; set; }
    }

}