using AutoMapper;
using Models.Common;
using Models.WeldingRodRecord;
using ViewModels;
using ViewModels.Common.Contractor;
using ViewModels.Employee;

namespace ChargieApi.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            //Employee
            CreateMap<EmployeeDetailViewModel, Employee>().ReverseMap();

        }
    }
}
