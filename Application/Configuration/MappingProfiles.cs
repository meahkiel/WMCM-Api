using Application.DTO;
using Application.DTOs;
using Core.Campaigns;
using Core.Contacts;
using Core.Tasks;

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

            CreateMap<Contact, ContactListDTO>()
                .ForMember(c => c.Title, o => o.MapFrom(c => c.ToGenderString()))
                .ForMember(c => c.Gender, o => o.MapFrom(c => c.ToGenderString()))
                .ForMember(c => c.FullName, o => o.MapFrom(c => (c.ToFullName())));

            CreateMap<Activity, ActivityEntryDTO>()
                .ForMember(a => a.Title, o => o.MapFrom(o => o.Title))
                .ForMember(a => a.Type, o => o.MapFrom(o => o.Type))
                .ForMember(a => a.DateToSend, o => o.MapFrom(o => o.DispatchDate))
                .ReverseMap();

            CreateMap<Campaign, CampaignDTO>()
                .ForMember(c => c.Activities, o => o.MapFrom(o => o.Activities))
                .ReverseMap();
            

            CreateMap<SubTask, MarketingSubTaskDTO>().ReverseMap();

            CreateMap<MarketingTask, MarketingTaskDTO>()
                .ForMember(c => c.SubTasks, o => o.MapFrom(o => o.SubTasks))
                .ReverseMap();

        }
    }
}
