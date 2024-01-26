using AutoMapper;
using VisReg.Web.Mappings;

namespace VisReg.Web
{
    public static class MappingConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<MappingViewModelToEntity>();
                x.AddProfile<MappingEntityToViewModel>();
            });
        }
    }
}