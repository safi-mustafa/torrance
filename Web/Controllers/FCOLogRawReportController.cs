﻿using AutoMapper;
using Enums;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.AppSettingServices;
using ViewModels;
using ViewModels.DataTable;
using Web.Controllers.SharedControllers;

namespace Web.Controllers
{
    public class FCOLogRawReportController : DatatableBaseController<FCOLogRawReportViewModel, FCOLogSearchViewModel>
    {
        private readonly IFCOLogService<FCOLogModifyViewModel, FCOLogModifyViewModel, FCOLogRawReportViewModel> _service;
        private readonly ILogger<FCOLogRawReportController> _logger;

        public FCOLogRawReportController(IFCOLogService<FCOLogModifyViewModel, FCOLogModifyViewModel, FCOLogRawReportViewModel> service, ILogger<FCOLogRawReportController> logger, IMapper mapper)
           :
           base(service, logger, mapper, "FCOLogRawReport", "Field Change Order Logs", true, true)
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
                new DataTableViewModel{title = "Date",data = "DateFormatted", orderable=true},
                new DataTableViewModel{title = "Unit",data = "Unit.Name", orderable=true},
                new DataTableViewModel{title = "Service/Location",data = "Location", orderable=true},
                new DataTableViewModel{title = "Pre TA / TA",data = "PreTA", orderable=true},
                new DataTableViewModel{title = "Shutdown Required?",data = "ShutdownRequired", orderable=true},
                new DataTableViewModel{title = "Scaffold Required?",data = "ScaffoldRequired", orderable=true},
                new DataTableViewModel{title = "Photo",data = "Photo.Url", format = "html", formatValue="image", className = "image-thumbnail"},
                new DataTableViewModel{title = "Loop Identification # & Equipment Number",data = "EquipmentNumber", orderable=true},
                new DataTableViewModel{title = "Description",data = "DescriptionOfFinding",sortingColumn="DescriptionOfFinding", orderable=true},
                new DataTableViewModel{title = "File",data = "File.Url", format = "html", formatValue="file", className = "file-thumbnail"},
                new DataTableViewModel{title = "FCO Type",data = "FCOType.Name", orderable=true},
                new DataTableViewModel{title = "Reason For Change",data = "FCOReason.Name", orderable=true},
                new DataTableViewModel{title = "Other Documentation",data = "OtherDocumentionFormatted", orderable=true},
                new DataTableViewModel{title = "Contractor",data = "Company.Name", orderable=true},
                new DataTableViewModel{title = "Designated Coordinator",data = "DesignatedCoordinator.Name", orderable=true},
                new DataTableViewModel{title = "Approval Date",data = "ApprovalDateFormatted", orderable=true},
                new DataTableViewModel{title = "Area Execution Lead",data = "AreaExecutionLead.Name", orderable=true},
                new DataTableViewModel{title = "Area Execution Lead Approval Date",data = "AreaExecutionLeadApprovalDateFormatted", orderable=true},
                new DataTableViewModel{title = "Business Team Leader",data = "BusinessTeamLeader.Name", orderable=true},
                new DataTableViewModel{title = "Business Team Leader Approval Date",data = "BusinessTeamLeaderApprovalDateFormatted", orderable=true},
                new DataTableViewModel{title = "Rejecter",data = "Rejecter.Name", orderable=true},
                new DataTableViewModel{title = "Rejecter Date",data = "RejecterDateFormatted", orderable=true},
                new DataTableViewModel{title = "Total Cost",data = "TotalCostFormatted", orderable=true},
                new DataTableViewModel{title = "Total Hours",data = "TotalHours", orderable=true},
                new DataTableViewModel{title = "Total Head Count",data = "TotalHeadCount", orderable=true},
                new DataTableViewModel{title = "Material Name",data = "MaterialName", orderable=true},
                new DataTableViewModel{title = "Material Rate",data = "MaterialRate",className="dt-currency", orderable=true},
                new DataTableViewModel{title = "Equipment Name",data = "EquipmentName", orderable=true},
                new DataTableViewModel{title = "Equipment Rate",data = "EquipmentRate",className="dt-currency", orderable=true},
                new DataTableViewModel{title = "Shop Name",data = "ShopName", orderable=true},
                new DataTableViewModel{title = "Shop Rate",data = "ShopRate",className="dt-currency", orderable=true},
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
                        new DataTableViewModel{title = $"Type - {i + 1}",data = $"FCOSections.{i}.OverrideTypeFormatted"},
                        new DataTableViewModel{title = $"Craft - {i + 1}",data = $"FCOSections.{i}.CraftRateFormatted"},
                        new DataTableViewModel{title = $"Rate - {i + 1}",data = $"FCOSections.{i}.Rate",className="dt-currency"},
                        new DataTableViewModel{title = $"Estimate - {i + 1}",data = $"FCOSections.{i}.Estimate"},
                    });
                }
            }

            dataColumns.AddRange(new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Total",data = "Total", className = "dt-currency"},
                new DataTableViewModel{title = "Contingency",data = "Contingency"},
                new DataTableViewModel{title = "Contingencies",data = "Contingencies",className="dt-currency", orderable=true},
                new DataTableViewModel{title = "Sub Total",data = "SubTotal",className="dt-currency", orderable=true},
                new DataTableViewModel{title = "Total Labor",data = "TotalLabor",className="dt-currency", orderable=true},
                new DataTableViewModel{title = "Total Material",data = "TotalMaterial",className="dt-currency", orderable=true},
                new DataTableViewModel{title = "Total Equipment",data = "TotalEquipment",className="dt-currency", orderable=true},
                new DataTableViewModel{title = "Total Shop",data = "TotalShop",className="dt-currency",sortingColumn="DescriptionOfFinding", orderable=true},
                new DataTableViewModel{title = "Section Total",data = "SectionTotal",className="dt-currency", orderable=true},
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
