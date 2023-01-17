using AutoMapper;
using Models.WeldingRodRecord;
using ViewModels.WeldingRodRecord.Employee;

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
