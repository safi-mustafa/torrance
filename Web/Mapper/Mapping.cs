using AutoMapper;
using ViewModels;

namespace Models.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {

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
