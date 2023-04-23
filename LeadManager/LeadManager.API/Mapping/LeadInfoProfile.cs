using AutoMapper;
using LeadManager.Core.Entities;
using LeadManager.Core.Entities.Source;
using LeadManager.Core.ViewModels;

namespace LeadManager.API.Profiles
{
    public class LeadInfoProfile : Profile
    {
        public LeadInfoProfile()
        {
            #region User
            CreateMap<User, UserForCreateDto>().ReverseMap();
            CreateMap<User, UserForDisplayDto>();
            #endregion

            #region Source
            CreateMap<Source, SourceDto>().ForMember(s => s.Id, opt => opt.MapFrom(sd => sd.SourceId)).ReverseMap();
            CreateMap<Source, SourceForCreateDto>().ReverseMap();
            CreateMap<Source, SourceForUpdateDto>().ReverseMap();
            #endregion

            #region Supplier
            CreateMap<Supplier, SupplierDto>().ForMember(s => s.Id, opt => opt.MapFrom(sd => sd.SupplierId)).ReverseMap();
            CreateMap<Supplier, SupplierForCreateDto>().ReverseMap();
            CreateMap<Supplier, SupplierForUpdateDto>().ReverseMap();
            #endregion

            #region Lead 
            CreateMap<Lead,LeadDto>().ForMember(l => l.Id, opt => opt.MapFrom(ld => ld.LeadId));
            CreateMap<Lead,LeadForCreateDto>()
                .ForMember(l => l.SourceId, opt => opt.MapFrom(lfcd => lfcd.SourceId))
                .ReverseMap()
                .ForMember(l => l.SupplierId, opt => opt.MapFrom(lfcd => lfcd.SupplierId))
                .ReverseMap();
            CreateMap<Lead, LeadForUpdateDto>()
                .ForMember(l => l.SourceId, opt => opt.MapFrom(lfud => lfud.SourceId))
                .ReverseMap()
                .ForMember(l => l.SupplierId, opt => opt.MapFrom(lfud => lfud.SupplierId))
                .ReverseMap();
            #endregion
        }
    }
}
