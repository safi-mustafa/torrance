﻿using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using Helpers.Models.Shared;
using Models.AppSettings;
using Models.Common;
using Models.OverrideLogs;
using Models.TimeOnTools;
using Models.WeldingRodRecord;
using ViewModels;
using ViewModels.AppSettings.Administrator;
using ViewModels.AppSettings.CompanyManager;
using ViewModels.AppSettings.Map;
using ViewModels.AppSettings.MobileFiles.Dropbox;
using ViewModels.Authentication;
using ViewModels.Authentication.Approver;
using ViewModels.Authentication.User;
using ViewModels.Common.Company;
using ViewModels.Common.Contractor;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Notification;
using ViewModels.OverrideLogs;
using ViewModels.OverrideLogs.ORLog;
using ViewModels.Shared;
using ViewModels.TimeOnTools;
using ViewModels.TimeOnTools.OngoingWorkDelay;
using ViewModels.TimeOnTools.PermittingIssue;
using ViewModels.TimeOnTools.PermitType;
using ViewModels.TimeOnTools.ReworkDelay;
using ViewModels.TimeOnTools.Shift;
using ViewModels.TimeOnTools.ShiftDelay;
using ViewModels.TimeOnTools.SOW;
using ViewModels.TimeOnTools.StartOfWorkDelay;
using ViewModels.TimeOnTools.TOTLog;
using ViewModels.WeldingRodRecord;
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

            //Company
            CreateMap<CompanyModifyViewModel, Company>().ReverseMap();
            CreateMap<Company, CompanyDetailViewModel>().ReverseMap();
            CreateMap<CompanyModifyViewModel, CompanyDetailViewModel>().ReverseMap();
            CreateMap<Company, CompanyBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, CompanyBriefViewModel>().ReverseMap();

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

            //DelayType
            CreateMap<DelayTypeModifyViewModel, DelayType>().ReverseMap();
            CreateMap<DelayType, DelayTypeDetailViewModel>().ReverseMap();
            CreateMap<DelayTypeModifyViewModel, DelayTypeDetailViewModel>().ReverseMap();
            CreateMap<DelayType, DelayTypeBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, DelayTypeBriefViewModel>().ReverseMap();

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

            //StartOfWorkDelay
            CreateMap<StartOfWorkDelayModifyViewModel, StartOfWorkDelay>().ReverseMap();
            CreateMap<StartOfWorkDelay, StartOfWorkDelayDetailViewModel>().ReverseMap();
            CreateMap<StartOfWorkDelayModifyViewModel, StartOfWorkDelayDetailViewModel>().ReverseMap();
            CreateMap<StartOfWorkDelay, StartOfWorkDelayBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, StartOfWorkDelayBriefViewModel>().ReverseMap();


            //OngoingWorkDelay
            CreateMap<OngoingWorkDelayModifyViewModel, OngoingWorkDelay>().ReverseMap();
            CreateMap<OngoingWorkDelay, OngoingWorkDelayDetailViewModel>().ReverseMap();
            CreateMap<OngoingWorkDelayModifyViewModel, OngoingWorkDelayDetailViewModel>().ReverseMap();
            CreateMap<OngoingWorkDelay, OngoingWorkDelayBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, OngoingWorkDelayBriefViewModel>().ReverseMap();

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
                .ForMember(src => src.CompanyId, opt => opt.MapFrom(dest => dest.Company.Id))
                .ForMember(x => x.Company, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Employee, EmployeeDetailViewModel>().ReverseMap();
            CreateMap<EmployeeModifyViewModel, EmployeeDetailViewModel>().ReverseMap();

            CreateMap<ToranceUser, EmployeeBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, EmployeeBriefViewModel>().ReverseMap();
            CreateMap<Employee, EmployeeBriefViewModel>()
                .ForMember(src => src.Name, opt => opt.MapFrom(dest => dest.FirstName + " " + dest.LastName))
                .ReverseMap();

            CreateMap<Employee, BaseBriefVM>()
                .ForMember(src => src.Name, opt => opt.MapFrom(dest => dest.FirstName + " " + dest.LastName))
                .ReverseMap();
            CreateMap<EmployeeModifyViewModel, SignUpModel>()
                .ForMember(src => src.UserName, opt => opt.MapFrom(dest => dest.Email))
               .ReverseMap();

            CreateMap<EmployeeModifyViewModel, EmployeeExcelViewModel>()
                .ForMember(d => d.CompanyName, s => s.MapFrom(x => x.Company.Name)).ReverseMap();


            //Company Manager
            CreateMap<ToranceUser, CompanyManagerDetailViewModel>().ReverseMap();

            CreateMap<CompanyManagerDetailViewModel, CompanyManagerModifyViewModel>().ReverseMap();

            //Administrator
            CreateMap<ToranceUser, AdministratorDetailViewModel>().ReverseMap();

            CreateMap<AdministratorDetailViewModel, AdministratorModifyViewModel>().ReverseMap();


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
                .ForMember(x => x.Company, o => o.Ignore())
                .ReverseMap();
            CreateMap<WRRLog, WRRLogDetailViewModel>()
                .ForMember(dest => dest.Approver, act => act.Condition(src => (src.Approver != null)))

                .ForMember(dest => dest.Employee, act => act.Condition(src => (src.Employee != null)))
                .ReverseMap();
            CreateMap<WRRLogModifyViewModel, WRRLogDetailViewModel>()
                .ReverseMap();
            CreateMap<WRRLog, WRRLogBriefViewModel>()
                .ReverseMap();
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
                .ForMember(src => src.StartOfWorkDelayId, opt => opt.MapFrom(dest => dest.StartOfWorkDelay.Id))
                .ForMember(x => x.StartOfWorkDelay, opt => opt.Ignore())
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
                //.ForMember(src => src.ForemanId, opt => opt.MapFrom(dest => dest.Foreman.Id))
                //.ForMember(dest => dest.ForemanId, act => act.Condition(src => (src.Foreman.Id != 0)))
                //.ForMember(x => x.Foreman, opt => opt.Ignore())
                .ForMember(src => src.ReasonForRequestId, opt => opt.MapFrom(dest => dest.ReasonForRequest.Id))
                .ForMember(x => x.ReasonForRequest, opt => opt.Ignore())
                .ForMember(src => src.EmployeeId, opt => opt.MapFrom(dest => dest.Employee.Id))
                .ForMember(x => x.Employee, opt => opt.Ignore())
                .ForMember(src => src.DelayTypeId, opt => opt.MapFrom(dest => dest.DelayType.Id))
                .ForMember(x => x.DelayType, opt => opt.Ignore())
                .ForMember(x => x.Company, o => o.Ignore())
                .ForMember(src => src.OngoingWorkDelayId, opt => opt.MapFrom(dest => dest.OngoingWorkDelay.Id))
                .ForMember(x => x.OngoingWorkDelay, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<TOTLog, TOTLogDetailViewModel>()
                .ForMember(dest => dest.Approver, act => act.Condition(src => (src.Approver != null)))
                .ForMember(dest => dest.Employee, act => act.Condition(src => (src.Employee != null)))
                .ReverseMap();

            CreateMap<DelayTypeBriefViewModel, DelayTypeBriefViewModel>()
            .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Identifier));

            CreateMap<TOTLogDetailViewModel, TOTLogModifyViewModel>()
                .ForMember(dest => dest.DelayType, opt => opt.MapFrom(src => src.DelayType))
                .ForPath(dest => dest.DelayType.Identifier, opt => opt.MapFrom(src => src.DelayType.Identifier))
                .ReverseMap();

            CreateMap<TOTLog, TOTLogBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, TOTLogBriefViewModel>().ReverseMap();

            //User


            CreateMap<ToranceUser, EmployeeBriefViewModel>()
              .ForMember(src => src.Name, opt => opt.MapFrom(dest => dest.FullName))
              .ReverseMap();
            CreateMap<ToranceUser, UserBriefViewModel>()
                .ForMember(src => src.Name, opt => opt.MapFrom(dest => dest.FullName))
                .ReverseMap();
            CreateMap<ToranceUser, EmployeeDetailViewModel>()
                .ForMember(dest => dest.Company, act => act.Condition(src => (src.Company != null)))
                .ReverseMap();
            CreateMap<ToranceUser, ApproverBriefViewModel>()
               //.ConstructUsing((src, context) => new ApproverBriefViewModel(true))
               .ForMember(src => src.Name, opt => opt.MapFrom(dest => dest.FullName))
               .ReverseMap();
            CreateMap<SignUpModel, ToranceUser>()
                .ForMember(src => src.CompanyId, opt => opt.MapFrom(dest => dest.Company.Id))
                .ForMember(x => x.Company, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<UserUpdateViewModel, ToranceUser>()
              .ReverseMap();
            CreateMap<UserDetailViewModel, ToranceUser>().ReverseMap();
            CreateMap<UserDetailViewModel, Employee>().ReverseMap();
            CreateMap<UserUpdateViewModel, UserDetailViewModel>().ReverseMap();
            CreateMap<UserUpdateViewModel, SignUpModel>().ReverseMap();

            //Approver
            CreateMap<ApproverModifyViewModel, ToranceUser>().ReverseMap();
            CreateMap<ApproverModifyViewModel, SignUpModel>().ReverseMap();
            CreateMap<ToranceUser, ApproverDetailViewModel>().ReverseMap();
            CreateMap<UserDetailViewModel, ApproverDetailViewModel>().ReverseMap();
            CreateMap<ApproverModifyViewModel, ApproverDetailViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, ApproverBriefViewModel>()
                //.ConstructUsing((src, context) => new ApproverBriefViewModel(true))
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

            ////CraftRate
            //CreateMap<CraftRate, CraftRateBriefViewModel>().ReverseMap();
            //CreateMap<BaseBriefVM, CraftRateBriefViewModel>().ReverseMap();
            //CreateMap<CraftRateBriefViewModel, CraftRateBriefViewModel>()
            //    .ForMember(src => src.Name, opt => opt.MapFrom(dest => dest.Name))
            //    .ReverseMap();
            //CreateMap<CraftRateModifyViewModel, CraftRate>().ReverseMap();
            //CreateMap<CraftRate, CraftRateDetailViewModel>().ReverseMap();
            //CreateMap<CraftRateModifyViewModel, CraftRateDetailViewModel>().ReverseMap();

            //CraftSkill
            CreateMap<CraftSkill, CraftSkillBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, CraftSkillBriefViewModel>().ReverseMap();
            CreateMap<CraftSkillModifyViewModel, CraftSkill>().ReverseMap();
            CreateMap<CraftSkill, CraftSkillDetailViewModel>().ReverseMap();
            CreateMap<CraftSkill, BaseBriefVM>().ReverseMap();
            CreateMap<CraftSkillModifyViewModel, CraftSkillDetailViewModel>().ReverseMap();

            //LeadPlanner
            CreateMap<LeadPlanner, LeadPlannerBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, LeadPlannerBriefViewModel>().ReverseMap();
            CreateMap<LeadPlannerModifyViewModel, LeadPlanner>().ReverseMap();
            CreateMap<LeadPlanner, LeadPlannerDetailViewModel>().ReverseMap();
            CreateMap<LeadPlannerModifyViewModel, LeadPlannerDetailViewModel>().ReverseMap();

            //OverrideType
            CreateMap<OverrideType, OverrideTypeBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, OverrideTypeBriefViewModel>().ReverseMap();
            CreateMap<OverrideTypeModifyViewModel, OverrideType>().ReverseMap();
            CreateMap<OverrideType, OverrideTypeDetailViewModel>().ReverseMap();
            CreateMap<OverrideTypeModifyViewModel, OverrideTypeDetailViewModel>().ReverseMap();

            //ReasonForRequest
            CreateMap<ReasonForRequest, ReasonForRequestBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, ReasonForRequestBriefViewModel>().ReverseMap();
            CreateMap<ReasonForRequestModifyViewModel, ReasonForRequest>().ReverseMap();
            CreateMap<ReasonForRequest, ReasonForRequestDetailViewModel>().ReverseMap();
            CreateMap<ReasonForRequestModifyViewModel, ReasonForRequestDetailViewModel>().ReverseMap();


            //ORLog

            CreateMap<OverrideLog, ORLogDetailViewModel>()
                 .ForMember(dest => dest.Approver, act => act.Condition(src => (src.Approver != null)))
                 .ForMember(dest => dest.Employee, act => act.Condition(src => (src.Employee != null)))
                .ReverseMap();
            CreateMap<ORLogModifyViewModel, ORLogDetailViewModel>()
                .ReverseMap();
            CreateMap<ORLogModifyViewModel, OverrideLog>()
                .ForMember(src => src.EmployeeId, opt => opt.MapFrom(dest => dest.Employee.Id))
                .ForMember(x => x.Employee, opt => opt.Ignore())
                .ForMember(src => src.CompanyId, opt => opt.MapFrom(dest => dest.Company.Id))
                .ForMember(x => x.Company, opt => opt.Ignore())
                .ForMember(src => src.DepartmentId, opt => opt.MapFrom(dest => dest.Department.Id))
                .ForMember(x => x.Department, opt => opt.Ignore())
                .ForMember(src => src.ShiftId, opt => opt.MapFrom(dest => dest.Shift.Id))
                .ForMember(x => x.Shift, opt => opt.Ignore())
                //.ForMember(src => src.ReworkDelayId, opt => opt.MapFrom(dest => dest.ReworkDelay.Id))
                //.ForMember(x => x.ReworkDelay, opt => opt.Ignore())
                //.ForMember(src => src.ShiftDelayId, opt => opt.MapFrom(dest => dest.ShiftDelay.Id))
                //.ForMember(x => x.ShiftDelay, opt => opt.Ignore())
                //.ForMember(src => src.StartOfWorkDelayId, opt => opt.MapFrom(dest => dest.StartOfWorkDelay.Id))
                //.ForMember(x => x.StartOfWorkDelay, opt => opt.Ignore())
                .ForMember(src => src.DelayTypeId, opt => opt.MapFrom(dest => dest.DelayType.Id))
                .ForMember(x => x.DelayType, opt => opt.Ignore())
                //.ForMember(src => src.CraftRateId, opt => opt.MapFrom(dest => dest.CraftRate.Id))
                //.ForMember(x => x.CraftRate, opt => opt.Ignore())
                //.ForMember(src => src.CraftSkillId, opt => opt.MapFrom(dest => dest.CraftSkill.Id))
                //.ForMember(x => x.CraftSkill, opt => opt.Ignore())
                .ForMember(src => src.ReasonForRequestId, opt => opt.MapFrom(dest => dest.ReasonForRequest.Id))
                .ForMember(x => x.ReasonForRequest, opt => opt.Ignore())
                //.ForMember(src => src.OverrideTypeId, opt => opt.MapFrom(dest => dest.OverrideType.Id))
                //.ForMember(x => x.OverrideType, opt => opt.Ignore())
                .ForMember(src => src.UnitId, opt => opt.MapFrom(dest => dest.Unit.Id))
                .ForMember(x => x.Unit, opt => opt.Ignore())
                .ForMember(src => src.ApproverId, opt => opt.MapFrom(dest => dest.Approver.Id))
                .ForMember(x => x.Approver, opt => opt.Ignore())
                .ForMember(dest => dest.ApproverId, act => act.Condition(src => (src.Approver.Id != 0)))
                .ForMember(src => src.ClippedEmployeesUrl, opt => opt.MapFrom(dest => dest.ClippedEmployees.Url))
                .ForMember(dest => dest.ClippedEmployeesUrl, act => act.Condition(src => (src.ClippedEmployees != null)))
                .ReverseMap();

            CreateMap<OverrideLog, ORLogBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, ORLogBriefViewModel>().ReverseMap();


            CreateMap<Notification, NotificationViewModel>().ReverseMap();
        }
    }
}
