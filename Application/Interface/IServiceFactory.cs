using Application.DTO;
using Core.Campaigns;

namespace Application.Interface
{
    public interface IServiceFactory
    {
        IActivityServiceAccessor<ActivityEntryDTO,Activity> GetWebPost();
        IActivityServiceAccessor<ActivityEntryDTO, Activity> GetSMS();
        IActivityServiceAccessor<ActivityEntryDTO, Activity> GetSMTP();



    }
}
