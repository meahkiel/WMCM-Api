using Application.DTO;
using AutoMapper;
using Core.Campaigns;
using Core.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Contact, ContactFormDTO>()
                .ForMember(c => c.Id, o => o.MapFrom(c => c.Id))
                .ForMember(c => c.Gender, o => o.MapFrom(c => c.ToGenderString()))
                .ForMember(c => c.Title, o => o.MapFrom(c => c.ToTitleString()));

            CreateMap<ContactFormDTO, Contact>()
                .ForMember(c => c.Title, o => o.MapFrom(c => c.ToSalutation()))
                .ForMember(c => c.Gender, o => o.MapFrom(c => c.ToGender()));

            CreateMap<Campaign, CampaignDTO>().ReverseMap();

            CreateMap<Activity, ActivityEntryDTO>()
                .ForMember(a => a.Title, o => o.MapFrom(o => o.Title))
                .ForMember(a => a.Type, o => o.MapFrom(o => o.Type));
            
        }
    }
}
