using AutoMapper;
using Models.Common;
using ViewModels;
using ViewModels.Common.Contractor;

namespace Models.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ContractorModifyViewModel, Contractor>().ReverseMap();
            CreateMap<Contractor, ContractorDetailViewModel>().ReverseMap();
            CreateMap<ContractorModifyViewModel, ContractorDetailViewModel>().ReverseMap();
            CreateMap<Contractor, ContractorBriefViewModel>().ReverseMap();
            CreateMap<BaseBriefVM, ContractorBriefViewModel>().ReverseMap();
            //IgnoreGlobalProperties();
        }
        //private void IgnoreGlobalProperties()
        //{
        //    var properties = typeof(BaseVM).GetProperties();
        //    foreach (var property in properties.Select(x => x.Name).ToList())
        //    {
        //        if (property != "Id")
        //            AddGlobalIgnore(property);
        //    }
        //}

    }
}
