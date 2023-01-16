using AutoMapper;
using Models.Common;
using ViewModels;
using ViewModels.Common.Contractor;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;

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
        }
    }
}
