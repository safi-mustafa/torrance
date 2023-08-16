using AutoMapper;
using Enums;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.AppSettingServices;
using ViewModels;
using ViewModels.DataTable;
using Web.Controllers.SharedControllers;

namespace Web.Controllers
{
    public class FCOLogRawController : DatatableBaseController<FCOLogRawReportViewModel, FCOLogSearchViewModel>
    {
        private readonly IFCOLogService<FCOLogModifyViewModel, FCOLogModifyViewModel, FCOLogRawReportViewModel> _service;
        private readonly ILogger<FCOLogRawController> _logger;

        public FCOLogRawController(IFCOLogService<FCOLogModifyViewModel, FCOLogModifyViewModel, FCOLogRawReportViewModel> service, ILogger<FCOLogRawController> logger, IMapper mapper)
           :
           base(service, logger, mapper, "FCOLog", "Field Change Order Logs", true, true)
        {
            _service = service;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            var dataColumns = new List<DataTableViewModel>();
            dataColumns.AddRange(new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "FCO#",data = "SrNoFormatted",exportColumn="SrNoFormatted"},
                new DataTableViewModel{title = "Additional Information",data = "AdditionalInformation"},
                new DataTableViewModel{title = "Equipment Number",data = "EquipmentNumber", orderable=true},
                new DataTableViewModel{title = "Location",data = "Location", orderable=true},
                new DataTableViewModel{title = "PreTA",data = "PreTA", orderable=true},
                new DataTableViewModel{title = "Shutdown Required",data = "ShutdownRequired", orderable=true},
                new DataTableViewModel{title = "Scaffold Required",data = "ScaffoldRequired", orderable=true},
                new DataTableViewModel{title = "Description Of Finding",data = "DescriptionOfFinding",sortingColumn="DescriptionOfFinding", orderable=true},
                new DataTableViewModel{title = "Analysis Of Alternatives",data = "AnalysisOfAlternatives", orderable=true},
                new DataTableViewModel{title = "Equipment Failure Report",data = "EquipmentFailureReport", orderable=true},
                new DataTableViewModel{title = "Drawings Attached",data = "DrawingsAttached", orderable=true},
                new DataTableViewModel{title = "Schedule Impact",data = "ScheduleImpact", orderable=true},
                new DataTableViewModel{title = "Days Impacted",data = "DaysImpacted", orderable=true},
                new DataTableViewModel{title = "During Execution",data = "DuringExecution", orderable=true},
                new DataTableViewModel{title = "Date",data = "DateFormatted", orderable=true},
                new DataTableViewModel{title = "Approval Date",data = "ApprovalDateFormatted", orderable=true},
                new DataTableViewModel{title = "Contractor",data = "Contractor.Name", orderable=true},
                new DataTableViewModel{title = "Company",data = "Company.Name", orderable=true},
                new DataTableViewModel{title = "Employee",data = "Employee.Name", orderable=true},
                new DataTableViewModel{title = "Department",data = "Department.Name", orderable=true},
                new DataTableViewModel{title = "Unit",data = "Unit.Name", orderable=true},
                new DataTableViewModel{title = "FCO Type",data = "FCOType.Name", orderable=true},
                new DataTableViewModel{title = "FCO Reason",data = "FCOReason.Name", orderable=true},
                new DataTableViewModel{title = "Designated Coordinator",data = "DesignatedCoordinator.Name", orderable=true},
                new DataTableViewModel{title = "Area Execution Lead",data = "AreaExecutionLead.Name", orderable=true},
                new DataTableViewModel{title = "Area Execution Lead Approval Date",data = "AreaExecutionLeadApprovalDate", orderable=true},
                new DataTableViewModel{title = "Business Team Leader",data = "BusinessTeamLeader.Name", orderable=true},
                new DataTableViewModel{title = "Business Team Leader Approval Date",data = "BusinessTeamLeaderApprovalDate", orderable=true},
                new DataTableViewModel{title = "Rejecter",data = "Rejecter.Name", orderable=true},
                new DataTableViewModel{title = "Rejecter Date",data = "RejecterDate", orderable=true},
                new DataTableViewModel{title = "Total Cost Formatted",data = "TotalCostFormatted", orderable=true},
                new DataTableViewModel{title = "Total Hours",data = "TotalHours", orderable=true},
                new DataTableViewModel{title = "Total HeadCount",data = "TotalHeadCount", orderable=true},
                new DataTableViewModel{title = "Material Name",data = "MaterialName", orderable=true},
                new DataTableViewModel{title = "Material Rate",data = "MaterialRate", orderable=true},
                new DataTableViewModel{title = "Equipment Name",data = "EquipmentName", orderable=true},
                new DataTableViewModel{title = "Equipment Rate",data = "EquipmentRate", orderable=true},
                new DataTableViewModel{title = "Shop Name",data = "ShopName", orderable=true},
                new DataTableViewModel{title = "Shop Rate",data = "ShopRate", orderable=true},
            });


            var maxSectionRows = _service.GetMaxSectionCount();
            if (maxSectionRows > 0)
            {
                for (int i = 0; i < maxSectionRows; i++)
                {
                    dataColumns.AddRange(new List<DataTableViewModel>()
                    {
                        new DataTableViewModel{title = $"Name - {i + 1}",data = $"FCOSections.{i}.Name"},
                        new DataTableViewModel{title = $"MN - {i + 1}",data = $"FCOSections.{i}.MN"},
                        new DataTableViewModel{title = $"DU - {i + 1}",data = $"FCOSections.{i}.DU"},
                        new DataTableViewModel{title = $"Type - {i + 1}",data = $"FCOSections.{i}.Type"},
                        new DataTableViewModel{title = $"Craft - {i + 1}",data = $"FCOSections.{i}.Craft.Name"},
                        new DataTableViewModel{title = $"Rate - {i + 1}",data = $"FCOSections.{i}.Rate"},
                        new DataTableViewModel{title = $"Estimate - {i + 1}",data = $"FCOSections.{i}.Estimate"},
                    });
                }
            }

            dataColumns.AddRange(new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Total",data = "Total"},
                new DataTableViewModel{title = "Contingency",data = "Contingency"},
                new DataTableViewModel{title = "Contingencies",data = "Contingencies", orderable=true},
                new DataTableViewModel{title = "Sub Total",data = "SubTotal", orderable=true},
                new DataTableViewModel{title = "Total Labor",data = "TotalLabor", orderable=true},
                new DataTableViewModel{title = "Total Material",data = "Total Material", orderable=true},
                new DataTableViewModel{title = "Total Equipment",data = "TotalEquipment", orderable=true},
                new DataTableViewModel{title = "Total Shop",data = "TotalShop",sortingColumn="DescriptionOfFinding", orderable=true},
                new DataTableViewModel{title = "Section Total",data = "SectionTotal", orderable=true},
            });


            return dataColumns;
        }

        protected override FCOLogSearchViewModel SetDefaultFilters()
        {
            var filters = base.SetDefaultFilters();
            filters.IsRawReport = true;
            return filters;
        }
    }
}
