using AutoMapper;
using Models;
using Models.AppSettings;
using Models.Common;
using Models.TimeOnTools;
using Models.WeldingRodRecord;
using ViewModels;
using ViewModels.AppSettings.Map;
using ViewModels.AppSettings.MobileFiles.Dropbox;
using ViewModels.Authentication;
using ViewModels.Common.Contractor;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
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
using Helpers.Models.Shared;
using ViewModels.Authentication.Approver;

namespace TorranceApi.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            //Contractor
            CreateMap<ContractorCreateViewModel, Contractor>().ReverseMap();
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

            //Folder
            CreateMap<FolderModifyViewModel, Folder>()
                .ForMember(x => x.Attachments, d => d.Ignore())
                .ReverseMap();
            CreateMap<FolderCreateViewModel, Folder>()
                .ForMember(x => x.Attachments, d => d.Ignore())
                .ReverseMap();
            CreateMap<Folder, FolderDetailViewModel>().ReverseMap();
            CreateMap<FolderModifyViewModel, FolderDetailViewModel>().ReverseMap();
            CreateMap<Folder, FolderBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, FolderBriefViewModel>().ReverseMap();

            //Employee
            CreateMap<EmployeeModifyViewModel, Employee>().ReverseMap();
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
               .ForMember(dest => dest.ApproverId, act => act.Condition(src => (src.Approver.Id != 0)))
               .ForMember(x => x.Approver, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<WRRLogCreateViewModel, WRRLog>()
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
               .ForMember(dest => dest.ApproverId, act => act.Condition(src => (src.Approver.Id != 0)))
               .ForMember(x => x.Approver, opt => opt.Ignore())
               .ReverseMap();
            CreateMap<WRRLog, ViewModels.WeldingRodRecord.WRRLog.WRRLogDetailViewModel>()
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
                 .ForMember(src => src.EmployeeId, opt => opt.MapFrom(dest => dest.Employee.Id))
               .ForMember(x => x.Employee, opt => opt.Ignore())
                .ForMember(src => src.ApproverId, opt => opt.MapFrom(dest => dest.Approver.Id))
               .ForMember(dest => dest.ApproverId, act => act.Condition(src => (src.Approver.Id != 0)))
               .ForMember(x => x.Approver, opt => opt.Ignore())
               .ForMember(src => src.ForemanId, opt => opt.MapFrom(dest => dest.Foreman.Id))
                .ForMember(dest => dest.ForemanId, act => act.Condition(src => (src.Foreman.Id != 0)))
               .ForMember(x => x.Foreman, opt => opt.Ignore())
                .ForMember(src => src.PermittingIssueId, opt => opt.MapFrom(dest => dest.PermittingIssue.Id))
                .ForMember(x => x.PermittingIssue, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<TOTLogCreateViewModel, TOTLog>()
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
               .ForMember(src => src.EmployeeId, opt => opt.MapFrom(dest => dest.Employee.Id))
               .ForMember(x => x.Employee, opt => opt.Ignore())
               .ForMember(src => src.ApproverId, opt => opt.MapFrom(dest => dest.Approver.Id))
               .ForMember(dest => dest.ApproverId, act => act.Condition(src => (src.Approver.Id != 0)))
               .ForMember(x => x.Approver, opt => opt.Ignore())
               .ForMember(src => src.ForemanId, opt => opt.MapFrom(dest => dest.Foreman.Id))
                .ForMember(dest => dest.ForemanId, act => act.Condition(src => (src.Foreman.Id != 0)))
               .ForMember(x => x.Foreman, opt => opt.Ignore())
                .ForMember(src => src.PermittingIssueId, opt => opt.MapFrom(dest => dest.PermittingIssue.Id))
                .ForMember(x => x.PermittingIssue, opt => opt.Ignore())
               .ReverseMap();
            CreateMap<TOTLog, TOTLogDetailViewModel>()
                .ReverseMap();
            CreateMap<TOTLogModifyViewModel, TOTLogDetailViewModel>().ReverseMap();
            CreateMap<TOTLog, TOTLogBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, TOTLogBriefViewModel>().ReverseMap();


            //User
            CreateMap<ToranceUser, UserBriefViewModel>()
                .ForMember(src => src.Name, opt => opt.MapFrom(dest => dest.Email))
                .ReverseMap();
            CreateMap<SignUpModel, ToranceUser>().ReverseMap();
            CreateMap<UserDetailViewModel, ToranceUser>().ReverseMap();
            CreateMap<UserDetailViewModel, Employee>().ReverseMap();

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


            //SearchModels
            CreateMap<WRRLogSearchViewModel, WRRLogAPISearchViewModel>()
                .ForMember(src => src.EmployeeId, opt => opt.MapFrom(dest => dest.Employee.Id))
                .ForMember(src => src.DepartmentId, opt => opt.MapFrom(dest => dest.Department.Id))
                .ForMember(src => src.LocationId, opt => opt.MapFrom(dest => dest.Location.Id))
                .ForMember(src => src.UnitId, opt => opt.MapFrom(dest => dest.Unit.Id))
                .ReverseMap();
            CreateMap<EmployeeSearchViewModel, EmployeeAPISearchViewModel>()
                 //.ForMember(src => src.ApproverId, opt => opt.MapFrom(dest => dest.Approver.Id))
                .ReverseMap();
            CreateMap<TOTLogSearchViewModel, TOTLogAPISearchViewModel>()
                .ForMember(src => src.ContractorId, opt => opt.MapFrom(dest => dest.Contractor.Id))
                .ForMember(src => src.DepartmentId, opt => opt.MapFrom(dest => dest.Department.Id))
                //.ForMember(src => src.ApproverId, opt => opt.MapFrom(dest => dest.Approver.Id))
                //.ForMember(src => src.ForemanId, opt => opt.MapFrom(dest => dest.Foreman.Id))
                .ForMember(src => src.UnitId, opt => opt.MapFrom(dest => dest.Unit.Id))
                .ReverseMap();


            //Dropbox
            CreateMap<DropboxModifyViewModel, Dropbox>().ReverseMap();
            CreateMap<Dropbox, DropboxDetailViewModel>().ReverseMap();
            CreateMap<DropboxModifyViewModel, DropboxDetailViewModel>().ReverseMap();

            //Attachment
            CreateMap<AttachmentVM, Attachment>()
               .ForMember(d => d.FolderId, s => s.MapFrom(x => x.Folder.Id))
               .ForMember(d => d.Folder, s => s.Ignore())
               .ReverseMap();
            CreateMap<AttachmentResponseVM, Attachment>().ReverseMap();

        }
    }
}
