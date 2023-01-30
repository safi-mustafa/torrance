using Microsoft.EntityFrameworkCore;
using Helpers.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Common;
using Enums;

namespace Models.WeldingRodRecord
{
    [Index(nameof(EmployeeId), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class Employee : BaseDBModel
    {
        [Required]
        [MaxLength(200, ErrorMessage = "First Name cannot be greater than 200 characters.")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(200, ErrorMessage = "Last Name cannot be greater than 200 characters.")]
        public string LastName { get; set; }
        [Required]
        [StringLength(4, ErrorMessage = "Employee Id must be of 4-digits.", MinimumLength = 4)]
        public string EmployeeId { get; set; }
        public long UserId { get; set; }
        public long? Telephone { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? SocialSecurity { get; set; }
        public string? DriverLicense { get; set; }
        public long? BankAccount { get; set; }
        public long? RoutingNumber { get; set; }
        public string? EmergencyContactName { get; set; }
        public long? EmergencyContactNumber { get; set; }
        public DateTime? DateOfHire { get; set; }
        public DateTime? TerminationDate { get; set; }
        public ApproverStatus? IsApprover { get; set; }
        [ForeignKey("Contractor")]
        public long ContractorId { get; set; }
        public Contractor Contractor { get; set; }
    }
}
