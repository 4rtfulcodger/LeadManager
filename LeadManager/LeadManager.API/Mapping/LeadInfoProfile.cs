using AutoMapper;
using LeadManager.API.Models;
using LeadManager.Core.Entities;

namespace LeadManager.API.Profiles
{
    public class LeadInfoProfile : Profile
    {
        public LeadInfoProfile()
        {
            CreateMap<Source, SourceDto>().ForMember(s => s.Id, opt => opt.MapFrom(sd => sd.SourceId));
            CreateMap<Supplier, SupplierDto>().ForMember(s => s.Id, opt => opt.MapFrom(sd => sd.SupplierId));

            CreateMap<Lead,LeadDto>().ForMember(l => l.Id, opt => opt.MapFrom(ld => ld.LeadId));
            CreateMap<Lead,LeadForCreateDto>()
                .ForMember(l => l.SourceId, opt => opt.MapFrom(lfcd => lfcd.SourceId))
                .ReverseMap()
                .ForMember(l => l.SupplierId, opt => opt.MapFrom(lfcd => lfcd.SupplierId))
                .ReverseMap(); 
        }
    }
}
