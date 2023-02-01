using AutoMapper;
using Helpers.Models.Shared;
using Models.AppSettings;
using Models.Common;
using Models.OverrideLogs;
using Models.TimeOnTools;
using Models.WeldingRodRecord;
using ViewModels;
using ViewModels.AppSettings.Map;
using ViewModels.AppSettings.MobileFiles.Dropbox;
using ViewModels.Authentication;
using ViewModels.Authentication.Approver;
using ViewModels.Common.Contractor;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.OverrideLogs;
using ViewModels.OverrideLogs.ORLog;
using ViewModels.Shared;
using ViewModels.TomeOnTools.PermittingIssue;
using ViewModels.TomeOnTools.PermitType;
using ViewModels.TomeOnTools.ReworkDelay;
using ViewModels.TomeOnTools.Shift;
using ViewModels.TomeOnTools.ShiftDelay;
using ViewModels.TomeOnTools.SOW;
using ViewModels.TomeOnTools.TOTLog;
using ViewModels.WeldingRodRecord.Employee;
using ViewModels.WeldingRodRecord.Location;
using ViewModels.WeldingRodRecord.RodType;
using ViewModels.WeldingRodRecord.WeldMethod;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Models.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            //Contractor
            CreateMap<ContractorModifyViewModel, Contractor>().ReverseMap();
            CreateMap<Contractor, ContractorDetailViewModel>().ReverseMap();
            CreateMap<ContractorModifyViewModel, ContractorDetailViewModel>().ReverseMap();
            CreateMap<Contractor, ContractorBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, ContractorBriefViewModel>().ReverseMap();

            //Department
            CreateMap<DepartmentModifyViewModel, Department>().ReverseMap();
            CreateMap<Department, DepartmentDetailViewModel>().ReverseMap();
            CreateMap<DepartmentModifyViewModel, DepartmentDetailViewModel>().ReverseMap();
            CreateMap<Department, DepartmentBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, DepartmentBriefViewModel>().ReverseMap();

            //Unit
            CreateMap<UnitModifyViewModel, Unit>().ReverseMap();
            CreateMap<Unit, UnitDetailViewModel>().ReverseMap();
            CreateMap<UnitModifyViewModel, UnitDetailViewModel>().ReverseMap();
            CreateMap<Unit, UnitBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, UnitBriefViewModel>().ReverseMap();


            //WeldMethod
            CreateMap<WeldMethodModifyViewModel, WeldMethod>().ReverseMap();
            CreateMap<WeldMethod, WeldMethodDetailViewModel>().ReverseMap();
            CreateMap<WeldMethodModifyViewModel, WeldMethodDetailViewModel>().ReverseMap();
            CreateMap<WeldMethod, WeldMethodBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, WeldMethodBriefViewModel>().ReverseMap();

            //RodType
            CreateMap<RodTypeModifyViewModel, RodType>().ReverseMap();
            CreateMap<RodType, RodTypeDetailViewModel>().ReverseMap();
            CreateMap<RodTypeModifyViewModel, RodTypeDetailViewModel>().ReverseMap();
            CreateMap<RodType, RodTypeBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, RodTypeBriefViewModel>().ReverseMap();

            //Location
            CreateMap<LocationModifyViewModel, Location>().ReverseMap();
            CreateMap<Location, LocationDetailViewModel>().ReverseMap();
            CreateMap<LocationModifyViewModel, LocationDetailViewModel>().ReverseMap();
            CreateMap<Location, LocationBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, LocationBriefViewModel>().ReverseMap();

            //PermitType
            CreateMap<PermitTypeModifyViewModel, PermitType>().ReverseMap();
            CreateMap<PermitType, PermitTypeDetailViewModel>().ReverseMap();
            CreateMap<PermitTypeModifyViewModel, PermitTypeDetailViewModel>().ReverseMap();
            CreateMap<PermitType, PermitTypeBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, PermitTypeBriefViewModel>().ReverseMap();


            //PermittingIssue
            CreateMap<PermittingIssueModifyViewModel, PermittingIssue>().ReverseMap();
            CreateMap<PermittingIssue, PermittingIssueDetailViewModel>().ReverseMap();
            CreateMap<PermittingIssueModifyViewModel, PermittingIssueDetailViewModel>().ReverseMap();
            CreateMap<PermittingIssue, PermittingIssueBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, PermittingIssueBriefViewModel>().ReverseMap();

            //ReworkDelay
            CreateMap<ReworkDelayModifyViewModel, ReworkDelay>().ReverseMap();
            CreateMap<ReworkDelay, ReworkDelayDetailViewModel>().ReverseMap();
            CreateMap<ReworkDelayModifyViewModel, ReworkDelayDetailViewModel>().ReverseMap();
            CreateMap<ReworkDelay, ReworkDelayBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, ReworkDelayBriefViewModel>().ReverseMap();

            //ShiftDelay
            CreateMap<ShiftDelayModifyViewModel, ShiftDelay>().ReverseMap();
            CreateMap<ShiftDelay, ShiftDelayDetailViewModel>().ReverseMap();
            CreateMap<ShiftDelayModifyViewModel, ShiftDelayDetailViewModel>().ReverseMap();
            CreateMap<ShiftDelay, ShiftDelayBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, ShiftDelayBriefViewModel>().ReverseMap();

            //Shift
            CreateMap<ShiftModifyViewModel, Shift>().ReverseMap();
            CreateMap<Shift, ShiftDetailViewModel>().ReverseMap();
            CreateMap<ShiftModifyViewModel, ShiftDetailViewModel>().ReverseMap();
            CreateMap<Shift, ShiftBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, ShiftBriefViewModel>().ReverseMap();

            //SOW
            CreateMap<SOWModifyViewModel, StatementOfWork>().ReverseMap();
            CreateMap<StatementOfWork, SOWDetailViewModel>().ReverseMap();
            CreateMap<SOWModifyViewModel, SOWDetailViewModel>().ReverseMap();
            CreateMap<StatementOfWork, SOWBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, SOWBriefViewModel>().ReverseMap();

            //Employee
            CreateMap<EmployeeModifyViewModel, Employee>()
                .ForMember(src => src.ContractorId, opt => opt.MapFrom(dest => dest.Contractor.Id))
                .ForMember(x => x.Contractor, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Employee, EmployeeDetailViewModel>().ReverseMap();
            CreateMap<EmployeeModifyViewModel, EmployeeDetailViewModel>().ReverseMap();
            CreateMap<Employee, EmployeeBriefViewModel>()
                .ForMember(src => src.Name, opt => opt.MapFrom(dest => dest.FirstName + " " + dest.LastName))
                .ReverseMap();
            CreateMap<BaseBriefVM, EmployeeBriefViewModel>().ReverseMap();

            //WRRLog
            CreateMap<WRRLogModifyViewModel, WRRLog>()
                .ForMember(src => src.DepartmentId, opt => opt.MapFrom(dest => dest.Department.Id))
                .ForMember(x => x.Department, opt => opt.Ignore())
                .ForMember(src => src.WeldMethodId, opt => opt.MapFrom(dest => dest.WeldMethod.Id))
                .ForMember(x => x.WeldMethod, opt => opt.Ignore())
                .ForMember(src => src.EmployeeId, opt => opt.MapFrom(dest => dest.Employee.Id))
                .ForMember(x => x.Employee, opt => opt.Ignore())
                .ForMember(src => src.RodTypeId, opt => opt.MapFrom(dest => dest.RodType.Id))
                .ForMember(x => x.RodType, opt => opt.Ignore())
                .ForMember(src => src.LocationId, opt => opt.MapFrom(dest => dest.Location.Id))
                .ForMember(x => x.Location, opt => opt.Ignore())
                .ForMember(src => src.UnitId, opt => opt.MapFrom(dest => dest.Unit.Id))
                .ForMember(x => x.Unit, opt => opt.Ignore())
                .ForMember(src => src.ContractorId, opt => opt.MapFrom(dest => dest.Contractor.Id))
                .ForMember(x => x.Contractor, opt => opt.Ignore())
                .ForMember(src => src.ApproverId, opt => opt.MapFrom(dest => dest.Approver.Id))
                .ForMember(x => x.Approver, opt => opt.Ignore())
                .ForMember(dest => dest.ApproverId, act => act.Condition(src => (src.Approver.Id != 0)))
                .ReverseMap();
            CreateMap<WRRLog, ViewModels.WeldingRodRecord.WRRLog.WRRLogDetailViewModel>()
                .ForMember(dest => dest.Approver, act => act.Condition(src => (src.Approver != null)))
                .ReverseMap();
            CreateMap<WRRLogModifyViewModel, ViewModels.WeldingRodRecord.WRRLog.WRRLogDetailViewModel>().ReverseMap();
            CreateMap<WRRLog, WRRLogBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, WRRLogBriefViewModel>().ReverseMap();

            //TOTLog
            CreateMap<TOTLogModifyViewModel, TOTLog>()
                .ForMember(src => src.DepartmentId, opt => opt.MapFrom(dest => dest.Department.Id))
                .ForMember(x => x.Department, opt => opt.Ignore())
                .ForMember(src => src.ContractorId, opt => opt.MapFrom(dest => dest.Contractor.Id))
                .ForMember(x => x.Contractor, opt => opt.Ignore())
                .ForMember(src => src.ReworkDelayId, opt => opt.MapFrom(dest => dest.ReworkDelay.Id))
                .ForMember(x => x.ReworkDelay, opt => opt.Ignore())
                .ForMember(src => src.ShiftDelayId, opt => opt.MapFrom(dest => dest.ShiftDelay.Id))
                .ForMember(x => x.ShiftDelay, opt => opt.Ignore())
                .ForMember(src => src.ShiftId, opt => opt.MapFrom(dest => dest.Shift.Id))
                .ForMember(x => x.Shift, opt => opt.Ignore())
                .ForMember(src => src.UnitId, opt => opt.MapFrom(dest => dest.Unit.Id))
                .ForMember(x => x.Unit, opt => opt.Ignore())
                .ForMember(src => src.PermitTypeId, opt => opt.MapFrom(dest => dest.PermitType.Id))
                .ForMember(x => x.PermitType, opt => opt.Ignore())
                .ForMember(src => src.PermittingIssueId, opt => opt.MapFrom(dest => dest.PermittingIssue.Id))
                .ForMember(x => x.PermittingIssue, opt => opt.Ignore())
                .ForMember(src => src.ApproverId, opt => opt.MapFrom(dest => dest.Approver.Id))
                .ForMember(x => x.Approver, opt => opt.Ignore())
                .ForMember(dest => dest.ApproverId, act => act.Condition(src => (src.Approver.Id != 0)))
                .ForMember(src => src.ForemanId, opt => opt.MapFrom(dest => dest.Foreman.Id))
                .ForMember(dest => dest.ForemanId, act => act.Condition(src => (src.Foreman.Id != 0)))
                .ForMember(x => x.Foreman, opt => opt.Ignore())
                .ForMember(src => src.EmployeeId, opt => opt.MapFrom(dest => dest.Employee.Id))
                .ForMember(x => x.Employee, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<TOTLog, ViewModels.OverrideLogs.ORLog.TOTLogDetailViewModel>()
                .ForMember(dest => dest.Approver, act => act.Condition(src => (src.Approver != null)))
                .ForMember(dest => dest.Foreman, act => act.Condition(src => (src.Foreman != null)))
                .ForMember(dest => dest.Employee, act => act.Condition(src => (src.Employee != null)))
                .ReverseMap();
            CreateMap<TOTLogModifyViewModel, ViewModels.OverrideLogs.ORLog.TOTLogDetailViewModel>()
                .ReverseMap();
            CreateMap<TOTLog, TOTLogBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, TOTLogBriefViewModel>().ReverseMap();


            //User
            CreateMap<ToranceUser, UserBriefViewModel>()
                .ForMember(src => src.Name, opt => opt.MapFrom(dest => dest.Email))
                .ReverseMap();
            CreateMap<SignUpModel, ToranceUser>().ReverseMap();

            //Employee
            CreateMap<BaseBriefVM, EmployeeBriefViewModel>().ReverseMap();
            CreateMap<Employee, EmployeeBriefViewModel>()
                .ForMember(src => src.Name, opt => opt.MapFrom(dest => dest.FirstName + " " + dest.LastName))
                .ReverseMap();
            CreateMap<Employee, BaseBriefVM>()
                .ForMember(src => src.Name, opt => opt.MapFrom(dest => dest.FirstName + " " + dest.LastName))
                .ReverseMap();
            CreateMap<EmployeeModifyViewModel, Employee>()
               //.ForMember(src => src.ApproverId, opt => opt.MapFrom(dest => dest.Approver.Id))
               //.ForMember(x => x.Approver, opt => opt.Ignore())
               .ReverseMap();
            CreateMap<Employee, EmployeeDetailViewModel>()
               .ReverseMap();
            CreateMap<EmployeeModifyViewModel, EmployeeDetailViewModel>().ReverseMap();
            CreateMap<EmployeeModifyViewModel, SignUpModel>()
                .ForMember(src => src.UserName, opt => opt.MapFrom(dest => dest.Email))
               .ReverseMap();


            //Folder
            CreateMap<Folder, BaseBriefVM>().ReverseMap();
            CreateMap<FolderCreateViewModel, Folder>()
                .ForMember(x => x.Attachments, d => d.Ignore())
                .ForMember(d => d.IconUrl, s => s.Condition(src => (!string.IsNullOrEmpty(src.IconUrl))))
                .ReverseMap();
            CreateMap<FolderModifyViewModel, Folder>()
                .ForMember(d => d.IconUrl, s => s.Condition(src => (!string.IsNullOrEmpty(src.IconUrl))))
                .ForMember(x => x.Attachments, d => d.Ignore())
                .ReverseMap();
            CreateMap<Folder, FolderDetailViewModel>().ReverseMap();
            CreateMap<FolderModifyViewModel, FolderDetailViewModel>().ReverseMap();
            CreateMap<Folder, FolderBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, FolderBriefViewModel>().ReverseMap();


            //Map
            CreateMap<MapModifyViewModel, Map>()
              .ReverseMap();
            CreateMap<Map, MapDetailViewModel>().ReverseMap();
            CreateMap<MapModifyViewModel, MapDetailViewModel>().ReverseMap();
            CreateMap<Map, BaseBriefVM>().ReverseMap();

            //Dropbox
            CreateMap<DropboxModifyViewModel, Dropbox>()
              .ReverseMap();
            CreateMap<Dropbox, DropboxDetailViewModel>().ReverseMap();
            CreateMap<DropboxModifyViewModel, DropboxDetailViewModel>().ReverseMap();
            CreateMap<Dropbox, BaseBriefVM>().ReverseMap();


            //Attachment
            CreateMap<AttachmentVM, Attachment>()
                .ForMember(d => d.FolderId, s => s.MapFrom(x => x.Folder.Id))
                .ForMember(d => d.Folder, s => s.Ignore())
                .ReverseMap();
            CreateMap<AttachmentResponseVM, Attachment>().ReverseMap();
            CreateMap<AttachmentResponseVM, AttachmentVM>().ReverseMap();
            CreateMap<AttachmentModifyViewModel, Attachment>()
                .ForMember(dest => dest.Url, act => act.Condition(src => (!string.IsNullOrEmpty(src.Url))))
                .ForMember(dest => dest.Type, act => act.Condition(src => (!string.IsNullOrEmpty(src.Type))))
                .ForMember(d => d.FolderId, d => d.MapFrom(x => x.Folder.Id))
                .ForMember(dest => dest.FolderId, act => act.Condition(src => src.Folder != null))
                .ForMember(d => d.Folder, s => s.Ignore())
                .ReverseMap();
            CreateMap<AttachmentResponseVM, AttachmentModifyViewModel>().ReverseMap();


            //Approver
            CreateMap<UserUpdateViewModel, ToranceUser>()
              .ReverseMap();
            CreateMap<ToranceUser, UserDetailViewModel>().ReverseMap();
            CreateMap<UserUpdateViewModel, UserDetailViewModel>().ReverseMap();
            CreateMap<UserUpdateViewModel, SignUpModel>().ReverseMap();


            //CraftRate
            CreateMap<CraftRateModifyViewModel, CraftRate>().ReverseMap();
            CreateMap<CraftRate, CraftRateDetailViewModel>().ReverseMap();
            CreateMap<CraftRateModifyViewModel, CraftRateDetailViewModel>().ReverseMap();
            CreateMap<CraftRate, CraftRateBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, CraftRateBriefViewModel>().ReverseMap();

            //CraftSkill
            CreateMap<CraftSkillModifyViewModel, CraftSkill>().ReverseMap();
            CreateMap<CraftSkill, CraftSkillDetailViewModel>().ReverseMap();
            CreateMap<CraftSkillModifyViewModel, CraftSkillDetailViewModel>().ReverseMap();
            CreateMap<CraftSkill, CraftSkillBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, CraftSkillBriefViewModel>().ReverseMap();

            //LeadPlanner
            CreateMap<LeadPlannerModifyViewModel, LeadPlanner>().ReverseMap();
            CreateMap<LeadPlanner, LeadPlannerDetailViewModel>().ReverseMap();
            CreateMap<LeadPlannerModifyViewModel, LeadPlannerDetailViewModel>().ReverseMap();
            CreateMap<LeadPlanner, LeadPlannerBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, LeadPlannerBriefViewModel>().ReverseMap();

            //OverrideType
            CreateMap<OverrideTypeModifyViewModel, OverrideType>().ReverseMap();
            CreateMap<OverrideType, OverrideTypeDetailViewModel>().ReverseMap();
            CreateMap<OverrideTypeModifyViewModel, OverrideTypeDetailViewModel>().ReverseMap();
            CreateMap<OverrideType, OverrideTypeBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, OverrideTypeBriefViewModel>().ReverseMap();

            //ReasonForRequest
            CreateMap<ReasonForRequestModifyViewModel, ReasonForRequest>().ReverseMap();
            CreateMap<ReasonForRequest, ReasonForRequestDetailViewModel>().ReverseMap();
            CreateMap<ReasonForRequestModifyViewModel, ReasonForRequestDetailViewModel>().ReverseMap();
            CreateMap<ReasonForRequest, ReasonForRequestBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, ReasonForRequestBriefViewModel>().ReverseMap();

        }
    }
}
