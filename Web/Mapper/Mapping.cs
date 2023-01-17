using AutoMapper;
using Models.Common;
using Models.TimeOnTools;
using Models.WeldingRodRecord;
using ViewModels;
using ViewModels.Common.Contractor;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.TomeOnTools.PermitType;
using ViewModels.TomeOnTools.ReworkDelay;
using ViewModels.TomeOnTools.Shift;
using ViewModels.TomeOnTools.ShiftDelay;
using ViewModels.TomeOnTools.SOW;
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
                .ReverseMap();
            CreateMap<WRRLog, WRRLogDetailViewModel>()
                .ReverseMap();
            CreateMap<WRRLogModifyViewModel, WRRLogDetailViewModel>().ReverseMap();
            CreateMap<WRRLog, WRRLogBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, WRRLogBriefViewModel>().ReverseMap();

        }
    }
}
