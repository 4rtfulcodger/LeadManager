using AutoMapper;
using LeadManager.Core.Entities;
using LeadManager.Core.Entities.Source;
using LeadManager.Core.Entities.Supplier;
using LeadManager.Core.Models;
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
            CreateMap<Source, SourceDto>();
            CreateMap<Source, SourceForCreateDto>().ReverseMap();
            CreateMap<Source, SourceForUpdateDto>().ReverseMap();
            #endregion

            #region Supplier
            CreateMap<Supplier, SupplierDto>();
            CreateMap<Supplier, SupplierForCreateDto>().ReverseMap();
            CreateMap<Supplier, SupplierForUpdateDto>().ReverseMap();
            #endregion

            #region Contact

            CreateMap<Contact, ContactDto>();
            CreateMap<Contact, ContactForCreateDto>().ReverseMap();

            #endregion

            #region PhoneNumber

            CreateMap<PhoneNumber, PhoneNumberDto>();
            CreateMap<PhoneNumber, PhoneNumberForCreateDto>().ReverseMap();

            #endregion

            #region Address

            CreateMap<Address, AddressDto>();
            CreateMap<Address, AddressForCreateDto>().ReverseMap();

            #endregion


            #region Lead 

            CreateMap<LeadType, LeadTypeDto>().ReverseMap();            

            CreateMap<Lead,LeadDto>().ForMember(l => l.Id, opt => opt.MapFrom(ld => ld.LeadId));
            CreateMap<Lead,LeadForCreateDto>()
                .ForMember(l => l.SourceId, opt => opt.MapFrom(lfcd => lfcd.SourceId))
                .ReverseMap()
                .ForMember(l => l.SupplierId, opt => opt.MapFrom(lfcd => lfcd.SupplierId))
                .ReverseMap()
                .ForMember(l => l.LeadTypeId, opt => opt.MapFrom(lfcd => lfcd.LeadTypeId));
            CreateMap<Lead, LeadForUpdateDto>()
                .ForMember(l => l.SourceId, opt => opt.MapFrom(lfud => lfud.SourceId))
                .ReverseMap()
                .ForMember(l => l.SupplierId, opt => opt.MapFrom(lfud => lfud.SupplierId))
                .ReverseMap();
            #endregion

            #region LeadType

            CreateMap<LeadType, LeadTypeForCreateDto>().ReverseMap();
            CreateMap<LeadType, LeadTypeForUpdateDto>().ReverseMap();

            #endregion

            #region LeadAttribute

            CreateMap<LeadAttribute, LeadAttributeDto>().ReverseMap();
            CreateMap<LeadAttribute, LeadAttributeForCreateDto>().ReverseMap();
            CreateMap<LeadAttribute, LeadAttributeForUpdateDto>().ReverseMap();

            #endregion
        }
    }
}
